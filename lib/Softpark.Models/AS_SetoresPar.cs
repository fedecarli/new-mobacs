namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AS_SetoresPar
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumContrato { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodSetor { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal Codigo { get; set; }

        public int? CodHierarquia { get; set; }

        public int? CodRetTrib { get; set; }

        public int? CodTurno { get; set; }

        public int? CodTpUnid { get; set; }

        [StringLength(7)]
        public string CNES { get; set; }

        public int? CodSetorVinc { get; set; }

        public int? CodCred { get; set; }

        public virtual Setores Setores { get; set; }
    }
}
