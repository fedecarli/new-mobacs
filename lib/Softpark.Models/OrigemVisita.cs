namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.OrigemVisita")]
    public partial class OrigemVisita
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrigemVisita()
        {
            UnicaLotacaoTransport = new HashSet<UnicaLotacaoTransport>();
        }

        [Key]
        public Guid token { get; set; }

        public bool finalizado { get; set; }

        public bool enviado { get; set; }

        public bool enviarParaThrift { get; set; }

        public int id_tipo_origem { get; set; }

        public virtual TipoOrigem TipoOrigem { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnicaLotacaoTransport> UnicaLotacaoTransport { get; set; }
    }
}
