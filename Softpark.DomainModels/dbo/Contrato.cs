using Softpark.DomainModels.Attributes;

namespace Softpark.DomainModels.dbo
{
    [Table("ASSMED_Contratos")]
    public interface IContrato : IEntidade
    {
        [Column("NumContrato", IsKey = true)]
        int NumContrato { get; }

        [Column("NomeContratante")]
        string NomeContratante { get; }

        [Column("CodigoIbgeMunicipio")]
        string CodigoIbgeMunicipio { get; }
    }

    internal class Contrato : AEntidade<IContrato>, IContrato
    {
        public virtual int NumContrato { get; set; }

        public virtual string NomeContratante { get; set; }

        public virtual string CodigoIbgeMunicipio { get; set; }
    }
}
