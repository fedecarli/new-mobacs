namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.IdentificacaoUsuarioCidadao")]
    public partial class IdentificacaoUsuarioCidadao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IdentificacaoUsuarioCidadao()
        {
            CadastroIndividual = new HashSet<CadastroIndividual>();
            Documentos = new HashSet<Documentos>();
            ASSMED_Cadastro = new HashSet<ASSMED_Cadastro>();
        }

        public Guid id { get; set; }

        [StringLength(70)]
        public string nomeSocial { get; set; }

        [StringLength(7)]
        public string codigoIbgeMunicipioNascimento { get; set; }

        public bool desconheceNomeMae { get; set; }

        [StringLength(100)]
        public string emailCidadao { get; set; }

        public int nacionalidadeCidadao { get; set; }

        [Required]
        [StringLength(70)]
        public string nomeCidadao { get; set; }

        [StringLength(70)]
        public string nomeMaeCidadao { get; set; }

        [StringLength(15)]
        public string cnsCidadao { get; set; }

        [StringLength(15)]
        public string cnsResponsavelFamiliar { get; set; }

        [StringLength(11)]
        public string telefoneCelular { get; set; }

        [StringLength(11)]
        public string numeroNisPisPasep { get; set; }

        public int? paisNascimento { get; set; }

        public int racaCorCidadao { get; set; }

        public int sexoCidadao { get; set; }

        public bool statusEhResponsavel { get; set; }

        public int? etnia { get; set; }

        public int? num_contrato { get; set; }

        [StringLength(70)]
        public string nomePaiCidadao { get; set; }

        public bool desconheceNomePai { get; set; }

        [StringLength(16)]
        public string portariaNaturalizacao { get; set; }

        [StringLength(2)]
        public string microarea { get; set; }

        public bool stForaArea { get; set; }

        [Column(TypeName = "date")]
        public DateTime dataNascimentoCidadao { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtNaturalizacao { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtEntradaBrasil { get; set; }

        [StringLength(4)]
        public string ComplementoRG { get; set; }

        [StringLength(20)]
        public string RG { get; set; }

        [StringLength(11)]
        public string CPF { get; set; }

        public bool? beneficiarioBolsaFamilia { get; set; }

        [StringLength(1)]
        public string EstadoCivil { get; set; }

        public decimal? Codigo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CadastroIndividual> CadastroIndividual { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documentos> Documentos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_Cadastro> ASSMED_Cadastro { get; set; }

        public virtual ASSMED_Cadastro ASSMED_Cadastro1 { get; set; }

        public virtual TP_Raca_Cor TP_Raca_Cor { get; set; }

        public virtual TP_Sexo TP_Sexo { get; set; }
    }
}
