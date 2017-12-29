namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TP_Exames
    {
        [Key]
        public int Id_tp_exames { get; set; }

        [StringLength(150)]
        public string descricao { get; set; }

        [StringLength(50)]
        public string nome_id { get; set; }
    }
}
