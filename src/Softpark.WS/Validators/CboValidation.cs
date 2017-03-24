using Softpark.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Softpark.WS.Validators
{
    /// <summary>
    /// Cbo Validator
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CboValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            string v = value?.ToString()?.Trim();
            return DomainContainer.Current.AS_ProfissoesTab.Any(x => x.CodProfTab != null && x.CodProfTab == v);
        }
    }
}
