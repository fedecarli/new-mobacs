using Softpark.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Softpark.WS.Validators
{
    /// <summary>
    /// Zoneamento Validator
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ZoneamentoValidationAttribute : ValidationAttribute
    {
        /// <inherit/>
        public override bool IsValid(object value)
        {
            if (value == null) return false;

            if (!decimal.TryParse(value.ToString(), out decimal codigo))
                return false;

            return DomainContainer.Current.VW_Cadastros_Zoneamento.Any(x => x.Codigo == codigo);
        }
    }
}
