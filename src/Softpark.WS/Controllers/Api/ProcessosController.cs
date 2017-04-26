using Softpark.Infrastructure.Extras;
using Softpark.Models;
using Softpark.WS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Softpark.WS.Controllers.Api
{
    /// <summary>
    /// Endpoints dos processos de importação de dados
    /// </summary>
    [Route("/api/processos")]
    [System.Web.Mvc.OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
    [System.Web.Mvc.SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class ProcessosController : BaseApiController
    {
        private async Task<FichaVisitaDomiciliarMaster> GetOrCreateMaster(Guid token)
        {
            try
            {
                var origem = await Domain.OrigemVisita.FindAsync(token);

                if (origem == null || origem.finalizado)
                {
                    throw new InvalidOperationException("Token inválido. Inicie o processo de transmissão.");
                }

                var header = await Domain.UnicaLotacaoTransport.FirstOrDefaultAsync(h => h.token == token);

                if (header == null)
                {
                    throw new InvalidOperationException("Token inválido. Inicie o processo de transmissão.");
                }

                var master = header.FichaVisitaDomiciliarMaster.FirstOrDefault(m => m.FichaVisitaDomiciliarChild.Count < 99) ??
                    Domain.FichaVisitaDomiciliarMaster.Create();

                if (master.uuidFicha == null)
                {
                    master.tpCdsOrigem = 3;
                    master.UnicaLotacaoTransport = header;

                    master.uuidFicha = header.cnes + '-' + Guid.NewGuid();
                    Domain.FichaVisitaDomiciliarMaster.Add(master);

                    await Domain.SaveChangesAsync();
                }

                return master;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// End point para carga de dados atômica
        /// </summary>
        /// <param name="atomic"></param>
        /// <returns></returns>
        [Route("enviar/cargaCompleta")]
        [HttpPost, ResponseType(typeof(bool))]
        private async Task<IHttpActionResult> EnviarCargaCompleta([FromBody, Required] AtomicTransporViewModel atomic)
        {
            var tokens = new List<OrigemVisita>();
            var errors = new List<Exception>();

            foreach (var cads in atomic.CadastrosIndividuais)
            {
                foreach (var ind in cads.Individuos)
                {
                    using (var trans = Domain.Database.BeginTransaction())
                    {
                        try
                        {
                            var origem = Domain.OrigemVisita.Create();
                            origem.enviarParaThrift = true;
                            origem.finalizado = true;
                            origem.id_tipo_origem = 1;
                            origem.token = Guid.NewGuid();

                            var cad = await ind.ToModel();
                            cad.UnicaLotacaoTransport = cads.Cabecalho.ToModel();
                            cad.UnicaLotacaoTransport.OrigemVisita = origem;
                            cad.UnicaLotacaoTransport.token = origem.token;

                            cad.Validar();

                            tokens.Add(origem);
                            Domain.OrigemVisita.Add(origem);
                            Domain.UnicaLotacaoTransport.Add(cad.UnicaLotacaoTransport);
                            Domain.CadastroIndividual.Add(cad);
                            Domain.SaveChanges();
                            trans.Commit();
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            errors.Add(e);
                            continue;
                        }
                    }
                }
            }

            foreach (var cads in atomic.CadastrosDomiciliares)
            {
                foreach (var dom in cads.Domicilios)
                {
                    using (var trans = Domain.Database.BeginTransaction())
                    {
                        try
                        {
                            var origem = Domain.OrigemVisita.Create();
                            origem.enviarParaThrift = true;
                            origem.finalizado = true;
                            origem.id_tipo_origem = 1;
                            origem.token = Guid.NewGuid();

                            var cad = await dom.ToModel();
                            cad.UnicaLotacaoTransport = cads.Cabecalho.ToModel();
                            cad.UnicaLotacaoTransport.OrigemVisita = origem;
                            cad.UnicaLotacaoTransport.token = origem.token;

                            await cad.Validar();

                            tokens.Add(origem);
                            Domain.OrigemVisita.Add(origem);
                            Domain.UnicaLotacaoTransport.Add(cad.UnicaLotacaoTransport);
                            Domain.CadastroDomiciliar.Add(cad);
                            Domain.SaveChanges();
                            trans.Commit();
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            errors.Add(e);
                            continue;
                        }
                    }
                }
            }

            foreach (var fichas in atomic.FichasDeVisitas)
            {
                OrigemVisita origem = null;
                UnicaLotacaoTransport header = null;
                FichaVisitaDomiciliarMaster master = null;

                foreach (var visita in fichas.Visitas)
                {
                    if (origem == null || header == null || (master = header.FichaVisitaDomiciliarMaster.FirstOrDefault(x => x.FichaVisitaDomiciliarChild.Count < 99)) == null)
                    {
                        using (var trans = Domain.Database.BeginTransaction())
                        {
                            try
                            {
                                origem = Domain.OrigemVisita.Create();
                                origem.enviarParaThrift = true;
                                origem.finalizado = true;
                                origem.id_tipo_origem = 1;
                                origem.token = Guid.NewGuid();

                                header = fichas.Cabecalho.ToModel();
                                header.OrigemVisita = origem;
                                header.token = origem.token;
                                header.id = Guid.NewGuid();

                                header.Validar();

                                tokens.Add(origem);
                                Domain.OrigemVisita.Add(origem);
                                Domain.UnicaLotacaoTransport.Add(header);

                                master = Domain.FichaVisitaDomiciliarMaster.Create();

                                master.UnicaLotacaoTransport = header;
                                master.tpCdsOrigem = 3;
                                master.uuidFicha = master.uuidFicha ?? (header.cnes + '-' + Guid.NewGuid());

                                master.Validar();

                                Domain.FichaVisitaDomiciliarMaster.Add(master);
                                Domain.SaveChanges();
                                trans.Commit();
                            }
                            catch (Exception e)
                            {
                                tokens.Remove(origem);
                                trans.Rollback();
                                errors.Add(e);
                                continue;
                            }
                        }
                    }

                    if (master != null)
                    {
                        using (var trans = Domain.Database.BeginTransaction())
                        {
                            try
                            {
                                var child = visita.ToModel();
                                child.FichaVisitaDomiciliarMaster = master;
                                child.uuidFicha = master.uuidFicha;

                                child.Validar();
                                master.FichaVisitaDomiciliarChild.Add(child);
                                Domain.FichaVisitaDomiciliarChild.Add(child);
                                Domain.SaveChanges();
                                trans.Commit();
                            }
                            catch (Exception e)
                            {
                                tokens.Remove(origem);
                                trans.Rollback();
                                errors.Add(e);
                                continue;
                            }
                        }
                    }
                }
            }

            await Domain.SaveChangesAsync();

            return Ok(true);
        }

        /// <summary>
        /// Cadastrar cabeçalho das fichas
        /// </summary>
        /// <remarks>
        /// Todas as fichas usarão esse cabeçalho
        /// </remarks>
        /// <param name="header">Dados à serem enviados</param>
        /// <returns>Token para envio das fichas</returns>
        [Route("enviar/cabecalho")]
        [HttpPost, ResponseType(typeof(Guid))]
        public async Task<IHttpActionResult> EnviarCabecalho([FromBody, Required] UnicaLotacaoTransportCadastroViewModel header)
        {
            var origem = Domain.OrigemVisita.Create();

            origem.token = Guid.NewGuid();
            origem.id_tipo_origem = User != null && User.Usuario() != null ? 2 : 1;
            origem.enviarParaThrift = true;
            origem.enviado = false;

            Domain.OrigemVisita.Add(origem);
            
            var transport = header.ToModel();

            transport.id = Guid.NewGuid();
            transport.OrigemVisita = origem;
            transport.token = origem.token;

            transport.Validar();

            Domain.UnicaLotacaoTransport.Add(transport);

            await Domain.SaveChangesAsync();

            return Ok(origem.token);
        }

        /// <summary>
        /// Envia uma ficha de visita (child) para cadastro
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        [Route("enviar/visita/child")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> EnviarFichaVisita([FromBody, Required] FichaVisitaDomiciliarChildCadastroViewModel child)
        {
            var master = await GetOrCreateMaster(child.token ?? Guid.Empty);

            var ficha = child.ToModel();

            foreach (var motivoId in child.motivosVisita)
            {
                var motivo = await Domain.SIGSM_MotivoVisita.FindAsync(motivoId);

                if (motivo != null)
                    ficha.SIGSM_MotivoVisita.Add(motivo);
            }

            Domain.FichaVisitaDomiciliarChild.Add(ficha);

            if (ficha.dtNascimento != null)
                Epoch.ValidateBirthDate(child.dtNascimento ?? 0, master.UnicaLotacaoTransport.dataAtendimento.ToUnix());

            ficha.FichaVisitaDomiciliarMaster = master;

            ficha.Validar();

            try
            {
                await Domain.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var ex = ((System.Data.Entity.Validation.DbEntityValidationException)e).EntityValidationErrors;
                throw new Exception(ex.First().ValidationErrors.First().ErrorMessage, e);
            }

            return Ok(true);
        }

        /// <summary>
        /// Cadastro Individual
        /// </summary>
        /// <param name="cadInd"></param>
        /// <returns></returns>
        [Route("enviar/cadastro/individual")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> EnviarCadastroIndividual([FromBody, Required] CadastroIndividualViewModel cadInd)
        {
            var origem = await Domain.OrigemVisita.FindAsync(cadInd.token);

            var header = origem?.UnicaLotacaoTransport?.FirstOrDefault();

            if (origem == null || origem.finalizado || header == null)
            {
                throw new InvalidOperationException("Token inválido. Inicie o processo de transmissão.");
            }

            var cad = await cadInd.ToModel();
            cad.tpCdsOrigem = 3;
            cad.UnicaLotacaoTransport = header;

            cad.Validar();

            Domain.CadastroIndividual.Add(cad);

            await Domain.SaveChangesAsync();

            return Ok(true);
        }

        /// <summary>
        /// Cadastro Domiciliar
        /// </summary>
        /// <param name="cadDom"></param>
        /// <returns></returns>
        [Route("enviar/cadastro/domiciliar")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> EnviarCadastroDomiciliar([FromBody, Required] CadastroDomiciliarViewModel cadDom)
        {
            var origem = await Domain.OrigemVisita.FindAsync(cadDom.token);

            if (origem == null || origem.finalizado)
            {
                throw new ValidationException("Token inválido. Inicie o processo de transmissão.");
            }

            var header = origem.UnicaLotacaoTransport.FirstOrDefault();
            var cad = await cadDom.ToModel();
            cad.tpCdsOrigem = 3;
            cad.UnicaLotacaoTransport = header ?? throw new ValidationException("Token inválido. Inicie o processo de transmissão.");

            await cad.Validar();

            Domain.CadastroDomiciliar.Add(cad);

            await Domain.SaveChangesAsync();

            return Ok(true);
        }

        /// <summary>
        /// Finaliza a transmissão de dados (encerra o token)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("encerrar/{token}")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> FinalizarTransmissao([FromUri, Required] Guid token)
        {
            var origem = await Domain.OrigemVisita.FindAsync(token);

            if (origem == null || origem.finalizado)
            {
                throw new ValidationException("Token inválido. Inicie o processo de transmissão.");
            }

            Domain.PR_ProcessarFichasAPI(token);

            return Ok(true);
        }
    }
}