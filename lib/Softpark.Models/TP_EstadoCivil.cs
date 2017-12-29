namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TP_EstadoCivil
    {
        [Key]
        [StringLength(1)]
        public string codigo { get; set; }

        [Required]
        [StringLength(50)]
        public string descricao { get; set; }
    }
}
