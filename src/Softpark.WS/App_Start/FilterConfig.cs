using Softpark.WS.Validators;
using System.Web.Mvc;

#pragma warning disable 1591
namespace Softpark.WS
{
    /// <summary>
    /// Configura os filtros padrões
    /// </summary>
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AddCustomHeaderFilter());
        }
    }
}
