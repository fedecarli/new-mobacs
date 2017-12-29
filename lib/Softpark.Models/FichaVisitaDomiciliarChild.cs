namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.FichaVisitaDomiciliarChild")]
    public partial class FichaVisitaDomiciliarChild
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FichaVisitaDomiciliarChild()
        {
            SIGSM_MotivoVisita = new HashSet<SIGSM_MotivoVisita>();
        }

        [Required]
        [StringLength(44)]
        public string uuidFicha { get; set; }

        public long turno { get; set; }

        [StringLength(30)]
        public string numProntuario { get; set; }

        [StringLength(15)]
        public string cnsCidadao { get; set; }

        public long? sexo { get; set; }

        public bool statusVisitaCompartilhadaOutroProfissional { get; set; }

        public long desfecho { get; set; }

        [StringLength(2)]
        public string microarea { get; set; }

        public bool stForaArea { get; set; }

        public long tipoDeImovel { get; set; }

        public decimal? pesoAcompanhamentoNutricional { get; set; }

        public decimal? alturaAcompanhamentoNutricional { get; set; }

        [StringLength(20)]
        public string latitude { get; set; }

        [StringLength(20)]
        public string longitude { get; set; }

        [Key]
        public Guid childId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtNascimento { get; set; }

        public DateTime? DataRegistro { get; set; }

        [StringLength(80)]
        public string nomeCidadao { get; set; }

        public decimal? Codigo { get; set; }

        public virtual FichaVisitaDomiciliarMaster FichaVisitaDomiciliarMaster { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_MotivoVisita> SIGSM_MotivoVisita { get; set; }
    }
}
