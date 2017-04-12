using Softpark.Infrastructure.Extras;
using Softpark.Models;
using Softpark.WS.ViewModels;
using System;
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

            await Domain.SaveChangesAsync();

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
            var master = await GetOrCreateMaster(child.token);

            var ficha = child.ToModel();

            foreach (var motivoId in child.motivosVisita)
            {
                var motivo = await Domain.SIGSM_MotivoVisita.FindAsync(motivoId);

                if (motivo != null)
                    ficha.SIGSM_MotivoVisita.Add(motivo);
            }

            Domain.FichaVisitaDomiciliarChild.Add(ficha);

            if (ficha.dtNascimento != null)
                Epoch.ValidateBirthDate(child.dtNascimento ?? 0, master.UnicaLotacaoTransport.dataAtendimento);

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

        /// <summary>
        /// Alterar cabeçalho das fichas - Somente se autenticado
        /// </summary>
        /// <remarks>
        /// Todas as fichas usarão esse cabeçalho
        /// </remarks>
        /// <param name="token">Token do cabeçalho</param>
        /// <param name="header">Dados à serem enviados</param>
        [Route("alterar/cabecalho/{token}")]
        [HttpPut, ResponseType(typeof(bool))]
        [Authorize]
        public async Task<IHttpActionResult> AlterarCabecalho([FromUri, Required] Guid token, [FromBody, Required] UnicaLotacaoTransportCadastroViewModel header)
        {
            var origem = await Domain.OrigemVisita.FindAsync(token);

            var transport = header.ToModel();

            var cabecalho = origem.UnicaLotacaoTransport.Count > 1 ? null : origem.UnicaLotacaoTransport.SingleOrDefault();

            var t1 = cabecalho.CadastroDomiciliar.Count + cabecalho.CadastroIndividual.Count;
            var t2 = cabecalho.CadastroIndividual.Count + cabecalho.FichaVisitaDomiciliarMaster.Count;
            var t3 = cabecalho.CadastroDomiciliar.Count + cabecalho.FichaVisitaDomiciliarMaster.Count;
            
            if (cabecalho == null ||
                (cabecalho.CadastroDomiciliar.Count > 0 && cabecalho.CadastroIndividual.Count > 0) ||
                (cabecalho.CadastroDomiciliar.Count > 0 && cabecalho.FichaVisitaDomiciliarMaster.Count > 0) ||
                (cabecalho.CadastroIndividual.Count > 0 && cabecalho.FichaVisitaDomiciliarMaster.Count > 0))
                throw new ValidationException("Não é possível alterar este cabeçalho, há outras fichas utilizando ele.");

            cabecalho.profissionalCNS = transport.profissionalCNS;
            cabecalho.ine = transport.ine;
            cabecalho.dataAtendimento = transport.dataAtendimento;
            cabecalho.codigoIbgeMunicipio = transport.codigoIbgeMunicipio;
            cabecalho.cnes = transport.cnes;
            cabecalho.cboCodigo_2002 = transport.cboCodigo_2002;

            cabecalho.Validar();

            await Domain.SaveChangesAsync();

            return Ok(true);
        }
    }
}