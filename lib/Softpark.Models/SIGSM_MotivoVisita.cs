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
    
    public partial class SIGSM_MotivoVisita
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SIGSM_MotivoVisita()
        {
            this.FichaVisitaDomiciliarChild = new HashSet<FichaVisitaDomiciliarChild>();
        }
    
        public long codigo { get; set; }
        public string nome { get; set; }
        public string observacoes { get; set; }
        public string campo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FichaVisitaDomiciliarChild> FichaVisitaDomiciliarChild { get; set; }
    }
}
