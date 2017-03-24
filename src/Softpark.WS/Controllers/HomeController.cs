using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Softpark.WS.Controllers
{
#if DEBUG
    //[Authorize]
#endif
    public class HomeController : Controller
    {
        [HttpGet, Route("/Content/cliente.css")]
        public FileContentResult CustomCss()
        {
            var f = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/Content/cliente.tpl.css"));

            var props = typeof(Parametros).GetProperties();

            foreach (var p in props)
            {
                f.Replace($"<%={p.Name}%>", p.GetValue(null).ToString());
            }

            return File(Encoding.UTF8.GetBytes(f), "text/css");
        }

        // GET: Home
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
//#if !DEBUG
            return RedirectToAction("Index", "Help");
//#endif
            ViewData.Add("nomeInicialSistema", "Prefeitura Municipal de Itanhaém");
            ViewData.Add("logoInicialSistema", "img/brasao_itanhaem.jpg");

            var xIp = Request.ServerVariables["REMOTE_ADDR"];
            var nav = Request.ServerVariables["HTTP_USER_AGENT"];
            var xMessNav = "";
            if (nav.Contains("MSIE")) {
                if (nav.Contains("MSIE 8.0"))
                    xMessNav = "Seu Navegador não é compatível, utilize o <a href='https://www.google.com.br/chrome/browser/desktop/index.html'><img src='img/chrome.png' style='width: 17px;margin-top: -3px;'/> Google Chrome</a> ou <img src='img/iexplorer.png' style='width: 16px;margin-top: -3px;'/> Internet Explorer 10 ou superior!";
                else if (nav.Contains("MSIE 7.0"))
                    xMessNav = "Seu Navegador não é compatível, utilize o <a href='https://www.google.com.br/chrome/browser/desktop/index.html'><img src='img/chrome.png' style='width: 17px;margin-top: -3px;'/> Google Chrome</a> ou <img src='img/iexplorer.png' style='width: 16px;margin-top: -3px;'/> Internet Explorer 10 ou superior!";
                else if (nav.Contains("MSIE6.0"))
                    xMessNav = "Seu Navegador não é compatível, utilize o <a href='https://www.google.com.br/chrome/browser/desktop/index.html'><img src='img/chrome.png' style='width: 17px;margin-top: -3px;'/> Google Chrome</a> ou <img src='img/iexplorer.png' style='width: 16px;margin-top: -3px;'/> Internet Explorer 10 ou superior!";
                else
                    xMessNav = "Para uma melhor visualização, utilize o <a href='https://www.google.com.br/chrome/browser/desktop/index.html'><img src='img/chrome.png' style='width: 17px;margin-top: -3px;'/> Google Chrome</a> ou <img src='img/iexplorer.png' style='width: 16px;margin-top: -3px;'/> Internet Explorer 10 ou superior!";
            }

            ViewData.Add("xMessNav", xMessNav);

            return View();
        }
    }
}