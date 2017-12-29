namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ASSMED_PesFisica
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumContrato { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal Codigo { get; set; }

        [StringLength(14)]
        public string CPF { get; set; }

        public DateTime? DtNasc { get; set; }

        [StringLength(1)]
        public string Sexo { get; set; }

        [StringLength(1)]
        public string DtObto { get; set; }

        [StringLength(1)]
        public string EstCivil { get; set; }

        [StringLength(100)]
        public string NomePai { get; set; }

        [StringLength(100)]
        public string NomeMae { get; set; }

        public int? CodSitFam { get; set; }

        public int? CodEscola { get; set; }

        public int? CodCor { get; set; }

        public int? CodNacao { get; set; }

        public int? CodEtnia { get; set; }

        [StringLength(1)]
        public string Deficiente { get; set; }

        public int? CodDeficiencia { get; set; }

        [StringLength(3)]
        public string TpSangue { get; set; }

        [StringLength(1)]
        public string Doador { get; set; }

        public float? Peso { get; set; }

        public float? Altura { get; set; }

        public long? CodCross { get; set; }

        [StringLength(20)]
        public string UfNacao { get; set; }

        [StringLength(100)]
        public string MuniNacao { get; set; }

        public int? MaeDesconhecida { get; set; }

        public int? PaiDesconhecido { get; set; }

        public int? Nacionalidade { get; set; }

        public int? MUNICIPIONASC { get; set; }

        [StringLength(16)]
        public string NATURALIZACAOPORTARIA { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NATURALIZADADATA { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ESTRANGEIRADATA { get; set; }

        public int? FALECIDO { get; set; }

        [Column(TypeName = "date")]
        public DateTime? OBITODATA { get; set; }

        [StringLength(10)]
        public string OBITONUMERO { get; set; }

        public int? ENDPAIS { get; set; }

        public int? ENDMUNICIPIOEXT { get; set; }

        [StringLength(10)]
        public string OCUPACAO { get; set; }

        public int? ESCOLARIDADE { get; set; }

        public int? ORIENTACAO { get; set; }

        public int? GENERO { get; set; }

        public virtual ASSMED_Cadastro ASSMED_Cadastro { get; set; }
    }
}
