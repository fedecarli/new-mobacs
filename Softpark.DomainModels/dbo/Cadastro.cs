using Softpark.DomainModels.api;
using Softpark.DomainModels.Attributes;
using System;

namespace Softpark.DomainModels.dbo
{
    [Table("ASSMED_Cadastro", Schema = "dbo")]
    public interface ICadastro : IEntidade
    {
        [BelongsTo("NumContrato")]
        IDataProxy<IContrato> Contrato { get; }

        [Column("NumContrato", IsKey = true, Order = 0)]
        int NumContrato { get; }

        [Column("Codigo", IsKey = true, Order = 1)]
        decimal Codigo { get; }

        [Column("Tipo")]
        char Tipo { get; set; }

        [Column("Nome")]
        string Nome { get; set; }

        [Column("DtSistema")]
        DateTime DataSistema { get; }

        [Column("CodUsu")]
        int CodUsu { get; set; }

        [Column("NumIP")]
        string NumIP { get; set; }

        [Column("NomeSocial")]
        string NomeSocial { get; set; }

        [BelongsTo("IdFicha")]
        IDataProxy<IIdentificacaoUsuarioCidadao> Ficha { get; }

        [Column("IdFicha")]
        Guid? IdFicha { get; set; }

        [Column("CodMunicipe")]
        int CodMunicipe { get; set; }

        [Column("DtAtualizacao")]
        DateTime DtAtualizacao { get; set; }

        [Column("CodTpHomologacao")]
        int CodTpHomologacao { get; set; }

        [Column("Justificativa")]
        string Justificativa { get; set; }

        [Column("MotivoHomologacao")]
        string MotivoHomologacao { get; set; }
    }

    internal class Cadastro : AEntidade<ICadastro>, ICadastro
    {
        public virtual IDataProxy<IContrato> Contrato { get; }

        public virtual int NumContrato { get; set; }

        public virtual decimal Codigo { get; set; }

        public virtual char Tipo { get; set; }

        public virtual string Nome { get; set; }

        public virtual DateTime DataSistema { get; set; }

        public virtual int CodUsu { get; set; }

        public virtual string NumIP { get; set; }

        public virtual string NomeSocial { get; set; }

        public virtual Guid? IdFicha { get; set; }

        public virtual int CodMunicipe { get; set; }

        public virtual DateTime DtAtualizacao { get; set; }

        public virtual int CodTpHomologacao { get; set; }

        public virtual string Justificativa { get; set; }

        public virtual string MotivoHomologacao { get; set; }

        public IDataProxy<IIdentificacaoUsuarioCidadao> Ficha { get; }
    }
}
