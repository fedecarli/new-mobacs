namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.FichaVisitaDomiciliarMaster")]
    public partial class FichaVisitaDomiciliarMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FichaVisitaDomiciliarMaster()
        {
            FichaVisitaDomiciliarChild = new HashSet<FichaVisitaDomiciliarChild>();
        }

        [Key]
        [StringLength(44)]
        public string uuidFicha { get; set; }

        public int tpCdsOrigem { get; set; }

        public Guid headerTransport { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FichaVisitaDomiciliarChild> FichaVisitaDomiciliarChild { get; set; }

        public virtual UnicaLotacaoTransport UnicaLotacaoTransport { get; set; }
    }
}
