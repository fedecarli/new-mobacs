using System.ComponentModel.DataAnnotations;

namespace Softpark.Models
{
    public class VW_Profissional
    {
        [Key]
        public string CBO { get; set; }
        [Key]
        public string CNES { get; set; }
        [Key]
        public string CNS { get; set; }
        public string INE { get; set; }
        public string Equipe { get; set; }
        public string Nome { get; set; }
        public string Profissao { get; set; }
        public string Unidade { get; set; }
        public int CodUsu { get; set; }
    }
}