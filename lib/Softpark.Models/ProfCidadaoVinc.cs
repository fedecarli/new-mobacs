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
    
    public partial class ProfCidadaoVinc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProfCidadaoVinc()
        {
            this.ProfCidadaoVincAgendaProd = new HashSet<ProfCidadaoVincAgendaProd>();
        }
    
        public int IdVinc { get; set; }
        public Nullable<int> IdProfissional { get; set; }
        public Nullable<int> IdCidadao { get; set; }
        public Nullable<bool> Marcado { get; set; }
        public Nullable<int> OrigemPerfilVinc { get; set; }
        public Nullable<System.DateTime> DataVinc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfCidadaoVincAgendaProd> ProfCidadaoVincAgendaProd { get; set; }
    }
}
