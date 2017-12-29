namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.RastroFicha")]
    public partial class RastroFicha
    {
        [Key]
        [Column(Order = 0)]
        public Guid token { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodUsu { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime DataModificacao { get; set; }

        [Column(TypeName = "ntext")]
        public string DadoAnterior { get; set; }

        [Column(TypeName = "ntext")]
        public string DadoAtual { get; set; }
    }
}
