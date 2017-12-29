using System.Web.Mvc;
using Softpark.Models;
using System.Linq;

namespace Softpark.WS.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : BaseAjaxController
    {
        /// <summary>
        /// 
        /// </summary>
        public HomeController() : base(new DomainContainer()) { }

        // GET: Home
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return Redirect(Url.Content("~/swagger"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Menu(string page)
        {
            var menu = 0;
            int? mFirst = null;
            int? mSecond = null;

            var ms = Domain.Get_VW_MenuSistema(page).FirstOrDefault();

            if (ms?.id_menu != null)
            {
                menu = ms.id_menu;

                mFirst = ms.id_pai_indireto ?? ms.id_pai_direto;
                mSecond = ms.id_pai_indireto == null ? null : ms.id_pai_direto;
            }
            else
            {
                //HttpContext.Response.Redirect(Url.Content("~/../"), true);
            }

            return PartialView("~/Views/Shared/_menu.cshtml", menu);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public RedirectResult Login()
        {
            var url = Domain.SIGSM_ServicoSerializador_Config.Find("sessionLocation").Valor.Replace("SessionVar.asp", "Esus/login.asp");

            return RedirectPermanent(url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public RedirectResult AjaxLogin()
        {
            var url = Domain.SIGSM_ServicoSerializador_Config.Find("sessionLocation").Valor.Replace("SessionVar.asp", "_inc/ajax/login/verificaSessao.asp");

            return RedirectPermanent(url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("/ajax/default.asp")]
        public RedirectResult Ajax()
        {
            var url = Domain.SIGSM_ServicoSerializador_Config.Find("sessionLocation").Valor.Replace("SessionVar.asp", "ESUS/ajax/default.asp");

            return RedirectPermanent(url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anything"></param>
        /// <returns></returns>
        public RedirectResult Inc(string anything)
        {
            var url = Domain.SIGSM_ServicoSerializador_Config.Find("sessionLocation").Valor.Replace("SessionVar.asp", $"_inc/img/{anything}");

            return RedirectPermanent(url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anything"></param>
        /// <returns></returns>
        public RedirectResult Img(string anything)
        {
            var url = Domain.SIGSM_ServicoSerializador_Config.Find("sessionLocation").Valor.Replace("SessionVar.asp", $"ESUS/img/{anything}");

            return RedirectPermanent(url);
        }
    }
}