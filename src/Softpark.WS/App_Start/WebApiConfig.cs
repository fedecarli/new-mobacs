using Newtonsoft.Json.Serialization;
using Softpark.WS.Validators;
using System.Web.Http;
using System.Web.Http.Cors;

#pragma warning disable 1591
namespace Softpark.WS
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //config.AddDbContext<Domain>();
            config.Filters.Add(new ExceptionHandlingAttribute());
            
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            log4net.Config.XmlConfigurator.Configure();

            // Web API routes
            config.MapHttpAttributeRoutes();
            
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{token}",
                new { token = RouteParameter.Optional }
            );

            ((DefaultContractResolver)config.Formatters.JsonFormatter.SerializerSettings.ContractResolver).IgnoreSerializableAttribute = true;
        }
    }
}
