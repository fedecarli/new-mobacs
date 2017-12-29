namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.InstituicaoPermanencia")]
    public partial class InstituicaoPermanencia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InstituicaoPermanencia()
        {
            CadastroDomiciliar = new HashSet<CadastroDomiciliar>();
        }

        public Guid id { get; set; }

        [StringLength(100)]
        public string nomeInstituicaoPermanencia { get; set; }

        public bool stOutrosProfissionaisVinculados { get; set; }

        [Required]
        [StringLength(70)]
        public string nomeResponsavelTecnico { get; set; }

        [StringLength(15)]
        public string cnsResponsavelTecnico { get; set; }

        [StringLength(100)]
        public string cargoInstituicao { get; set; }

        [StringLength(11)]
        public string telefoneResponsavelTecnico { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CadastroDomiciliar> CadastroDomiciliar { get; set; }
    }
}
