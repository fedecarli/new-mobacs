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
    
    public partial class ASSMED_Suporte
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ASSMED_Suporte()
        {
            this.ASSMED_SuporteAnda = new HashSet<ASSMED_SuporteAnda>();
        }
    
        public int AnoSuporte { get; set; }
        public decimal NumSuporte { get; set; }
        public string EMail { get; set; }
        public Nullable<System.DateTime> DtHora { get; set; }
        public string EMailRemete { get; set; }
        public string LocalRemete { get; set; }
        public string DescSuporte { get; set; }
        public string NumIP { get; set; }
        public int NumContrato { get; set; }
        public string ArqAnexo { get; set; }
        public string Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_SuporteAnda> ASSMED_SuporteAnda { get; set; }
    }
}
