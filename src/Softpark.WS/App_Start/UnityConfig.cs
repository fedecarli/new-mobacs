using Microsoft.Practices.Unity;
using Softpark.Models;
using System.Web.Http;
using System.Web.Mvc;
using Unity.WebApi;

namespace Softpark.WS
{
    /// <summary>
    /// Configuração de Unidade de Trabalho
    /// </summary>
    public static class UnityConfig
    {
        /// <summary>
        /// Método que executa a configuração
        /// </summary>
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<DomainContainer>(new HierarchicalLifetimeManager(), new InjectionFactory(c => {
                return new DomainContainer();
            }));

            var resolver = new UnityDependencyResolver(container);
            
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}