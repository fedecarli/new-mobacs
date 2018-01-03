namespace Softpark.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public partial class Etnia
    {
        public int NumContrato { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodEtnia { get; set; }

        [StringLength(150)]
        public string DesEtnia { get; set; }
    }
}
