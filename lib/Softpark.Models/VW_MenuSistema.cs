namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_MenuSistema
    {
        public int? id_pai_indireto { get; set; }

        public int? id_pai_direto { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_menu { get; set; }

        [StringLength(50)]
        public string link { get; set; }

        [StringLength(50)]
        public string sublink { get; set; }

        public int? ordem { get; set; }

        public int? id_sistema { get; set; }

        [StringLength(50)]
        public string icone { get; set; }

        [StringLength(100)]
        public string descricao { get; set; }
    }
}
