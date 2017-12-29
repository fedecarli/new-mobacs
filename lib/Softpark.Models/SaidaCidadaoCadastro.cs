namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.SaidaCidadaoCadastro")]
    public partial class SaidaCidadaoCadastro
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SaidaCidadaoCadastro()
        {
            CadastroIndividual = new HashSet<CadastroIndividual>();
        }

        public Guid id { get; set; }

        public int? motivoSaidaCidadao { get; set; }

        [StringLength(9)]
        public string numeroDO { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dataObito { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CadastroIndividual> CadastroIndividual { get; set; }

        public virtual TP_Motivo_Saida TP_Motivo_Saida { get; set; }
    }
}
