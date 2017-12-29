namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ASSMED_CadEmails
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumContrato { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal Codigo { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string EMail { get; set; }

        [StringLength(1)]
        public string TipoEMail { get; set; }

        public DateTime? DtSistema { get; set; }

        [StringLength(60)]
        public string NumIP { get; set; }

        public int? CodUsu { get; set; }

        public virtual ASSMED_Cadastro ASSMED_Cadastro { get; set; }
    }
}
