using Softpark.Infrastructure.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Softpark.WS.Validators
{
    /// <summary>
    /// Cns Validator
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CnsValidationAttribute : ValidationAttribute
    {
        private bool _canBeNull;

        public CnsValidationAttribute(bool canBeNull = false)
        {
            _canBeNull = canBeNull;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            return _canBeNull && value == null || (value != null && Cns.isValidCns(value.ToString()));
        }
    }
}
