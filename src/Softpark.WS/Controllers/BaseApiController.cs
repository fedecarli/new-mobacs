using log4net;
using System.Web.Http;

namespace Softpark.WS.Controllers
{
    /// <summary>
    /// Base Api Controller
    /// </summary>
    [System.Web.Mvc.SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public abstract class BaseApiController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected static ILog Log => log;
    }
}