using System.Web.Http;
using WebActivatorEx;
using Softpark.WS;
using Swashbuckle.MVC.Handler;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
//[assembly: PreApplicationStartMethod(typeof(SwaggerMVCConfig), "Register")]
namespace Softpark.WS
{
    public class SwaggerMVCConfig
    {
		public static void Register()
        {
			DynamicModuleUtility.RegisterModule(typeof(SwashbuckleMVCModule));
		}
	}
}