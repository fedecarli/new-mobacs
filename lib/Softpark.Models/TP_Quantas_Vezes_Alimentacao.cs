namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TP_Quantas_Vezes_Alimentacao
    {
        [Key]
        public int id_tp_quantas_vezes_alimentacao { get; set; }

        public int codigo { get; set; }

        [Required]
        [StringLength(60)]
        public string descricao { get; set; }

        [StringLength(512)]
        public string observacao { get; set; }
    }
}
