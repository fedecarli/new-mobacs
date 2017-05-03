using System;

namespace Softpark.Models
{
    public class VW_ultimo_cadastroIndividual
    {
        public decimal Codigo { get; set; }

        public Guid idCadastroIndividual { get; set; }

        public Guid headerTransport { get; set; }

        public Guid token { get; set; }
    }
}
