namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.OrigemAlimentoSituacaoRua")]
    public partial class OrigemAlimentoSituacaoRua
    {
        [Key]
        [Column(Order = 0)]
        public Guid id_em_situacao_rua { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_tp_origem_alimento { get; set; }

        public virtual EmSituacaoDeRua EmSituacaoDeRua { get; set; }
    }
}
