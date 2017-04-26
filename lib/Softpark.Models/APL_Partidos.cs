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
    
    public partial class APL_Partidos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public APL_Partidos()
        {
            this.APL_Candidato = new HashSet<APL_Candidato>();
            this.APL_ColigaPartidos = new HashSet<APL_ColigaPartidos>();
        }
    
        public int NumContrato { get; set; }
        public int CodPartido { get; set; }
        public string Sigla { get; set; }
        public string Nome { get; set; }
        public string Logotipo { get; set; }
        public Nullable<int> TamanhoLogo { get; set; }
        public Nullable<int> AlturaLogo { get; set; }
        public string PresidenteNacional { get; set; }
        public Nullable<System.DateTime> DtDeferimento { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APL_Candidato> APL_Candidato { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APL_ColigaPartidos> APL_ColigaPartidos { get; set; }
    }
}
