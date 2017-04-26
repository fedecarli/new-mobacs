using Softpark.Models;
using System.Web.Mvc;
using System.Linq;
using System.Web.Http;
using System.ComponentModel.DataAnnotations;
using Softpark.WS.Helpers;
using System;

namespace Softpark.WS.Areas.AjaxTemplates.Controllers
{
    public class CabecalhoController : Controller
    {
        DomainContainer db = new DomainContainer();

        [System.Web.Mvc.Route("{cnes:string}/{numContrato:int}")]
        public ActionResult Index([FromUri, Required] string cnes, [FromUri, Required] int numContrato)
        {
            var setores = db.Setores;
            var pares = db.AS_SetoresPar;

            var setor = (from a in setores
                         join b in pares on a.CodSetor equals b.CodSetor
                         where b.NumContrato == numContrato && b.NumContrato == a.NumContrato && b.CNES == cnes
                         orderby b.CNES
                         select new { a.DesSetor, b.CNES }).FirstOrDefault();
            ViewBag.setor = setor;
            return View();
        }

        public JavaScriptResult Script()
        {
            return this.JScript();
        }
    }
}