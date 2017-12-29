using Softpark.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Softpark.WS.Validators
{
    /// <summary>
    /// Cns Validator
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MicroAreaValidationAttribute : ValidationAttribute
    {
        private object _value;

        /// <inherit/>
        public override bool IsValid(object value)
        {
            _value = value;

            var ma = value?.ToString();

            return DomainContainer.Current.SIGSM_MicroAreas.Any(x => x.Codigo == ma);
        }

        /// <inherit/>
        public override string FormatErrorMessage(string name)
        {
            if (_value == null)
                ErrorMessage = "A Micro Área é obrigatória.";

            return (ErrorMessage == null || _value == null) ? base.FormatErrorMessage(name) :
                string.Format(ErrorMessage, _value);
        }
    }
}
