using System.Web.Mvc;

namespace Softpark.WS
{
    /// <summary>
    /// Atributo para track de erros
    /// </summary>
    public class ApiHandleErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// Origem
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public ApiHandleErrorAttribute() : base() {}

        /// <summary>
        /// Evento disparado em exceção
        /// </summary>
        /// <param name="filterContext">Contexto de exceção</param>
        public override void OnException(ExceptionContext filterContext)
        {
            Source = filterContext.Exception.Source;

            base.OnException(filterContext);
        }
    }
}