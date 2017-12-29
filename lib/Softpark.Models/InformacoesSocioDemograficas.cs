namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.InformacoesSocioDemograficas")]
    public partial class InformacoesSocioDemograficas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InformacoesSocioDemograficas()
        {
            CadastroIndividual = new HashSet<CadastroIndividual>();
            DeficienciasCidadao = new HashSet<DeficienciasCidadao>();
            ResponsavelPorCrianca = new HashSet<ResponsavelPorCrianca>();
        }

        public Guid id { get; set; }

        public int? grauInstrucaoCidadao { get; set; }

        [StringLength(10)]
        public string ocupacaoCodigoCbo2002 { get; set; }

        public int? orientacaoSexualCidadao { get; set; }

        [StringLength(100)]
        public string povoComunidadeTradicional { get; set; }

        public int? relacaoParentescoCidadao { get; set; }

        public int? situacaoMercadoTrabalhoCidadao { get; set; }

        public bool statusDesejaInformarOrientacaoSexual { get; set; }

        public bool statusFrequentaBenzedeira { get; set; }

        public bool statusFrequentaEscola { get; set; }

        public bool statusMembroPovoComunidadeTradicional { get; set; }

        public bool statusParticipaGrupoComunitario { get; set; }

        public bool statusPossuiPlanoSaudePrivado { get; set; }

        public bool statusTemAlgumaDeficiencia { get; set; }

        public int? identidadeGeneroCidadao { get; set; }

        public bool statusDesejaInformarIdentidadeGenero { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CadastroIndividual> CadastroIndividual { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeficienciasCidadao> DeficienciasCidadao { get; set; }

        public virtual TP_Identidade_Genero_Cidadao TP_Identidade_Genero_Cidadao { get; set; }

        public virtual TP_Orientacao_Sexual TP_Orientacao_Sexual { get; set; }

        public virtual TP_Relacao_Parentesco TP_Relacao_Parentesco { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResponsavelPorCrianca> ResponsavelPorCrianca { get; set; }
    }
}
