using System;
using System.Web.Mvc;

namespace Softpark.WS.Validators
{
    /// <summary>
    /// Handler de sobreposição de cabeçalho http
    /// </summary>
    public class AddCustomHeaderFilter : ActionFilterAttribute
    {
        /// <summary>
        /// sobreposição
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            context.HttpContext.Response.Headers.Add("x-server-time", DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZzzz"));
            context.HttpContext.Response.Headers.Add("x-api-version", Versions.Version);
        }
    }
}