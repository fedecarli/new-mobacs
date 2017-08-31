using System.ComponentModel.DataAnnotations;

namespace Softpark.Models
{
    /// <summary>
    /// Model da view VW_Profissional
    /// </summary>
    public class VW_Profissional
    {
        /// <summary>
        /// CBO
        /// </summary>
        [Key]
        public string CBO { get; set; }
        /// <summary>
        /// CNES
        /// </summary>
        [Key]
        public string CNES { get; set; }
        /// <summary>
        /// CNS
        /// </summary>
        [Key]
        public string CNS { get; set; }
        /// <summary>
        /// INE
        /// </summary>
        public string INE { get; set; }
        /// <summary>
        /// Equipe
        /// </summary>
        public string Equipe { get; set; }
        /// <summary>
        /// Nome
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// Profissão
        /// </summary>
        public string Profissao { get; set; }
        /// <summary>
        /// Unidade
        /// </summary>
        public string Unidade { get; set; }
    }
}