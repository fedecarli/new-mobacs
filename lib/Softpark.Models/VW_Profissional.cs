using System.ComponentModel.DataAnnotations;

namespace Softpark.Models
{
    public partial class VW_Profissional
    {
        [Key]
        public long id { get; set; }
        public string CNS { get; set; }
        public string Nome { get; set; }
        public string CBO { get; set; }
        public string Profissao { get; set; }
        public string CNES { get; set; }
        public string Unidade { get; set; }
        public string INE { get; set; }
        public string Equipe { get; set; }
        public int CodUsu { get; set; }
        public bool Autorizado { get; set; }
    }
}
