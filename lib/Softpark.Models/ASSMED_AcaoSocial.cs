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
    
    public partial class ASSMED_AcaoSocial
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ASSMED_AcaoSocial()
        {
            this.ASSMED_AtendAcaoSocial = new HashSet<ASSMED_AtendAcaoSocial>();
            this.ASSMED_CadastroAcaoSocial = new HashSet<ASSMED_CadastroAcaoSocial>();
        }
    
        public int NumContrato { get; set; }
        public int CodAcao { get; set; }
        public string DesAcao { get; set; }
        public Nullable<System.DateTime> DtInicio { get; set; }
        public Nullable<System.DateTime> DtFinal { get; set; }
        public string NecessitaCad { get; set; }
        public Nullable<int> CodQuest { get; set; }
        public Nullable<System.DateTime> DtSistema { get; set; }
        public Nullable<int> CodUsu { get; set; }
        public string NumIP { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_AtendAcaoSocial> ASSMED_AtendAcaoSocial { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_CadastroAcaoSocial> ASSMED_CadastroAcaoSocial { get; set; }
    }
}
