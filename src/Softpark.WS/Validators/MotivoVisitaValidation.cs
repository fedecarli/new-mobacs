using Softpark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Softpark.WS.Validators
{
    /// <summary>
    /// Cns Validator
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MotivoVisitaValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            var motivos = value as IEnumerable<long>;

            if (motivos == null || motivos.Count() == 0) return false;

            return DomainContainer.Current.SIGSM_MotivoVisita.Count(x => motivos.Contains(x.codigo)) == motivos.Count();
        }
    }
}
