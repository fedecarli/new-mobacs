namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SIGSM_Transmissao_StatusGeracao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SIGSM_Transmissao_StatusGeracao()
        {
            SIGSM_Transmissao_Processos = new HashSet<SIGSM_Transmissao_Processos>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(60)]
        public string Descricao { get; set; }

        [StringLength(60)]
        public string Cor { get; set; }

        [StringLength(60)]
        public string Icone { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_Transmissao_Processos> SIGSM_Transmissao_Processos { get; set; }
    }
}
