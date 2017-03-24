using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace Softpark.WS.Validators
{
    public class SigApiControllerActionInvoker : ApiControllerActionInvoker
    {
        public override Task<HttpResponseMessage> InvokeActionAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            var result = base.InvokeActionAsync(actionContext, cancellationToken);

            if (result.Exception != null && result.Exception.GetBaseException() != null)
            {
                var baseException = result.Exception.GetBaseException() ?? result.Exception;

                if (baseException is ValidationException)
                {
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(baseException.Message),
                        ReasonPhrase = "Validation Error"
                    });
                }
                else if (baseException is System.Data.Entity.Validation.DbEntityValidationException)
                {
                    var e = baseException as System.Data.Entity.Validation.DbEntityValidationException;
                    var msgs = e.EntityValidationErrors.SelectMany(a => a.ValidationErrors.Select(b => b.ErrorMessage)).Aggregate((a, b) => $"{a}\n\n{b}");

                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(msgs),
                        ReasonPhrase = "Validation Error"
                    });
                }
                //else
                //{
                //    throw result.Exception;
                //    //Log critical error
                //    Debug.WriteLine(baseException);

                //    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.InternalServerError)
                //    {
                //        Content = new StringContent(baseException.Message + "\n\nStack Trace: " + baseException.StackTrace +
                //        (baseException.InnerException != null ? ("\n\nInner Exception: " + baseException.InnerException.Message + "\n\nInner Stack Trace: " + baseException.InnerException.StackTrace) : "")),
                //        ReasonPhrase = "Critical Error",
                //        RequestMessage = actionContext.Request
                //    });
                //}
            }
            //else
            //{
            //    throw result.Exception;
            //}

            return result;
        }
    }
}