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
    
    public partial class AF_ChequesRec
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AF_ChequesRec()
        {
            this.AF_ChequesRecSit = new HashSet<AF_ChequesRecSit>();
            this.AF_RecebeCheque = new HashSet<AF_RecebeCheque>();
        }
    
        public int NumContrato { get; set; }
        public string CodBanco { get; set; }
        public Nullable<decimal> Codigo { get; set; }
        public Nullable<int> CodConta { get; set; }
        public string CodAgencia { get; set; }
        public string NumConta { get; set; }
        public Nullable<decimal> Valor { get; set; }
        public string NumCheque { get; set; }
        public string CodMoeda { get; set; }
        public Nullable<System.DateTime> DtCheque { get; set; }
        public Nullable<int> Codusu { get; set; }
    
        public virtual AF_Bancos AF_Bancos { get; set; }
        public virtual AF_Contas AF_Contas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AF_ChequesRecSit> AF_ChequesRecSit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AF_RecebeCheque> AF_RecebeCheque { get; set; }
    }
}
