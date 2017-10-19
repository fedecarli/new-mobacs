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