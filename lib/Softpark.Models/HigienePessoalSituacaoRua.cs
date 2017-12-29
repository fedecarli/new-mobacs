namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.HigienePessoalSituacaoRua")]
    public partial class HigienePessoalSituacaoRua
    {
        [Key]
        [Column(Order = 0)]
        public Guid id_em_situacao_de_rua { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int codigo_higiene_pessoal { get; set; }

        public virtual EmSituacaoDeRua EmSituacaoDeRua { get; set; }
    }
}
