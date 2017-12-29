namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TP_Raca_Cor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TP_Raca_Cor()
        {
            IdentificacaoUsuarioCidadao = new HashSet<IdentificacaoUsuarioCidadao>();
        }

        [Key]
        public int id_tp_raca_cor { get; set; }

        [StringLength(150)]
        public string descricao { get; set; }

        public int? id_user_cadastro { get; set; }

        public int? ativo { get; set; }

        public DateTime? data_cadastro { get; set; }

        public int? id_user_alteracao { get; set; }

        public DateTime? data_alteracao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IdentificacaoUsuarioCidadao> IdentificacaoUsuarioCidadao { get; set; }
    }
}
