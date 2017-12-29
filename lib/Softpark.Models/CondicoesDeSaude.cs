namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.CondicoesDeSaude")]
    public partial class CondicoesDeSaude
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CondicoesDeSaude()
        {
            CadastroIndividual = new HashSet<CadastroIndividual>();
            DoencaCardiaca = new HashSet<DoencaCardiaca>();
            DoencaRespiratoria = new HashSet<DoencaRespiratoria>();
            DoencaRins = new HashSet<DoencaRins>();
        }

        public Guid id { get; set; }

        [StringLength(100)]
        public string descricaoCausaInternacaoEm12Meses { get; set; }

        [StringLength(100)]
        public string descricaoOutraCondicao1 { get; set; }

        [StringLength(100)]
        public string descricaoOutraCondicao2 { get; set; }

        [StringLength(100)]
        public string descricaoOutraCondicao3 { get; set; }

        [StringLength(100)]
        public string descricaoPlantasMedicinaisUsadas { get; set; }

        [StringLength(100)]
        public string maternidadeDeReferencia { get; set; }

        public int? situacaoPeso { get; set; }

        public bool statusEhDependenteAlcool { get; set; }

        public bool statusEhDependenteOutrasDrogas { get; set; }

        public bool statusEhFumante { get; set; }

        public bool statusEhGestante { get; set; }

        public bool statusEstaAcamado { get; set; }

        public bool statusEstaDomiciliado { get; set; }

        public bool statusTemDiabetes { get; set; }

        public bool statusTemDoencaRespiratoria { get; set; }

        public bool statusTemHanseniase { get; set; }

        public bool statusTemHipertensaoArterial { get; set; }

        public bool statusTemTeveCancer { get; set; }

        public bool statusTemTeveDoencasRins { get; set; }

        public bool statusTemTuberculose { get; set; }

        public bool statusTeveAvcDerrame { get; set; }

        public bool statusTeveDoencaCardiaca { get; set; }

        public bool statusTeveInfarto { get; set; }

        public bool statusTeveInternadoem12Meses { get; set; }

        public bool statusUsaOutrasPraticasIntegrativasOuComplementares { get; set; }

        public bool statusUsaPlantasMedicinais { get; set; }

        public bool statusDiagnosticoMental { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CadastroIndividual> CadastroIndividual { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DoencaCardiaca> DoencaCardiaca { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DoencaRespiratoria> DoencaRespiratoria { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DoencaRins> DoencaRins { get; set; }
    }
}
