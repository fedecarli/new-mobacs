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
    
    public partial class AF_ReceberMatricula
    {
        public int NumContrato { get; set; }
        public decimal Codigo { get; set; }
        public int ItemMat { get; set; }
        public int AnoRec { get; set; }
        public decimal NumRec { get; set; }
        public Nullable<System.DateTime> DtSistema { get; set; }
        public string NumIP { get; set; }
        public Nullable<int> CodUsu { get; set; }
    
        public virtual AF_Receber AF_Receber { get; set; }
        public virtual ASSMED_TurmaMatricula ASSMED_TurmaMatricula { get; set; }
    }
}
