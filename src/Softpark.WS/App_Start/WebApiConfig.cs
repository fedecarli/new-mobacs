using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Softpark.WS.Validators;
using Softpark.WS.ViewModels;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

#pragma warning disable 1591
namespace Softpark.WS
{
    /// <summary>
    /// Configuração da aplicação
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registra a configuração
        /// </summary>
        /// <param name="config"></param>
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

            // define o formato padrão de data nos endpoitns
            config.Formatters.JsonFormatter.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            config.Formatters.JsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ssZ";
            
            ((DefaultContractResolver)config.Formatters.JsonFormatter.SerializerSettings.ContractResolver).IgnoreSerializableAttribute = true;
        }
    }
}
