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
    
    public partial class AS_SetoresPar
    {
        public int NumContrato { get; set; }
        public int CodSetor { get; set; }
        public decimal Codigo { get; set; }
        public Nullable<int> CodHierarquia { get; set; }
        public Nullable<int> CodRetTrib { get; set; }
        public Nullable<int> CodTurno { get; set; }
        public Nullable<int> CodTpUnid { get; set; }
        public string CNES { get; set; }
        public Nullable<int> CodSetorVinc { get; set; }
        public Nullable<int> CodCred { get; set; }
    
        public virtual Setores Setores { get; set; }
    }
}