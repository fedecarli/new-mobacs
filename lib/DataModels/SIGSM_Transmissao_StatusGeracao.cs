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
    
    public partial class SIGSM_Transmissao_StatusGeracao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SIGSM_Transmissao_StatusGeracao()
        {
            this.SIGSM_Transmissao_Processos = new HashSet<SIGSM_Transmissao_Processos>();
        }
    
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Cor { get; set; }
        public string Icone { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_Transmissao_Processos> SIGSM_Transmissao_Processos { get; set; }
    }
}
