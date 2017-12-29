namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AS_CredenciadosUsu
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumContrato { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodCred { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemU { get; set; }

        public int? CodUsuD { get; set; }

        public DateTime? DtInicio { get; set; }

        public DateTime? DtFinal { get; set; }

        public int? CodUsu { get; set; }

        public DateTime? DtSistema { get; set; }

        [StringLength(60)]
        public string NumIP { get; set; }

        public virtual AS_Credenciados AS_Credenciados { get; set; }
    }
}
