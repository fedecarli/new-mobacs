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
    
    public partial class ASSMED_Vacinas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ASSMED_Vacinas()
        {
            this.ASSMED_CadastroVacina = new HashSet<ASSMED_CadastroVacina>();
            this.ASSMED_VacinaConfRel = new HashSet<ASSMED_VacinaConfRel>();
            this.ASSMED_VacinasCombate = new HashSet<ASSMED_VacinasCombate>();
            this.ASSMED_VacinaSolicita = new HashSet<ASSMED_VacinaSolicita>();
            this.ASSMED_VacinasValor = new HashSet<ASSMED_VacinasValor>();
        }
    
        public int NumContrato { get; set; }
        public int CodVacina { get; set; }
        public string DesVacina { get; set; }
        public string Sexo { get; set; }
        public Nullable<int> IdadeI { get; set; }
        public Nullable<int> IdadeF { get; set; }
        public string TipoIdade { get; set; }
        public Nullable<int> NumDoses { get; set; }
        public Nullable<int> ACada { get; set; }
        public string TipoDose { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_CadastroVacina> ASSMED_CadastroVacina { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_VacinaConfRel> ASSMED_VacinaConfRel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_VacinasCombate> ASSMED_VacinasCombate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_VacinaSolicita> ASSMED_VacinaSolicita { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_VacinasValor> ASSMED_VacinasValor { get; set; }
    }
}
