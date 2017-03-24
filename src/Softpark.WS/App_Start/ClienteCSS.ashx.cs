using System;
using Softpark.WS.Controllers;
using System.Web;

namespace Softpark.WS
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ClienteCSS : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: https://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var f = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/cliente.tpl.css"));

            var props = typeof(Parametros).GetProperties();

            foreach (var p in props)
            {
                f = f.Replace($"<%={p.Name}%>", p.GetValue(null).ToString());
            }

            context.Response.ContentType = "text/css";
            context.Response.Write(f);
        }

        #endregion
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
