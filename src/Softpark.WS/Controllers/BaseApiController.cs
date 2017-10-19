using Softpark.Models;
using Softpark.WS.Controllers.Api;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace Softpark.WS.Controllers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// Base Api Controller
    /// </summary>
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// Domain models
        /// </summary>
        protected DomainContainer Domain { get; private set; }

        protected BaseApiController(DomainContainer domain)
        {
            Domain = domain;
            DomainContainer.Current = domain;
        }

        protected bool Autenticado()
        {
            return ASPSessionVar.Read("acesso") == "True";
        }

        protected async Task<UsuarioVM> BuscarAcesso(Guid tokenAcesso)
        {

            var u = (await (from prf in Domain.VW_Profissional
                            join cad in Domain.ASSMED_Cadastro
                                     on prf.Codigo equals cad.Codigo
                            join usu in Domain.ASSMED_Usuario
                                     on prf.CodUsu equals usu.CodUsu
                            join acs in Domain.ASSMED_Acesso
                                     on usu.CodUsu equals acs.CodUsu
                            where prf.CodUsu != null
                               && usu.Login != null
                               && usu.Senha != null
                               && usu.Ativo == 1
                               && acs.DtSaida == null
#if !DEBUG
                              && prf.CBO == "515105" // somente se for um AGENTE COMUNITÁRIO DE SAÚDE
#endif
                            select new UsuarioVM { cad = cad, usu = usu, acs = acs, profissional = prf })
                      .ToArrayAsync())
                      .SingleOrDefault(x => x.acs.ASPSESSIONIDQASRTRQT == tokenAcesso.ToString()
                              && x.acs.EMail == $"{x.profissional.CBO}|{x.profissional.CNES}|{x.profissional.INE}");

            if (u == null)
                return null;

            var acesso = u.acs;

            var timer = (acesso.DtUltVer ?? acesso.DtAcesso) - acesso.DtAcesso;

            acesso.DtUltVer = DateTime.Now;

            if (timer.TotalDays >= 2)
            {
                acesso.DtSaida = DateTime.Now;

                await Domain.SaveChangesAsync();

                return null;
            }

            await Domain.SaveChangesAsync();

            return u;
        }

        protected async Task<UnicaLotacaoTransport> CriarCabecalho(UsuarioVM acesso, DateTime dataAtendimento)
        {
            var prof = acesso.profissional;

            var contrato = await Domain.ASSMED_Contratos.FirstAsync();

            var origem = new OrigemVisita
            {
                enviado = false,
                enviarParaThrift = true,
                finalizado = true,
                id_tipo_origem = 1,
                token = Guid.NewGuid()
            };

            Domain.OrigemVisita.Add(origem);

            var header = new UnicaLotacaoTransport
            {
                cboCodigo_2002 = prof.CBO.Trim(),
                cnes = prof.CNES.Trim(),
                codigoIbgeMunicipio = contrato.CodigoIbgeMunicipio,
                dataAtendimento = dataAtendimento,
                id = Guid.NewGuid(),
                ine = prof.INE?.Trim(),
                profissionalCNS = prof.CNS,
                OrigemVisita = origem,
                token = origem.token
            };

            Domain.UnicaLotacaoTransport.Add(header);

            await Domain.SaveChangesAsync();

            return header;
        }
    }

    /// <summary>
    /// Base Ajax Controller
    /// </summary>
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public abstract class BaseAjaxController : Controller
    {
        /// <summary>
        /// Domain models
        /// </summary>
        protected DomainContainer Domain { get; private set; }

        protected BaseAjaxController(DomainContainer domain)
        {
            Domain = domain;
            DomainContainer.Current = domain;
        }

        protected bool Autenticado()
        {
            return ASPSessionVar.Read("acesso") == "True";
        }

        protected ActionResult BadRequest(string message)
        {
            Response.StatusCode = 400;
            return Json(message);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}