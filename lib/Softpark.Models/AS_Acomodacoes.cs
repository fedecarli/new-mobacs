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
    
    public partial class AS_Acomodacoes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AS_Acomodacoes()
        {
            this.AS_AcomodacoesCarac = new HashSet<AS_AcomodacoesCarac>();
            this.AS_Leitos = new HashSet<AS_Leitos>();
        }
    
        public int NumContrato { get; set; }
        public int CodAcomodacao { get; set; }
        public string DesAcomodacao { get; set; }
        public string Andar { get; set; }
        public string Ala { get; set; }
        public string Numero { get; set; }
        public Nullable<int> CodSetor { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AS_AcomodacoesCarac> AS_AcomodacoesCarac { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AS_Leitos> AS_Leitos { get; set; }
    }
}
