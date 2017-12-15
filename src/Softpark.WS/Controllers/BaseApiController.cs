using Softpark.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
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
    }

    /// <summary>
    /// Base Ajax Controller
    /// </summary>
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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var asp = ConfigurationManager.AppSettings["aspDbConnPath"].Replace("_config", @"\v2\_inc\config.asp");

            var content = System.IO.File.ReadAllText(asp);

            var keys = new[]
            {
                "idPaisCliente",
                "nomePaisCliente",
                "idMunicipioCliente",
                "nomeCliente",
                "estadoCliente",
                "siglaEstadoCliente",
                "nomeInicialSistema",
                "logoInicialSistema",
                "exibirMensagemSistema",
                "mensagemSistema",
                "rodapeMensagemSistema",
                "corPrincipalSistema",
                "corMenuTop",
                "corMenuTopHover",
                "corLinks",
                "corHoverLinks",
                "corHoverMenu",
                "corFundoMenu",
                "corFundoMenuActive",
                "corFundoAbaActive"
            };

            var getValue = new Regex("^(.+ = \")([^\"]+)(\".*)$");

            keys.Where(x => content.Split('\n').Any(y => y.Contains(x)))
                .AsParallel().ForAll(x =>
                {
                    var lines = content.Split('\n');
                    var line = lines.FirstOrDefault(y => y.Contains(x));
                    if (line == null)
                    {
                        ViewData.Add(x, string.Empty);
                        return;
                    }

                    var value = getValue.Replace(line, "$2") ?? string.Empty;

                    if (x == "logoInicialSistema")
                    {
                        value = (Domain.SIGSM_ServicoSerializador_Config.SingleOrDefault(y => y.Configuracao == "sessionLocation")?.Valor ?? string.Empty)
                        .Replace("SessionVar.asp", value);
                    }

                    ViewData.Add(x, value);
                });

            if (ASPSessionVar.Read("acesso") != "True")
                HttpContext.Response.Redirect("~/../", true);

            ViewBag.idUsuario = Convert.ToInt32(ASPSessionVar.Read("idUsuario") ?? "0");
            ViewBag.idSistema = Convert.ToInt32(ASPSessionVar.Read("idSistema") ?? "0");
            ViewBag.Setor = ASPSessionVar.Read("setor");
            ViewBag.NomeSistema = ASPSessionVar.Read("NomeSistema");
            ViewBag.Usuario = ASPSessionVar.Read("usuario");
            ViewBag.Domain = Domain;
            ViewBag.Alert = ASPSessionVar.Read("alert");
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}