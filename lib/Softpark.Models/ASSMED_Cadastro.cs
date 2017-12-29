namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ASSMED_Cadastro
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ASSMED_Cadastro()
        {
            IdentificacaoUsuarioCidadao1 = new HashSet<IdentificacaoUsuarioCidadao>();
            ASSMED_CadastroDocPessoal = new HashSet<ASSMED_CadastroDocPessoal>();
            ASSMED_CadEmails = new HashSet<ASSMED_CadEmails>();
            ASSMED_CadTelefones = new HashSet<ASSMED_CadTelefones>();
            ASSMED_Endereco = new HashSet<ASSMED_Endereco>();
            SIGSM_MicroArea_CredenciadoCidadao = new HashSet<SIGSM_MicroArea_CredenciadoCidadao>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumContrato { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal Codigo { get; set; }

        [StringLength(1)]
        public string Tipo { get; set; }

        [StringLength(100)]
        public string Nome { get; set; }

        public DateTime? DtSistema { get; set; }

        public int? CodUsu { get; set; }

        [StringLength(60)]
        public string NumIP { get; set; }

        [StringLength(100)]
        public string NomeSocial { get; set; }

        public Guid? IdFicha { get; set; }

        public int? CodMunicipe { get; set; }

        public DateTime? DtAtualizacao { get; set; }

        public int? CodTpHomologacao { get; set; }

        [StringLength(500)]
        public string Justificativa { get; set; }

        [StringLength(500)]
        public string MotivoHomologacao { get; set; }

        [StringLength(2)]
        public string MicroArea { get; set; }

        public virtual IdentificacaoUsuarioCidadao IdentificacaoUsuarioCidadao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IdentificacaoUsuarioCidadao> IdentificacaoUsuarioCidadao1 { get; set; }

        public virtual SIGSM_MicroAreas SIGSM_MicroAreas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_CadastroDocPessoal> ASSMED_CadastroDocPessoal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_CadEmails> ASSMED_CadEmails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_CadTelefones> ASSMED_CadTelefones { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_Endereco> ASSMED_Endereco { get; set; }

        public virtual ASSMED_PesFisica ASSMED_PesFisica { get; set; }

        public virtual ASSMED_CadastroPF ASSMED_CadastroPF { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_MicroArea_CredenciadoCidadao> SIGSM_MicroArea_CredenciadoCidadao { get; set; }
    }
}
