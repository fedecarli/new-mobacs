namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cidade")]
    public partial class Cidade
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodCidade { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumContrato { get; set; }

        [StringLength(50)]
        public string NomeCidade { get; set; }

        [StringLength(2)]
        public string UF { get; set; }

        [StringLength(7)]
        public string CodIbge { get; set; }

        public int? CodDNE { get; set; }

        public double? Lei { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DtCriacao { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DtInstala { get; set; }

        [StringLength(20)]
        public string Latitude { get; set; }

        [StringLength(20)]
        public string Longitude { get; set; }

        public decimal? Area { get; set; }

        public decimal? Altitude { get; set; }

        [StringLength(1)]
        public string Capital { get; set; }

        [StringLength(1)]
        public string Fonteira { get; set; }

        [StringLength(1)]
        public string Fronteira { get; set; }

        public int? DDD { get; set; }
    }
}
