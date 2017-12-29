namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TP_Nasf
    {
        [Key]
        public int id_tp_nasf { get; set; }

        [StringLength(150)]
        public string descricao { get; set; }

        [StringLength(100)]
        public string nome_id { get; set; }
    }
}
