namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ASSMED_Acesso
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(80)]
        public string EMail { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime DtAcesso { get; set; }

        public int? CodUsu { get; set; }

        [StringLength(1)]
        public string Validou { get; set; }

        [StringLength(60)]
        public string NumIP { get; set; }

        public DateTime? DtSaida { get; set; }

        [StringLength(40)]
        public string ASPSESSIONIDQASRTRQT { get; set; }

        public DateTime? DtUltVer { get; set; }
    }
}
