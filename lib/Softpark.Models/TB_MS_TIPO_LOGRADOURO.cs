namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_MS_TIPO_LOGRADOURO
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(3)]
        public string CO_TIPO_LOGRADOURO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string DS_TIPO_LOGRADOURO { get; set; }

        [StringLength(15)]
        public string DS_TIPO_LOGRADOURO_ABREV { get; set; }
    }
}
