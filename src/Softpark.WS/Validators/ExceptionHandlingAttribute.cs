using log4net;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Softpark.WS.Validators
{
    /// <summary>
    /// Track de exceções não tratadas
    /// </summary>
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Realiza o log das exceções
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            //Log Critical errors
            Debug.WriteLine(context.Exception);

            Log.Error(String.Format("Unhandled exception thrown in {0} for request {1}: {2}", context.Request.Method, context.Request.RequestUri, context.Exception));

            if (context.Exception is System.ComponentModel.DataAnnotations.ValidationException)
            {
                var e = context.Exception as System.ComponentModel.DataAnnotations.ValidationException;
                var msgs = e.Message;

                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(msgs),
                    ReasonPhrase = "Validation Error"
                });
            }
            else if (context.Exception is System.Data.Entity.Validation.DbEntityValidationException)
            {
                var e = context.Exception as System.Data.Entity.Validation.DbEntityValidationException;
                var msgs = e.EntityValidationErrors.SelectMany(a => a.ValidationErrors.Select(b => b.ErrorMessage)).Aggregate((a, b) => $"{a}\n\n{b}");

                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(msgs),
                    ReasonPhrase = "Validation Error"
                });
            }
        }
    }
}