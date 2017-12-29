namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ASSMED_Endereco
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumContrato { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal Codigo { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemEnd { get; set; }

        [StringLength(10)]
        public string CEP { get; set; }

        [StringLength(1)]
        public string TipoEnd { get; set; }

        [StringLength(1)]
        public string Corresp { get; set; }

        [StringLength(100)]
        public string Logradouro { get; set; }

        [StringLength(50)]
        public string Bairro { get; set; }

        [StringLength(50)]
        public string Complemento { get; set; }

        [StringLength(80)]
        public string NomeCidade { get; set; }

        [StringLength(2)]
        public string UF { get; set; }

        [StringLength(10)]
        public string Numero { get; set; }

        [StringLength(20)]
        public string Latitude { get; set; }

        [StringLength(20)]
        public string Longitude { get; set; }

        public int? CodTpLogra { get; set; }

        public int? CodCidade { get; set; }

        public Guid? IdFicha { get; set; }

        [StringLength(2)]
        public string MicroArea { get; set; }

        public virtual EnderecoLocalPermanencia EnderecoLocalPermanencia { get; set; }

        public virtual ASSMED_Cadastro ASSMED_Cadastro { get; set; }

        public virtual SIGSM_MicroAreas SIGSM_MicroAreas { get; set; }
    }
}
