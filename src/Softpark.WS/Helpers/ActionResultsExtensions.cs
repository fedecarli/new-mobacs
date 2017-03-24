using RazorEngine;
using System;
using System.IO;
using System.Web.Mvc;

namespace Softpark.WS.Helpers
{
    public static class ActionResultsExtensions
    {
        public static JavaScriptResult JScript(this Controller controller, string viewPath, object model)
        {
            var script = viewPath ?? (controller.Request.MapPath("~/Areas/AjaxTemplates/Views/" + controller.RouteData.GetRequiredString("controller") + "/"
                + controller.RouteData.GetRequiredString("action") + ".js"));
            var template = File.ReadAllText(script);
            
            var bag = new RazorEngine.Templating.DynamicViewBag(controller.ViewData);

            var scriptContent = Razor.Parse(template, model, bag, Guid.NewGuid().ToString());

            return new JavaScriptResult() { Script = scriptContent };
        }

        public static JavaScriptResult JScript(this Controller controller, object model)
        {
            return controller.JScript(null, model);
        }

        public static JavaScriptResult JScript(this Controller controller, string viewPath)
        {
            return controller.JScript(viewPath, null);
        }

        public static JavaScriptResult JScript(this Controller controller)
        {
            return controller.JScript(null, null);
        }
    }
}