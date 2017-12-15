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
    
    public partial class AS_CredenciadosVinc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AS_CredenciadosVinc()
        {
            this.SIGSM_MicroArea_CredenciadoVinc = new HashSet<SIGSM_MicroArea_CredenciadoVinc>();
        }
    
        public int NumContrato { get; set; }
        public int CodCred { get; set; }
        public int ItemVinc { get; set; }
        public Nullable<int> CodRegime { get; set; }
        public Nullable<int> CodTabProfissao { get; set; }
        public Nullable<int> CodSetor { get; set; }
        public string CNESLocal { get; set; }
        public string Matricula { get; set; }
        public string CodProfTab { get; set; }
        public Nullable<System.DateTime> DtInicio { get; set; }
        public Nullable<System.DateTime> DtFinal { get; set; }
        public Nullable<int> CodUsu { get; set; }
        public Nullable<int> CodConv { get; set; }
        public Nullable<int> CodINE { get; set; }
    
        public virtual AS_Credenciados AS_Credenciados { get; set; }
        public virtual Setores Setores { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_MicroArea_CredenciadoVinc> SIGSM_MicroArea_CredenciadoVinc { get; set; }
    }
}
