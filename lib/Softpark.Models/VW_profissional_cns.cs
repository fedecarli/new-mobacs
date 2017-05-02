using System.ComponentModel.DataAnnotations;

namespace Softpark.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class VW_profissional_cns
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal Codigo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int idProfissional { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cnsProfissional { get; set; }

        public string cnsCidadao { get; set; }
        
        public int IdCidadao { get; set; }

        public string CNES { get; set; }

        public string CBO { get; set; }
        
        public string INE { get; set; }

        public bool AgendamentoMarcado { get; set; }
    }
}
