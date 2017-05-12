using System.Web.Mvc;

namespace Softpark.WS
{
    public class ApiHandleErrorAttribute : HandleErrorAttribute
    {
        public string Source { get; set; }

        public ApiHandleErrorAttribute() : base() {}

        public override void OnException(ExceptionContext filterContext)
        {
            Source = filterContext.Exception.Source;

            base.OnException(filterContext);
        }
    }
}