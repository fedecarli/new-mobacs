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
    
    public partial class AS_EspecialidadesUsu
    {
        public int NumContrato { get; set; }
        public int CodEsp { get; set; }
        public int ItemE { get; set; }
        public Nullable<int> CodUsuD { get; set; }
        public Nullable<System.DateTime> DtInicio { get; set; }
        public Nullable<System.DateTime> DtFinal { get; set; }
        public Nullable<System.DateTime> DtSistema { get; set; }
        public string NumIP { get; set; }
        public Nullable<int> CodUsu { get; set; }
    
        public virtual AS_Especialidades AS_Especialidades { get; set; }
    }
}
