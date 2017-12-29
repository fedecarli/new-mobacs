namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.EnderecoLocalPermanencia")]
    public partial class EnderecoLocalPermanencia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EnderecoLocalPermanencia()
        {
            CadastroDomiciliar = new HashSet<CadastroDomiciliar>();
            ASSMED_Endereco = new HashSet<ASSMED_Endereco>();
        }

        public Guid id { get; set; }

        [Required]
        [StringLength(72)]
        public string bairro { get; set; }

        [Required]
        [StringLength(8)]
        public string cep { get; set; }

        [Required]
        [StringLength(7)]
        public string codigoIbgeMunicipio { get; set; }

        [StringLength(30)]
        public string complemento { get; set; }

        [Required]
        [StringLength(72)]
        public string nomeLogradouro { get; set; }

        [StringLength(10)]
        public string numero { get; set; }

        [Required]
        [StringLength(2)]
        public string numeroDneUf { get; set; }

        [StringLength(11)]
        public string telefoneContato { get; set; }

        [StringLength(11)]
        public string telefoneResidencia { get; set; }

        [Required]
        [StringLength(3)]
        public string tipoLogradouroNumeroDne { get; set; }

        public bool stSemNumero { get; set; }

        [StringLength(40)]
        public string pontoReferencia { get; set; }

        [StringLength(2)]
        public string microarea { get; set; }

        public bool stForaArea { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CadastroDomiciliar> CadastroDomiciliar { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_Endereco> ASSMED_Endereco { get; set; }
    }
}
