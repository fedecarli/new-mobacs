using Softpark.WS.Areas.HelpPage.Controllers;
using System.Web.Mvc;
using System.Web.Routing;

#pragma warning disable 1591
namespace Softpark.WS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                namespaces: new[] { typeof(HelpController).Namespace },
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DefaultArea",
                namespaces: new[] { typeof(HelpController).Namespace },
                url: "",
                defaults: new { area = "HelpPage", controller = "Help", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DefaultApiAction",
                url: "api/{controller}/{action}/{token}",
                defaults: new { token = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ODataDefaultRoute",
                url: "api/odata/{controller}/{action}",
                defaults: new { action = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CustomCss",
                url: "Content/cliente.css",
                defaults: new { controller = "Home", action = "CustomCss" }
            );
        }
    }
}
