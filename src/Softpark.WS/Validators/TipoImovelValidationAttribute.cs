using Softpark.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Softpark.WS.Validators
{
    /// <summary>
    /// Cns Validator
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TipoImovelValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            var ok = int.TryParse(value.ToString(), out int codigo);

            var hasImovel = ok && DomainContainer.Current.TP_Imovel.Any(x => x.codigo == codigo);

            return (value != null && hasImovel);
        }
    }
}
