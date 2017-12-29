namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TP_Condicao_Avaliada
    {
        [Key]
        public int Id_tp_condicao_avaliada { get; set; }

        [StringLength(150)]
        public string descricao { get; set; }

        [StringLength(1)]
        public string tp_sub_grupos { get; set; }

        [StringLength(100)]
        public string nome_id { get; set; }

        [StringLength(12)]
        public string codigo { get; set; }
    }
}
