namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_ConsultaCadastrosDomiciliares
    {
        [StringLength(50)]
        public string Complemento { get; set; }

        [StringLength(100)]
        public string Responsavel { get; set; }

        [StringLength(10)]
        public string Numero { get; set; }

        [StringLength(100)]
        public string Endereco { get; set; }

        [StringLength(39)]
        public string Telefone { get; set; }

        public Guid? IdFicha { get; set; }

        [Key]
        public decimal Codigo { get; set; }
    }
}
