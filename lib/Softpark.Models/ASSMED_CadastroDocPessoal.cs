namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ASSMED_CadastroDocPessoal
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodTpDocP { get; set; }

        public int? CodOrgao { get; set; }

        [StringLength(50)]
        public string Numero { get; set; }

        [StringLength(5)]
        public string Serie { get; set; }

        [StringLength(5)]
        public string Secao { get; set; }

        [StringLength(5)]
        public string Zona { get; set; }

        public DateTime? DtEmissao { get; set; }

        public DateTime? DtValidade { get; set; }

        [StringLength(2)]
        public string UFOrgao { get; set; }

        public int? CodUsu { get; set; }

        public DateTime? DtSistema { get; set; }

        [StringLength(60)]
        public string NumIP { get; set; }

        [StringLength(4)]
        public string ComplementoRG { get; set; }

        public virtual ASSMED_Cadastro ASSMED_Cadastro { get; set; }

        public virtual ASSMED_TipoDocPessoal ASSMED_TipoDocPessoal { get; set; }
    }
}
