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
                "Default",
                namespaces: new[] { typeof(HelpController).Namespace },
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
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
