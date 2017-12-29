namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.EmSituacaoDeRua")]
    public partial class EmSituacaoDeRua
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmSituacaoDeRua()
        {
            CadastroIndividual = new HashSet<CadastroIndividual>();
            HigienePessoalSituacaoRua = new HashSet<HigienePessoalSituacaoRua>();
            OrigemAlimentoSituacaoRua = new HashSet<OrigemAlimentoSituacaoRua>();
        }

        public Guid id { get; set; }

        [StringLength(100)]
        public string grauParentescoFamiliarFrequentado { get; set; }

        [StringLength(100)]
        public string outraInstituicaoQueAcompanha { get; set; }

        public int? quantidadeAlimentacoesAoDiaSituacaoRua { get; set; }

        public bool statusAcompanhadoPorOutraInstituicao { get; set; }

        public bool statusPossuiReferenciaFamiliar { get; set; }

        public bool statusRecebeBeneficio { get; set; }

        public bool statusSituacaoRua { get; set; }

        public bool statusTemAcessoHigienePessoalSituacaoRua { get; set; }

        public bool statusVisitaFamiliarFrequentemente { get; set; }

        public int? tempoSituacaoRua { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CadastroIndividual> CadastroIndividual { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HigienePessoalSituacaoRua> HigienePessoalSituacaoRua { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrigemAlimentoSituacaoRua> OrigemAlimentoSituacaoRua { get; set; }
    }
}
