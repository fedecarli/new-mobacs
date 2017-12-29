using Softpark.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Softpark.WS.Validators
{
    /// <summary>
    /// SIGSM_MicroArea_Unidade Validator
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class MAUValidationAttribute : ValidationAttribute
    {
        private bool _editing;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="editing"></param>
        public MAUValidationAttribute(bool editing = false) => _editing = editing;

        /// <inherit/>
        public override bool IsValid(object value)
        {
            if (value is SIGSM_MicroArea_Unidade mau)
            {
                if (_editing)

                    return DomainContainer.Current.SIGSM_MicroArea_Unidade.Any(x => x.MicroArea == mau.MicroArea &&
                        x.CodSetor == mau.CodSetor && (!_editing || x.id != mau.id));
            }

            return false;
        }
    }
}
