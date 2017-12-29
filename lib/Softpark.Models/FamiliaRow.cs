namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.FamiliaRow")]
    public partial class FamiliaRow
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FamiliaRow()
        {
            CadastroDomiciliar = new HashSet<CadastroDomiciliar>();
        }

        public Guid id { get; set; }

        [Required]
        [StringLength(15)]
        public string numeroCnsResponsavel { get; set; }

        public int? numeroMembrosFamilia { get; set; }

        [StringLength(30)]
        public string numeroProntuario { get; set; }

        public int? rendaFamiliar { get; set; }

        public bool stMudanca { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dataNascimentoResponsavel { get; set; }

        [Column(TypeName = "date")]
        public DateTime? resideDesde { get; set; }

        public virtual TP_Renda_Familiar TP_Renda_Familiar { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CadastroDomiciliar> CadastroDomiciliar { get; set; }
    }
}
