namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_Cadastros_Zoneamento
    {
        [Key]
        public decimal Codigo { get; set; }

        public int? ItemEnd { get; set; }

        public Guid? IdIdentificacaoUsuarioCidadao { get; set; }

        public Guid? IdEnderecoLocalPermanencia { get; set; }

        [StringLength(2)]
        public string MicroArea { get; set; }
    }
}
