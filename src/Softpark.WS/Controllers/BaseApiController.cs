using Softpark.Models;
using System.Web.Http;

namespace Softpark.WS.Controllers
{
    /// <summary>
    /// Base Api Controller
    /// </summary>
    [System.Web.Mvc.SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// Domain models
        /// </summary>
        protected DomainContainer Domain { get; private set; }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected BaseApiController(DomainContainer domain)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            Domain = domain;
        }
    }
}