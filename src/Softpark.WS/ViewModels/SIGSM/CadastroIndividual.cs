using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Softpark.WS.ViewModels.SIGSM
{
    public class CadastroIndividualVM
    {
        public int NumContrato { get; set; }
        public decimal Codigo { get; set; }
        public Guid? IdFicha { get; set; }
        public string NomeCidadao { get; set; }
        public string CnsCidadao { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string CPF { get; set; }
    }
}