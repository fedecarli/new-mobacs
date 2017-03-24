using System.Web.Mvc;

namespace Softpark.WS.Areas.AjaxTemplates
{
    public class AjaxTemplatesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AjaxTemplates";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AjaxTemplates_default",
                "AjaxTemplates/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}