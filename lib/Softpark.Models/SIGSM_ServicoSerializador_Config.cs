namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SIGSM_ServicoSerializador_Config
    {
        [Key]
        [StringLength(32)]
        public string Configuracao { get; set; }

        [StringLength(512)]
        public string Valor { get; set; }
    }
}
