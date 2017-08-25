using Softpark.Models;
using System.Threading.Tasks;
using System.Web.Http;

namespace Softpark.WS.Controllers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
        protected ASPSessionVar Session { get; private set; }

        protected BaseApiController(DomainContainer domain)
        {
            Domain = domain;
        }

        protected bool Autenticado()
        {
            Session = new ASPSessionVar(Url);
            
            return Session.Read("acesso") == "True";
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}