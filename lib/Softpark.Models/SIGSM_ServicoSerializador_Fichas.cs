namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SIGSM_ServicoSerializador_Fichas
    {
        [Key]
        [StringLength(32)]
        public string Ficha { get; set; }

        [Required]
        [StringLength(6)]
        public string Formato { get; set; }

        public bool Gerar { get; set; }
    }
}
