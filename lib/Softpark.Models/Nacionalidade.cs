namespace Softpark.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Nacionalidade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodNacao { get; set; }
        [StringLength(50)]
        public string DesNacao { get; set; }
        public Nullable<int> codigo { get; set; }
    }
}
