namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SetoresINEs
    {
        [Key]
        public int CodINE { get; set; }

        public int? NumContrato { get; set; }

        public int? CodSetor { get; set; }

        [StringLength(10)]
        public string Numero { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }

        public virtual Setores Setores { get; set; }
    }
}
