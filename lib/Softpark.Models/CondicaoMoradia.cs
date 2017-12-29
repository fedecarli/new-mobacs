namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.CondicaoMoradia")]
    public partial class CondicaoMoradia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CondicaoMoradia()
        {
            CadastroDomiciliar = new HashSet<CadastroDomiciliar>();
        }

        public Guid id { get; set; }

        public int? abastecimentoAgua { get; set; }

        public int? areaProducaoRural { get; set; }

        public int? destinoLixo { get; set; }

        public int? formaEscoamentoBanheiro { get; set; }

        public int? localizacao { get; set; }

        public int? materialPredominanteParedesExtDomicilio { get; set; }

        public int? nuComodos { get; set; }

        public int? nuMoradores { get; set; }

        public int? situacaoMoradiaPosseTerra { get; set; }

        public bool stDisponibilidadeEnergiaEletrica { get; set; }

        public int? tipoAcessoDomicilio { get; set; }

        public int? tipoDomicilio { get; set; }

        public int? aguaConsumoDomicilio { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CadastroDomiciliar> CadastroDomiciliar { get; set; }

        public virtual TP_Cond_Posse_Uso_Terra TP_Cond_Posse_Uso_Terra { get; set; }
    }
}
