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
    
    public partial class SIGSM_MicroArea_CredenciadoVinc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SIGSM_MicroArea_CredenciadoVinc()
        {
            this.SIGSM_Check_Cadastros = new HashSet<SIGSM_Check_Cadastros>();
        }
    
        public int id { get; set; }
        public int idMicroAreaUnidade { get; set; }
        public int NumContrato { get; set; }
        public int CodCred { get; set; }
        public int ItemVinc { get; set; }
    
        public virtual AS_Credenciados AS_Credenciados { get; set; }
        public virtual AS_CredenciadosVinc AS_CredenciadosVinc { get; set; }
        public virtual ASSMED_Contratos ASSMED_Contratos { get; set; }
        public virtual SIGSM_MicroArea_Unidade SIGSM_MicroArea_Unidade { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_Check_Cadastros> SIGSM_Check_Cadastros { get; set; }
    }
}
