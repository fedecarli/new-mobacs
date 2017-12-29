namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TP_Renda_Familiar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TP_Renda_Familiar()
        {
            FamiliaRow = new HashSet<FamiliaRow>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int codigo { get; set; }

        [Required]
        [StringLength(60)]
        public string descricao { get; set; }

        [StringLength(512)]
        public string observacoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FamiliaRow> FamiliaRow { get; set; }
    }
}
