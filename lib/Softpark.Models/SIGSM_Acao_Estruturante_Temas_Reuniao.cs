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
    
    public partial class SIGSM_Acao_Estruturante_Temas_Reuniao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SIGSM_Acao_Estruturante_Temas_Reuniao()
        {
            this.SIGSM_Atividade_Coletiva_Temas_Reuniao = new HashSet<SIGSM_Atividade_Coletiva_Temas_Reuniao>();
        }
    
        public int id { get; set; }
        public string nome { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_Atividade_Coletiva_Temas_Reuniao> SIGSM_Atividade_Coletiva_Temas_Reuniao { get; set; }
    }
}
