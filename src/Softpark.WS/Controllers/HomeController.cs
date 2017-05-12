using System.Web.Mvc;

namespace Softpark.WS.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : Controller
    {
        // GET: Home
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return Redirect(Url.Content("~/swagger"));
        }
    }
}