namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.ResponsavelPorCrianca")]
    public partial class ResponsavelPorCrianca
    {
        [Key]
        [Column(Order = 0)]
        public Guid id_informacoes_sociodemograficas { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_tp_crianca { get; set; }

        public virtual InformacoesSocioDemograficas InformacoesSocioDemograficas { get; set; }
    }
}
