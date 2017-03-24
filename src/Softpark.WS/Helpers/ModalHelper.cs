using Softpark.Models;
using System.IO;

namespace System.Web.Mvc
{
    public static class ModalHelper
    {
        static string RenderRazorViewToString(this ControllerContext controllerContext, string helperName, string odataUri, string title = null, string[] fields = null)
        {
            var viewName = $"~/Views/Helpers/{helperName.Replace("Helper", "")}.cshtml";

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var viewContext = new ViewContext(controllerContext, viewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, sw);
                viewContext.ViewBag.fields = fields;
                viewContext.ViewBag.title = title;
                viewContext.ViewBag.odataUri = odataUri;
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        private static DomainContainer db => DomainContainer.Current;

        public static string ModalAjax(this HtmlHelper helper, string odataUri, string title = null, string[] fields = null)
        {
            return helper.ViewContext.RenderRazorViewToString(nameof(ModalHelper), odataUri, title, fields);
        }
    }
}