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
    
    public partial class AF_ChequesRecSit
    {
        public int NumContrato { get; set; }
        public string CodBanco { get; set; }
        public string CodAgencia { get; set; }
        public Nullable<int> AnoLan { get; set; }
        public string NumConta { get; set; }
        public string NumCheque { get; set; }
        public int Item { get; set; }
        public System.DateTime DtSit { get; set; }
        public Nullable<decimal> NumLan { get; set; }
        public Nullable<int> CodSitCheque { get; set; }
        public Nullable<int> CodUsu { get; set; }
    
        public virtual AF_ChequesRec AF_ChequesRec { get; set; }
        public virtual AF_SitCheque AF_SitCheque { get; set; }
        public virtual AF_Lancamentos AF_Lancamentos { get; set; }
    }
}
