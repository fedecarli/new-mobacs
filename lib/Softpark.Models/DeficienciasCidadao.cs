namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.DeficienciasCidadao")]
    public partial class DeficienciasCidadao
    {
        [Key]
        [Column(Order = 0)]
        public Guid id_informacoes_socio_demograficas { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_tp_deficiencia_cidadao { get; set; }

        public virtual InformacoesSocioDemograficas InformacoesSocioDemograficas { get; set; }
    }
}
