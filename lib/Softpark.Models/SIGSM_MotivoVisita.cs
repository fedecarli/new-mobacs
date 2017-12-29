namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SIGSM_MotivoVisita
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SIGSM_MotivoVisita()
        {
            FichaVisitaDomiciliarChild = new HashSet<FichaVisitaDomiciliarChild>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long codigo { get; set; }

        [Required]
        [StringLength(80)]
        public string nome { get; set; }

        [Required]
        [StringLength(255)]
        public string observacoes { get; set; }

        [Required]
        [StringLength(120)]
        public string campo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FichaVisitaDomiciliarChild> FichaVisitaDomiciliarChild { get; set; }
    }
}
