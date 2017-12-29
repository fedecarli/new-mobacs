namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.AnimalNoDomicilio")]
    public partial class AnimalNoDomicilio
    {
        [Key]
        [Column(Order = 0)]
        public Guid id_cadastro_domiciliar { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_tp_animal { get; set; }

        public virtual CadastroDomiciliar CadastroDomiciliar { get; set; }
    }
}
