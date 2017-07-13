using Softpark.Infrastructure.Extras;
using Softpark.Models;
using Softpark.WS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;

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
        protected ProcessosController() : base(new DomainContainer()) { }

        public ProcessosController(DomainContainer domain) : base(domain)
        {
        }

        private static log4net.ILog Log { get; set; } = log4net.LogManager.GetLogger(typeof(ProcessosController));

        private async Task<FichaVisitaDomiciliarMaster> GetOrCreateMaster(Guid token)
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

            if (master.uuidFicha != null) return master;
            master.tpCdsOrigem = 3;
            master.UnicaLotacaoTransport = header;

            master.uuidFicha = header.cnes + '-' + Guid.NewGuid();
            Domain.FichaVisitaDomiciliarMaster.Add(master);

            await Domain.SaveChangesAsync();

            return master;
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
            Log.Info("----");
            Log.Info("enviar/cabecalho");
            var serializer = new JavaScriptSerializer();
            Log.Info(serializer.Serialize(header));

            var origem = Domain.OrigemVisita.Create();

            origem.token = Guid.NewGuid();
            origem.id_tipo_origem = 1;
            origem.enviarParaThrift = true;
            origem.enviado = false;

            Domain.OrigemVisita.Add(origem);

            var transport = header.ToModel(Domain);

            transport.id = Guid.NewGuid();
            transport.OrigemVisita = origem;
            transport.token = origem.token;

            transport.Validar(Domain);

            Domain.UnicaLotacaoTransport.Add(transport);

            await Domain.SaveChangesAsync();

            Log.Info(origem.token);
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
            Log.Info("-----");
            Log.Info($"GET api/visita/child/");

            var serializer = new JavaScriptSerializer();
            Log.Info(serializer.Serialize(child));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var master = await GetOrCreateMaster(child.token ?? Guid.Empty);

            var ficha = child.ToModel(Domain);

            foreach (var motivoId in child.motivosVisita)
            {
                var motivo = await Domain.SIGSM_MotivoVisita.FindAsync(motivoId);

                if (motivo != null)
                    ficha.SIGSM_MotivoVisita.Add(motivo);
            }

            Domain.FichaVisitaDomiciliarChild.Add(ficha);

            if (ficha.dtNascimento != null)
                ficha.dtNascimento.Value.IsValidBirthDateTime(master.UnicaLotacaoTransport.dataAtendimento);

            ficha.FichaVisitaDomiciliarMaster = master;

            ficha.Validar();

            try
            {
                await Domain.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Fatal(e.Message);

                if (e is DbEntityValidationException)
                {
                    var ex = (e as DbEntityValidationException).EntityValidationErrors;
                    throw new Exception(ex.First().ValidationErrors.First().ErrorMessage, e);
                }

                throw e;
            }

            Log.Info(Ok(true));
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
            Log.Info("-----");
            Log.Info($"GET api/cadastro/individual/{cadInd.token}");

            var serializer = new JavaScriptSerializer();
            Log.Info(serializer.Serialize(cadInd));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var origem = await Domain.OrigemVisita.FindAsync(cadInd.token);

            var header = origem?.UnicaLotacaoTransport?.FirstOrDefault();

            if (origem == null || origem.finalizado || header == null)
            {
                throw new InvalidOperationException("Token inválido. Inicie o processo de transmissão.");
            }

            await ProcessarIndividuos(new[] { cadInd }, header, true);

            var cad = await cadInd.ToModel(Domain);
            cad.tpCdsOrigem = 3;
            cad.UnicaLotacaoTransport = header;

            cad.Validar(Domain);

            Domain.CadastroIndividual.Add(cad);

            await Domain.SaveChangesAsync();

            Log.Info(Ok(true));
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
            Log.Info("-----");
            Log.Info("POST enviar/cadastro/domiciliar");
            Log.Info($"TOKEN {cadDom.token}");

            var serializer = new JavaScriptSerializer();
            Log.Info(serializer.Serialize(cadDom));

            if (!ModelState.IsValid)
            {
                Log.Fatal(ModelState);
                return BadRequest(ModelState);
            }

            var origem = await Domain.OrigemVisita.FindAsync(cadDom.token);

            if (origem == null || origem.finalizado)
            {
                Log.Fatal("Token inválido. Inicie o processo de transmissão.");
                throw new ValidationException("Token inválido. Inicie o processo de transmissão.");
            }

            var header = origem.UnicaLotacaoTransport.FirstOrDefault();
            var cad = await cadDom.ToModel(Domain);
            cad.tpCdsOrigem = 3;
            cad.UnicaLotacaoTransport = header;
            if (header == null)
            {
                Log.Fatal("Token inválido. Inicie o processo de transmissão.");
                throw new ValidationException("Token inválido. Inicie o processo de transmissão.");
            }

            await cad.Validar(Domain);

            Domain.CadastroDomiciliar.Add(cad);

            await Domain.SaveChangesAsync();

            Log.Info(Ok(true));
            return Ok(true);
        }

        /// <summary>
        /// Cadastro Atômico
        /// </summary>
        /// <param name="cadastros"></param>
        /// <returns></returns>
        [Route("api/processos/enviar/cadastro/atomico")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> CadastramentoAtomico([FromBody, Required] AtomicTransporViewModel[] cadastros)
        {
            Log.Info("-----");
            Log.Info("POST api/processos/enviar/cadastro/atomico");

            var serializer = new JavaScriptSerializer();
            Log.Info(serializer.Serialize(cadastros));

            var origem = Domain.OrigemVisita.Create();

            origem.token = Guid.NewGuid();
            origem.id_tipo_origem = 1;
            origem.enviarParaThrift = true;
            origem.enviado = false;

            Domain.OrigemVisita.Add(origem);

            await Domain.SaveChangesAsync();

            try
            {
                foreach (var cadastro in cadastros)
                {
                    var header = cadastro.cabecalho.ToModel(Domain);

                    header.id = Guid.NewGuid();
                    header.OrigemVisita = origem;
                    header.token = origem.token;

                    Domain.UnicaLotacaoTransport.Add(header);

                    await ProcessarIndividuos(cadastro.individuos.Where(x => x.identificacaoUsuarioCidadao != null &&
                    x.identificacaoUsuarioCidadao.statusEhResponsavel), header);

                    await ProcessarIndividuos(cadastro.individuos.Where(x => x.identificacaoUsuarioCidadao == null ||
                    !x.identificacaoUsuarioCidadao.statusEhResponsavel), header);

                    foreach (var domicilio in cadastro.domicilios)
                    {
                        var cad = await domicilio.ToModel(Domain);
                        cad.tpCdsOrigem = 3;
                        cad.UnicaLotacaoTransport = header;

                        Domain.CadastroDomiciliar.Add(cad);
                    }

                    var masters = new List<FichaVisitaDomiciliarMaster>();

                    Func<FichaVisitaDomiciliarMaster> getOrCreateMaster = () =>
                    {
                        var master = masters.FirstOrDefault(x => x.FichaVisitaDomiciliarChild.Count < 99) ?? Domain.FichaVisitaDomiciliarMaster.Create();

                        if (master.uuidFicha != null) return master;

                        master.tpCdsOrigem = 3;
                        master.UnicaLotacaoTransport = header;

                        master.uuidFicha = header.cnes + '-' + Guid.NewGuid();

                        Domain.FichaVisitaDomiciliarMaster.Add(master);

                        return master;
                    };

                    foreach (var child in cadastro.visitas)
                    {
                        var master = getOrCreateMaster();

                        var ficha = child.ToModel(Domain);

                        foreach (var motivoId in child.motivosVisita)
                        {
                            var motivo = await Domain.SIGSM_MotivoVisita.FindAsync(motivoId);

                            if (motivo != null)
                                ficha.SIGSM_MotivoVisita.Add(motivo);
                        }

                        Domain.FichaVisitaDomiciliarChild.Add(ficha);

                        if (ficha.dtNascimento != null)
                            Epoch.ValidateBirthDateTime(ficha.dtNascimento.Value, master.UnicaLotacaoTransport.dataAtendimento);

                        ficha.FichaVisitaDomiciliarMaster = master;

                        //ficha.Validar();
                    }
                }
            }
            catch (Exception e)
            {
                Log.Fatal(e.Message);

                if (e is DbEntityValidationException)
                {
                    var ex = (e as DbEntityValidationException).EntityValidationErrors;
                    throw new Exception(ex.First().ValidationErrors.First().ErrorMessage, e);
                }

                throw e;
            }

            try
            {
                await Domain.SaveChangesAsync();

                var uni = origem.UnicaLotacaoTransport.First();

                var usuario = Domain.VW_Profissional.FirstOrDefault(x => x.CNS == uni.profissionalCNS);

                Domain.RastroFicha.Add(new RastroFicha
                {
                    CodUsu = usuario.CodUsu,
                    DataModificacao = DateTime.Now,
                    token = origem.token,
                    DadoAtual = await Request.Content.ReadAsStringAsync()
                });

                await Domain.SaveChangesAsync();

                //Domain.PR_ProcessarFichasAPI(origem.token);
            }
            catch (Exception e)
            {
                Log.Fatal(e.Message);

                if (e is DbEntityValidationException)
                {
                    var ex = (e as DbEntityValidationException).EntityValidationErrors;
                    throw new Exception(ex.First().ValidationErrors.First().ErrorMessage, e);
                }

                throw e;
            }

            return Ok(true);
        }

        private async Task ProcessarIndividuos(IEnumerable<PrimitiveCadastroIndividualViewModel> individuos,
            UnicaLotacaoTransport header, bool validar = false)
        {
            foreach (var individuo in individuos)
            {
                var cad = await individuo.ToModel(Domain);
                cad.tpCdsOrigem = 3;
                cad.UnicaLotacaoTransport = header;

                if (validar) cad.Validar(Domain);

                Domain.CadastroIndividual.Add(cad);
            }
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
            Log.Info("-----");
            Log.Info("POST api/encerrar/token");

            var serializer = new JavaScriptSerializer();
            Log.Info(serializer.Serialize(token));

            var origem = await Domain.OrigemVisita.FindAsync(token);

            if (origem == null || origem.finalizado)
            {
                throw new ValidationException("Token inválido. Inicie o processo de transmissão.");
            }
            else if (origem.UnicaLotacaoTransport.Sum(x => x.FichaVisitaDomiciliarMaster.Count + x.CadastroDomiciliar.Count + x.CadastroIndividual.Count) > 0)
            {
                var uni = origem.UnicaLotacaoTransport.First();

                var usuario = Domain.VW_Profissional.FirstOrDefault(x => x.CNS == uni.profissionalCNS);

                Domain.RastroFicha.Add(new RastroFicha
                {
                    CodUsu = usuario.CodUsu,
                    DataModificacao = DateTime.Now,
                    OrigemVisita = origem,
                    token = origem.token
                });

                await Domain.SaveChangesAsync();

                //Domain.PR_ProcessarFichasAPI(token);
            }
            else
            {
                try
                {
                    origem.finalizado = true;
                    await Domain.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Log.Warn("Erro ao tentar finalizar token.", e);
                }
            }
            
            Log.Info(Ok(true));
            return Ok(true);
        }
    }
}