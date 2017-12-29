namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TP_Situacao_Moradia
    {
        [Key]
        public int id_tp_situacao_moradia { get; set; }

        [StringLength(150)]
        public string descricao { get; set; }

        public int? id_user_cadastro { get; set; }

        public int? ativo { get; set; }

        public DateTime? data_cadastro { get; set; }

        public int? id_user_alteracao { get; set; }

        public DateTime? data_alteracao { get; set; }

        public int codigo { get; set; }
    }
}
