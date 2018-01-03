using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Softpark.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class VW_Vinculos
    {
        public string CNSProfissional { get; set; }

        public string NomeProfissional { get; set; }

        public string CNES { get; set; }

        public string Unidade { get; set; }

        public decimal Codigo { get; set; }

        public string CNSCidadao { get; set; }

        public string NomeCidadao { get; set; }

        public string CodMicroArea { get; set; }

        public string MicroArea { get; set; }

        public int? Vinculo { get; set; }

        public int? CodCred { get; set; }

        public string TpLog { get; set; }

        public string Logradouro { get; set; }

        public string Numero { get; set; }

        public string CEP { get; set; }

        public string Bairro { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long row { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
