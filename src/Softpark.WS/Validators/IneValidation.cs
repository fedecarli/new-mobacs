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
    public class IneValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            var ine = value?.ToString();
            return DomainContainer.Current.SetoresINEs.Any(x => x.Numero == ine);
        }
    }
}
