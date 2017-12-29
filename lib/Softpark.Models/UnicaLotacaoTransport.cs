namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.UnicaLotacaoTransport")]
    public partial class UnicaLotacaoTransport
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UnicaLotacaoTransport()
        {
            CadastroDomiciliar = new HashSet<CadastroDomiciliar>();
            CadastroIndividual = new HashSet<CadastroIndividual>();
            FichaVisitaDomiciliarMaster = new HashSet<FichaVisitaDomiciliarMaster>();
        }

        public Guid id { get; set; }

        [Required]
        [StringLength(15)]
        public string profissionalCNS { get; set; }

        [Required]
        [StringLength(6)]
        public string cboCodigo_2002 { get; set; }

        [Required]
        [StringLength(7)]
        public string cnes { get; set; }

        [StringLength(10)]
        public string ine { get; set; }

        [Required]
        [StringLength(7)]
        public string codigoIbgeMunicipio { get; set; }

        public Guid token { get; set; }

        public DateTime dataAtendimento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CadastroDomiciliar> CadastroDomiciliar { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CadastroIndividual> CadastroIndividual { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FichaVisitaDomiciliarMaster> FichaVisitaDomiciliarMaster { get; set; }

        public virtual OrigemVisita OrigemVisita { get; set; }
    }
}
