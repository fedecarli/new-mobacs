[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Softpark.WS.App_Start.NinjectWeb), "Start")]

#pragma warning disable 1591
namespace Softpark.WS.App_Start
{
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject.Web;

    public static class NinjectWeb 
    {
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
        }
    }
}
