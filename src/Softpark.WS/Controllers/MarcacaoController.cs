using System.Web.Mvc;

namespace Softpark.WS.Controllers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class MarcacaoController : BaseAjaxController
    {
        public MarcacaoController() : base(new Models.DomainContainer()) { }

        // GET: Marcacao
        public ActionResult Index()
        {
            return View();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}