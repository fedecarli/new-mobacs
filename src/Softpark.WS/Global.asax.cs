using System.Globalization;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web;
using System;
using Softpark.WS.Validators;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

#pragma warning disable 1591
namespace Softpark.WS
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            CultureInfo.DefaultThreadCurrentCulture =
            CultureInfo.DefaultThreadCurrentUICulture =
            Thread.CurrentThread.CurrentUICulture =
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("pt-BR");

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }

    public abstract class MessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
            var requestInfo = string.Format("{0} {1}", request.Method, request.RequestUri);

            var requestMessage = await request.Content.ReadAsByteArrayAsync();

            await IncommingMessageAsync(corrId, requestInfo, requestMessage);

            var response = await base.SendAsync(request, cancellationToken);

            byte[] responseMessage;

            if (response.IsSuccessStatusCode)
                responseMessage = await response.Content.ReadAsByteArrayAsync();
            else
                responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);

            await OutgoingMessageAsync(corrId, requestInfo, responseMessage);
            
            return response;
        }

        protected abstract Task IncommingMessageAsync(string correlationId, string requestInfo, byte[] message);
        protected abstract Task OutgoingMessageAsync(string correlationId, string requestInfo, byte[] message);
    }
}
