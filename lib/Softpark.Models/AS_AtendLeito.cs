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
    
    public partial class AS_AtendLeito
    {
        public int NumContrato { get; set; }
        public int AnoAtend { get; set; }
        public decimal NumAtend { get; set; }
        public int ItemLeito { get; set; }
        public Nullable<int> CodLeito { get; set; }
        public Nullable<System.DateTime> DtEntrada { get; set; }
        public string HrEntrada { get; set; }
        public Nullable<System.DateTime> DtSaida { get; set; }
        public string HrSaida { get; set; }
        public Nullable<int> CodUsu { get; set; }
        public string NumIP { get; set; }
        public Nullable<System.DateTime> DtSistema { get; set; }
    
        public virtual AS_Atendimento AS_Atendimento { get; set; }
        public virtual AS_Leitos AS_Leitos { get; set; }
    }
}
