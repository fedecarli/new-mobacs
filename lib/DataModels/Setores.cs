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
    
    public partial class Setores
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Setores()
        {
            this.AS_CredenciadosVinc = new HashSet<AS_CredenciadosVinc>();
            this.AS_SetoresPar = new HashSet<AS_SetoresPar>();
            this.SetoresINEs = new HashSet<SetoresINEs>();
        }
    
        public int CodSetor { get; set; }
        public int NumContrato { get; set; }
        public string DesSetor { get; set; }
        public Nullable<decimal> Codigo { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool almoxarifado { get; set; }
        public string DesSetorRes { get; set; }
        public Nullable<int> blindado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AS_CredenciadosVinc> AS_CredenciadosVinc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AS_SetoresPar> AS_SetoresPar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SetoresINEs> SetoresINEs { get; set; }
    }
}
