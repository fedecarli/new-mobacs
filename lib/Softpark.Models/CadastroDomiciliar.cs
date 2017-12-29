namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.CadastroDomiciliar")]
    public partial class CadastroDomiciliar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CadastroDomiciliar()
        {
            AnimalNoDomicilio = new HashSet<AnimalNoDomicilio>();
            FamiliaRow = new HashSet<FamiliaRow>();
        }

        public Guid id { get; set; }

        public Guid? condicaoMoradia { get; set; }

        public Guid? enderecoLocalPermanencia { get; set; }

        public bool fichaAtualizada { get; set; }

        public int? quantosAnimaisNoDomicilio { get; set; }

        public bool stAnimaisNoDomicilio { get; set; }

        public bool statusTermoRecusa { get; set; }

        public int tpCdsOrigem { get; set; }

        public Guid? uuidFichaOriginadora { get; set; }

        public int tipoDeImovel { get; set; }

        public Guid? instituicaoPermanencia { get; set; }

        public Guid headerTransport { get; set; }

        [StringLength(20)]
        public string latitude { get; set; }

        [StringLength(20)]
        public string longitude { get; set; }

        public DateTime? DataRegistro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnimalNoDomicilio> AnimalNoDomicilio { get; set; }

        public virtual CondicaoMoradia CondicaoMoradia1 { get; set; }

        public virtual EnderecoLocalPermanencia EnderecoLocalPermanencia1 { get; set; }

        public virtual InstituicaoPermanencia InstituicaoPermanencia1 { get; set; }

        public virtual TP_Imovel TP_Imovel { get; set; }

        public virtual UnicaLotacaoTransport UnicaLotacaoTransport { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FamiliaRow> FamiliaRow { get; set; }
    }
}
