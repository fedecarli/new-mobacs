//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SIGSM_Procedimento_Sigtap
    {
        public long id { get; set; }
        public long id_procedimento { get; set; }
        public int NumContrato { get; set; }
        public decimal CodProc { get; set; }
        public Nullable<int> id_tipo_procedimento { get; set; }
        public Nullable<long> id_Procedimento_Usuario { get; set; }
    
        public virtual AS_Procedimentos AS_Procedimentos { get; set; }
        public virtual SIGSM_Procedimento SIGSM_Procedimento { get; set; }
        public virtual SIGSM_Tipo_Procedimento SIGSM_Tipo_Procedimento { get; set; }
    }
}
