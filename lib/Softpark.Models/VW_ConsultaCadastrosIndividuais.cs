namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_ConsultaCadastrosIndividuais
    {
        [StringLength(100)]
        public string NomeCidadao { get; set; }

        public DateTime? DataNascimento { get; set; }

        [StringLength(100)]
        public string NomeMae { get; set; }

        [StringLength(50)]
        public string CnsCidadao { get; set; }

        [StringLength(50)]
        public string MunicipioNascimento { get; set; }

        [Key]
        public decimal Codigo { get; set; }

        public Guid? IdFicha { get; set; }
    }
}
