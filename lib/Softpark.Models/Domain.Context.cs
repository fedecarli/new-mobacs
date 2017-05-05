﻿

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Softpark.Models
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using System.Data.Entity.Core.Objects;
using System.Linq;


public partial class DomainContainer : DbContext
{
    public DomainContainer()
        : base("name=DomainContainer")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<VW_IdentificacaoUsuarioCidadao> VW_IdentificacaoUsuarioCidadao { get; set; }

    public virtual DbSet<VW_profissional_cns> VW_profissional_cns { get; set; }

    public virtual DbSet<VW_ultimo_cadastroDomiciliar> VW_ultimo_cadastroDomiciliar { get; set; }

    public virtual DbSet<VW_ultimo_cadastroIndividual> VW_ultimo_cadastroIndividual { get; set; }

    public virtual DbSet<AnimalNoDomicilio> AnimalNoDomicilio { get; set; }

    public virtual DbSet<CadastroDomiciliar> CadastroDomiciliar { get; set; }

    public virtual DbSet<CadastroIndividual> CadastroIndividual { get; set; }

    public virtual DbSet<CondicaoMoradia> CondicaoMoradia { get; set; }

    public virtual DbSet<CondicoesDeSaude> CondicoesDeSaude { get; set; }

    public virtual DbSet<DeficienciasCidadao> DeficienciasCidadao { get; set; }

    public virtual DbSet<DoencaCardiaca> DoencaCardiaca { get; set; }

    public virtual DbSet<DoencaRespiratoria> DoencaRespiratoria { get; set; }

    public virtual DbSet<DoencaRins> DoencaRins { get; set; }

    public virtual DbSet<EmSituacaoDeRua> EmSituacaoDeRua { get; set; }

    public virtual DbSet<EnderecoLocalPermanencia> EnderecoLocalPermanencia { get; set; }

    public virtual DbSet<FamiliaRow> FamiliaRow { get; set; }

    public virtual DbSet<FichaVisitaDomiciliarChild> FichaVisitaDomiciliarChild { get; set; }

    public virtual DbSet<FichaVisitaDomiciliarMaster> FichaVisitaDomiciliarMaster { get; set; }

    public virtual DbSet<HigienePessoalSituacaoRua> HigienePessoalSituacaoRua { get; set; }

    public virtual DbSet<IdentificacaoUsuarioCidadao> IdentificacaoUsuarioCidadao { get; set; }

    public virtual DbSet<InformacoesSocioDemograficas> InformacoesSocioDemograficas { get; set; }

    public virtual DbSet<InstituicaoPermanencia> InstituicaoPermanencia { get; set; }

    public virtual DbSet<OrigemAlimentoSituacaoRua> OrigemAlimentoSituacaoRua { get; set; }

    public virtual DbSet<OrigemVisita> OrigemVisita { get; set; }

    public virtual DbSet<ResponsavelPorCrianca> ResponsavelPorCrianca { get; set; }

    public virtual DbSet<SaidaCidadaoCadastro> SaidaCidadaoCadastro { get; set; }

    public virtual DbSet<TipoOrigem> TipoOrigem { get; set; }

    public virtual DbSet<UnicaLotacaoTransport> UnicaLotacaoTransport { get; set; }

    public virtual DbSet<ASSMED_Cadastro> ASSMED_Cadastro { get; set; }

    public virtual DbSet<ASSMED_CadastroDocPessoal> ASSMED_CadastroDocPessoal { get; set; }

    public virtual DbSet<ASSMED_CadastroPF> ASSMED_CadastroPF { get; set; }

    public virtual DbSet<ASSMED_Endereco> ASSMED_Endereco { get; set; }

    public virtual DbSet<ASSMED_PesFisica> ASSMED_PesFisica { get; set; }

    public virtual DbSet<Cidade> Cidade { get; set; }

    public virtual DbSet<Etnia> Etnia { get; set; }

    public virtual DbSet<Paises> Paises { get; set; }

    public virtual DbSet<ProfCidadaoVinc> ProfCidadaoVinc { get; set; }

    public virtual DbSet<ProfCidadaoVincAgendaProd> ProfCidadaoVincAgendaProd { get; set; }

    public virtual DbSet<Setores> Setores { get; set; }

    public virtual DbSet<SetoresINEs> SetoresINEs { get; set; }

    public virtual DbSet<SituacaoFamiliar> SituacaoFamiliar { get; set; }

    public virtual DbSet<TP_Abastecimento_Agua> TP_Abastecimento_Agua { get; set; }

    public virtual DbSet<TP_Acesso_Domicilio> TP_Acesso_Domicilio { get; set; }

    public virtual DbSet<TP_Animais> TP_Animais { get; set; }

    public virtual DbSet<TP_Cond_Posse_Uso_Terra> TP_Cond_Posse_Uso_Terra { get; set; }

    public virtual DbSet<TP_Condicao_Avaliada> TP_Condicao_Avaliada { get; set; }

    public virtual DbSet<TP_Consideracao_Peso> TP_Consideracao_Peso { get; set; }

    public virtual DbSet<TP_Construcao_Domicilio> TP_Construcao_Domicilio { get; set; }

    public virtual DbSet<TP_Crianca> TP_Crianca { get; set; }

    public virtual DbSet<TP_Curso> TP_Curso { get; set; }

    public virtual DbSet<TP_Deficiencia> TP_Deficiencia { get; set; }

    public virtual DbSet<TP_Destino_Lixo> TP_Destino_Lixo { get; set; }

    public virtual DbSet<TP_Doenca_Cardiaca> TP_Doenca_Cardiaca { get; set; }

    public virtual DbSet<TP_Doenca_Renal> TP_Doenca_Renal { get; set; }

    public virtual DbSet<TP_Doenca_Respiratoria> TP_Doenca_Respiratoria { get; set; }

    public virtual DbSet<TP_Domicilio> TP_Domicilio { get; set; }

    public virtual DbSet<TP_Escoamento_Esgoto> TP_Escoamento_Esgoto { get; set; }

    public virtual DbSet<TP_Exames> TP_Exames { get; set; }

    public virtual DbSet<TP_Higiene_Pessoal> TP_Higiene_Pessoal { get; set; }

    public virtual DbSet<TP_Identidade_Genero_Cidadao> TP_Identidade_Genero_Cidadao { get; set; }

    public virtual DbSet<TP_Imovel> TP_Imovel { get; set; }

    public virtual DbSet<TP_Localizacao> TP_Localizacao { get; set; }

    public virtual DbSet<TP_Motivo_Saida> TP_Motivo_Saida { get; set; }

    public virtual DbSet<TP_Nacionalidade> TP_Nacionalidade { get; set; }

    public virtual DbSet<TP_Orientacao_Sexual> TP_Orientacao_Sexual { get; set; }

    public virtual DbSet<TP_Origem_Alimentacao> TP_Origem_Alimentacao { get; set; }

    public virtual DbSet<TP_Quantas_Vezes_Alimentacao> TP_Quantas_Vezes_Alimentacao { get; set; }

    public virtual DbSet<TP_Raca_Cor> TP_Raca_Cor { get; set; }

    public virtual DbSet<TP_Relacao_Parentesco> TP_Relacao_Parentesco { get; set; }

    public virtual DbSet<TP_Renda_Familiar> TP_Renda_Familiar { get; set; }

    public virtual DbSet<TP_Saida_Cadastro> TP_Saida_Cadastro { get; set; }

    public virtual DbSet<TP_Sexo> TP_Sexo { get; set; }

    public virtual DbSet<TP_Sexo_Genero> TP_Sexo_Genero { get; set; }

    public virtual DbSet<TP_Sit_Mercado> TP_Sit_Mercado { get; set; }

    public virtual DbSet<TP_Sit_Rua> TP_Sit_Rua { get; set; }

    public virtual DbSet<TP_Situacao_Moradia> TP_Situacao_Moradia { get; set; }

    public virtual DbSet<TP_Situacao_Moradia_Rural> TP_Situacao_Moradia_Rural { get; set; }

    public virtual DbSet<TP_Tratamento_Agua> TP_Tratamento_Agua { get; set; }

    public virtual DbSet<UF> UF { get; set; }

    public virtual DbSet<TipoLogradouro> TipoLogradouro { get; set; }

    public virtual DbSet<TP_Conduta> TP_Conduta { get; set; }

    public virtual DbSet<TP_Nasf> TP_Nasf { get; set; }

    public virtual DbSet<SIGSM_MotivoVisita> SIGSM_MotivoVisita { get; set; }

    public virtual DbSet<AS_Profissoes> AS_Profissoes { get; set; }

    public virtual DbSet<AS_ProfissoesTab> AS_ProfissoesTab { get; set; }

    public virtual DbSet<TB_MS_TIPO_LOGRADOURO> TB_MS_TIPO_LOGRADOURO { get; set; }

    public virtual DbSet<AS_SetoresPar> AS_SetoresPar { get; set; }


    public virtual int PR_ProcessarFichasAPI(Nullable<System.Guid> token)
    {

        var tokenParameter = token.HasValue ?
            new ObjectParameter("token", token) :
            new ObjectParameter("token", typeof(System.Guid));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PR_ProcessarFichasAPI", tokenParameter);
    }


    public virtual int PR_EncerrarAgenda(Nullable<int> idAgendaProd, Nullable<bool> retorno, Nullable<bool> tipoFicha)
    {

        var idAgendaProdParameter = idAgendaProd.HasValue ?
            new ObjectParameter("idAgendaProd", idAgendaProd) :
            new ObjectParameter("idAgendaProd", typeof(int));


        var retornoParameter = retorno.HasValue ?
            new ObjectParameter("Retorno", retorno) :
            new ObjectParameter("Retorno", typeof(bool));


        var tipoFichaParameter = tipoFicha.HasValue ?
            new ObjectParameter("TipoFicha", tipoFicha) :
            new ObjectParameter("TipoFicha", typeof(bool));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PR_EncerrarAgenda", idAgendaProdParameter, retornoParameter, tipoFichaParameter);
    }

}

}

