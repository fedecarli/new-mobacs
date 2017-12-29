namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UF")]
    public partial class UF
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UF()
        {
            AS_Credenciados = new HashSet<AS_Credenciados>();
        }

        [Key]
        [Column("UF")]
        [StringLength(2)]
        public string UF1 { get; set; }

        [StringLength(50)]
        public string DesUF { get; set; }

        public int? CodANP { get; set; }

        [StringLength(2)]
        public string DNE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AS_Credenciados> AS_Credenciados { get; set; }
    }
}
