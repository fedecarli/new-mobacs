namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ASSMED_CadTelefones
    {
        public int NumContrato { get; set; }

        public decimal Codigo { get; set; }

        public int DDD { get; set; }

        [Required]
        [StringLength(9)]
        public string NumTel { get; set; }

        [StringLength(1)]
        public string TipoTel { get; set; }

        public DateTime? DtSistema { get; set; }

        [StringLength(60)]
        public string NumIP { get; set; }

        public int? CodUsu { get; set; }

        [StringLength(100)]
        public string Observacoes { get; set; }

        [Key]
        public long IDTelefone { get; set; }

        public virtual ASSMED_Cadastro ASSMED_Cadastro { get; set; }
    }
}
