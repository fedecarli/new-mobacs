using Softpark.WS.Areas.HelpPage.Controllers;
using Softpark.WS.Controllers;
using System.Web.Mvc;
using System.Web.Routing;

#pragma warning disable 1591
namespace Softpark.WS
{
    /// <summary>
    /// Configuração de rotas
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Login",
                url: "{anything}login.asp",
                defaults: new { controller = "Home", action = "Login", anything = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Ajax",
                url: "{anything}ajax/default.asp",
                defaults: new { controller = "Home", action = "Ajax", anything = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AjaxLogin",
                url: "{anything}ajax/login/verificaSessao.asp",
                defaults: new { controller = "Home", action = "AjaxLogin", anything = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Inc",
                url: "{anything}_inc/img/{anythings}",
                defaults: new { controller = "Home", action = "Inc", anything = UrlParameter.Optional, anythings = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Img",
                url: "{anything}img/{anythings}",
                defaults: new { controller = "Home", action = "Img", anything = UrlParameter.Optional, anythings = UrlParameter.Optional }
            );

            routes.MapRoute(
                "DefaultArea",
                namespaces: new[] { typeof(HelpController).Namespace },
                url: "",
                defaults: new { area = "HelpPage", controller = "Help", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "DefaultApiAction",
                "api/{controller}/{action}/{token}",
                new { token = UrlParameter.Optional }
            );
        }
    }
}
