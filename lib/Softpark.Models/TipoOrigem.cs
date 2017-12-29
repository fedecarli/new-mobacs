namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.TipoOrigem")]
    public partial class TipoOrigem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoOrigem()
        {
            OrigemVisita = new HashSet<OrigemVisita>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Descricao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrigemVisita> OrigemVisita { get; set; }
    }
}
