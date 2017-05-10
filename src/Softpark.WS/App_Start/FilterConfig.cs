using System.Web.Mvc;

#pragma warning disable 1591
namespace Softpark.WS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ApiHandleErrorAttribute());
        }
    }
}
