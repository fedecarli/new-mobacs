using Softpark.Infrastructure.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Softpark.WS.Validators
{
    /// <summary>
    /// Cns Validator
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CnsValidationAttribute : ValidationAttribute
    {
        private bool _canBeNull;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canBeNull"></param>
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
