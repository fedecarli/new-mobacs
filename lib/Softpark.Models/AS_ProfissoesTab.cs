namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AS_ProfissoesTab
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumContrato { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal CodProfissao { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodTabProfissao { get; set; }

        [StringLength(10)]
        public string CodProfTab { get; set; }

        [StringLength(120)]
        public string DesProfTab { get; set; }

        public virtual AS_Profissoes AS_Profissoes { get; set; }
    }
}
