namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SIGSM_MicroAreas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SIGSM_MicroAreas()
        {
            ASSMED_Cadastro = new HashSet<ASSMED_Cadastro>();
            ASSMED_Endereco = new HashSet<ASSMED_Endereco>();
            SIGSM_MicroArea_Unidade = new HashSet<SIGSM_MicroArea_Unidade>();
        }

        [Key]
        [StringLength(2)]
        public string Codigo { get; set; }

        [Required]
        [StringLength(80)]
        public string Descricao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_Cadastro> ASSMED_Cadastro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_Endereco> ASSMED_Endereco { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_MicroArea_Unidade> SIGSM_MicroArea_Unidade { get; set; }
    }
}
