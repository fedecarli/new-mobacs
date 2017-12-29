namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.DoencaCardiaca")]
    public partial class DoencaCardiaca
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_tp_doenca_cariaca { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid id_condicao_de_saude { get; set; }

        public virtual CondicoesDeSaude CondicoesDeSaude { get; set; }
    }
}
