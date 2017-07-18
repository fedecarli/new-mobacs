using Softpark.Infrastructure.Extras;
using Softpark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Softpark.WS.ViewModels
{
    /// <summary>
    /// ViewModel Collection de cadastros individuais
    /// </summary>
    public class CadastroIndividualViewModelCollection : List<GetCadastroIndividualViewModel>
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public CadastroIndividualViewModelCollection() { }

        public CadastroIndividualViewModelCollection(CadastroIndividual[] models)
        {
            AddRange(models);
        }

        public CadastroIndividualViewModelCollection(List<CadastroIndividual> models) : this(models.ToArray())
        {
        }

        public CadastroIndividualViewModelCollection(IEnumerable<GetCadastroIndividualViewModel> models) : base(models) { }

        public static implicit operator CadastroIndividualViewModelCollection(List<CadastroIndividual> models)
        {
            return new CadastroIndividualViewModelCollection(models);
        }

        public static implicit operator CadastroIndividualViewModelCollection(CadastroIndividual[] models)
        {
            return new CadastroIndividualViewModelCollection(models);
        }

        public static implicit operator CadastroIndividualViewModelCollection(GetCadastroIndividualViewModel[] models)
        {
            return new CadastroIndividualViewModelCollection(models);
        }

        public void AddRange(CadastroIndividual[] models)
        {
            foreach (var model in models)
            {
                Add(model);
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// Ficha de cadastro individual
    /// </summary>
    public class PrimitiveCadastroIndividualViewModel
    {
        /// <summary>
        /// Token de acesso
        /// </summary>
        public Guid? token { get; set; } = null;
        /// <summary>
        /// Condições de Saúde
        /// </summary>
        public CondicoesDeSaudeViewModel condicoesDeSaude { get; set; } = null;
        /// <summary>
        /// Em Situação de Rua
        /// </summary>
        public EmSituacaoDeRuaViewModel emSituacaoDeRua { get; set; } = null;
        /// <summary>
        /// Ficha atualizada
        /// </summary>
        public bool fichaAtualizada { get; set; }
        /// <summary>
        /// Identificação do usuário cidadão
        /// </summary>
        public IdentificacaoUsuarioCidadaoViewModel identificacaoUsuarioCidadao { get; set; } = null;
        /// <summary>
        /// Informações socio demográficas
        /// </summary>
        public InformacoesSocioDemograficasViewModel informacoesSocioDemograficas { get; set; } = null;
        /// <summary>
        /// Termo de cadastro recusado
        /// </summary>
        public bool statusTermoRecusaCadastroIndividualAtencaoBasica { get; set; }
        /// <summary>
        /// Ficha de origem, informar somente se a ficha for de atualização
        /// </summary>
        public Guid? uuidFichaOriginadora { get; set; } = null;
        /// <summary>
        /// Dados da saída do cidadão do cadastro
        /// </summary>
        public SaidaCidadaoCadastroViewModel saidaCidadaoCadastro { get; set; } = null;

        /// <summary>
        /// Latitude de demarcação do início do cadastro
        /// </summary>
        public string latitude { get; set; } = null;

        /// <summary>
        /// Latitude de demarcação do início do cadastro
        /// </summary>
        public string longitude { get; set; } = null;

        /// <summary>
        /// Data de registro da ficha no app
        /// </summary>
        public DateTime? DataRegistro { get; set; } = null;

        /// <summary>
        /// Justificativa
        /// </summary>
        public string Justificativa { get; set; } = null;

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        internal async Task<CadastroIndividual> ToModel(DomainContainer domain)
        {
            var ci = domain.CadastroIndividual.Create();

            ci.id = Guid.NewGuid();
            ci.CondicoesDeSaude1 = await condicoesDeSaude?.ToModel(domain);
            ci.EmSituacaoDeRua1 = await emSituacaoDeRua?.ToModel(domain);
            ci.fichaAtualizada = fichaAtualizada;
            ci.IdentificacaoUsuarioCidadao1 = identificacaoUsuarioCidadao?.ToModel(domain);
            ci.InformacoesSocioDemograficas1 = await informacoesSocioDemograficas?.ToModel(domain);
            ci.statusTermoRecusaCadastroIndividualAtencaoBasica = statusTermoRecusaCadastroIndividualAtencaoBasica;
            ci.uuidFichaOriginadora = uuidFichaOriginadora;
            ci.SaidaCidadaoCadastro1 = saidaCidadaoCadastro?.ToModel(domain);
            ci.latitude = latitude;
            ci.longitude = longitude;
            ci.Justificativa = Justificativa;
            ci.DataRegistro = DataRegistro;

            return ci;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator PrimitiveCadastroIndividualViewModel(CadastroIndividual model)
        {
            var vm = new PrimitiveCadastroIndividualViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        internal void ApplyModel(CadastroIndividual model)
        {
            if (model == null) return;

            token = model.UnicaLotacaoTransport.token;
            condicoesDeSaude = model.CondicoesDeSaude1;
            emSituacaoDeRua = model.EmSituacaoDeRua1;
            fichaAtualizada = model.fichaAtualizada;
            identificacaoUsuarioCidadao = model.IdentificacaoUsuarioCidadao1;
            informacoesSocioDemograficas = model.InformacoesSocioDemograficas1;
            statusTermoRecusaCadastroIndividualAtencaoBasica = model.statusTermoRecusaCadastroIndividualAtencaoBasica;
            uuidFichaOriginadora = model.uuidFichaOriginadora;
            saidaCidadaoCadastro = model.SaidaCidadaoCadastro1;
            latitude = model.latitude;
            longitude = model.longitude;
            Justificativa = model.Justificativa;
            DataRegistro = model.DataRegistro;
        }
    }

    /// <summary>
    /// Ficha de cadastro individual
    /// </summary>
    public class CadastroIndividualViewModel : PrimitiveCadastroIndividualViewModel
    {
        /// <summary>
        /// Token de acesso
        /// </summary>
        [Required]
        public new Guid token { get; set; }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator CadastroIndividualViewModel(CadastroIndividual model)
        {
            var vm = new CadastroIndividualViewModel();

            vm.ApplyModel(model);

            return vm;
        }
    }

    /// <summary>
    /// ViewModel de consulta de cadastro individual
    /// </summary>
    public class GetCadastroIndividualViewModel : CadastroIndividualViewModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid uuid { get; set; }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator GetCadastroIndividualViewModel(CadastroIndividual model)
        {
            var vm = new GetCadastroIndividualViewModel();

            if (model != null)
                vm.uuid = model.id;

            vm.ApplyModel(model);

            return vm;
        }
    }

    /// <summary>
    /// Dados da saída do cidadão do cadastro
    /// </summary>
    public class SaidaCidadaoCadastroViewModel
    {
        /// <summary>
        /// Motivo da saída do cidadão
        /// </summary>
        public int? motivoSaidaCidadao { get; set; } = null;
        /// <summary>
        /// Data de óbito
        /// </summary>
        public DateTime? dataObito { get; set; } = null;
        /// <summary>
        /// Número da Declaração
        /// </summary>
        public string numeroDO { get; set; } = null;

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator SaidaCidadaoCadastroViewModel(SaidaCidadaoCadastro model)
        {
            var vm = new SaidaCidadaoCadastroViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        private void ApplyModel(SaidaCidadaoCadastro model)
        {
            if (model == null) return;

            motivoSaidaCidadao = model.motivoSaidaCidadao;
            dataObito = model.dataObito;
            numeroDO = model.numeroDO;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="domain"></param>
        internal SaidaCidadaoCadastro ToModel(DomainContainer domain)
        {
            var scc = domain.SaidaCidadaoCadastro.Create();

            scc.id = Guid.NewGuid();
            scc.motivoSaidaCidadao = motivoSaidaCidadao;
            scc.dataObito = dataObito;
            scc.numeroDO = numeroDO;

            return scc;
        }
    }

    /// <summary>
    /// Informações sócio demográficas
    /// </summary>
    public class InformacoesSocioDemograficasViewModel
    {
        /// <summary>
        /// Graud de instrução
        /// </summary>
        public int? grauInstrucaoCidadao { get; set; } = null;
        /// <summary>
        /// ocupação
        /// </summary>
        public string ocupacaoCodigoCbo2002 { get; set; } = null;
        /// <summary>
        /// orientação sexual
        /// </summary>
        public int? orientacaoSexualCidadao { get; set; } = null;
        /// <summary>
        /// comunidade
        /// </summary>
        public string povoComunidadeTradicional { get; set; } = null;
        /// <summary>
        /// parentesco
        /// </summary>
        public int? relacaoParentescoCidadao { get; set; } = null;
        /// <summary>
        /// situação / trabalho
        /// </summary>
        public int? situacaoMercadoTrabalhoCidadao { get; set; } = null;
        /// <summary>
        /// Deseja informar a orientação sexual
        /// </summary>
        public bool statusDesejaInformarOrientacaoSexual { get; set; }
        /// <summary>
        /// frequenta benzedeira
        /// </summary>
        public bool statusFrequentaBenzedeira { get; set; }
        /// <summary>
        /// frequenta escola
        /// </summary>
        public bool statusFrequentaEscola { get; set; }
        /// <summary>
        /// membro de comunidade
        /// </summary>
        public bool statusMembroPovoComunidadeTradicional { get; set; }
        /// <summary>
        /// participa do grupo comunitário
        /// </summary>
        public bool statusParticipaGrupoComunitario { get; set; }
        /// <summary>
        /// possui plano de saude privado
        /// </summary>
        public bool statusPossuiPlanoSaudePrivado { get; set; }
        /// <summary>
        /// tem alguma deficiencia
        /// </summary>
        public bool statusTemAlgumaDeficiencia { get; set; }
        /// <summary>
        /// identidade de gênero
        /// </summary>
        public int? identidadeGeneroCidadao { get; set; } = null;
        /// <summary>
        /// deseja informar a identidade de genero
        /// </summary>
        public bool statusDesejaInformarIdentidadeGenero { get; set; }
        /// <summary>
        /// Lista de deficiências do cidadão
        /// </summary>
        public int[] deficienciasCidadao { get; set; } = new int[0];
        /// <summary>
        /// Lista de tipo de responsabilidade por crianças
        /// </summary>
        public int[] responsavelPorCrianca { get; set; } = new int[0];

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator InformacoesSocioDemograficasViewModel(InformacoesSocioDemograficas model)
        {
            var vm = new InformacoesSocioDemograficasViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        private void ApplyModel(InformacoesSocioDemograficas model)
        {
            if (model == null) return;

            deficienciasCidadao = model.DeficienciasCidadao.Select(d => d.id_tp_deficiencia_cidadao).ToArray();

            grauInstrucaoCidadao = model.grauInstrucaoCidadao;
            ocupacaoCodigoCbo2002 = model.ocupacaoCodigoCbo2002;
            orientacaoSexualCidadao = model.orientacaoSexualCidadao;
            povoComunidadeTradicional = model.povoComunidadeTradicional;
            relacaoParentescoCidadao = model.relacaoParentescoCidadao;
            situacaoMercadoTrabalhoCidadao = model.situacaoMercadoTrabalhoCidadao;
            statusDesejaInformarOrientacaoSexual = model.statusDesejaInformarOrientacaoSexual;
            statusFrequentaBenzedeira = model.statusFrequentaBenzedeira;
            statusFrequentaEscola = model.statusFrequentaEscola;
            statusMembroPovoComunidadeTradicional = model.statusMembroPovoComunidadeTradicional;
            statusParticipaGrupoComunitario = model.statusParticipaGrupoComunitario;
            statusPossuiPlanoSaudePrivado = model.statusPossuiPlanoSaudePrivado;
            statusTemAlgumaDeficiencia = model.statusTemAlgumaDeficiencia;
            identidadeGeneroCidadao = model.identidadeGeneroCidadao;
            statusDesejaInformarIdentidadeGenero = model.statusDesejaInformarIdentidadeGenero;
            responsavelPorCrianca = model.ResponsavelPorCrianca.Select(d => d.id_tp_crianca).ToArray();
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="domain"></param>
        internal async Task<InformacoesSocioDemograficas> ToModel(DomainContainer domain)
        {
            var isd = domain.InformacoesSocioDemograficas.Create();
            isd.id = Guid.NewGuid();

            TP_Deficiencia dc;
            foreach (var _dc in deficienciasCidadao)
                if ((dc = await domain.TP_Deficiencia.FirstOrDefaultAsync(y => y.codigo == _dc)) != null)
                {
                    DeficienciasCidadao @dcs = domain.DeficienciasCidadao.Create();
                    @dcs.id_tp_deficiencia_cidadao = dc.codigo;
                    @dcs.InformacoesSocioDemograficas = isd;

                    isd.DeficienciasCidadao.Add(@dcs);
                    domain.DeficienciasCidadao.Add(@dcs);
                }

            TP_Crianca cr;
            foreach (var _cr in responsavelPorCrianca)
                if ((cr = await domain.TP_Crianca.FirstOrDefaultAsync(y => y.codigo == _cr)) != null)
                {
                    ResponsavelPorCrianca @crs = domain.ResponsavelPorCrianca.Create();
                    @crs.InformacoesSocioDemograficas = isd;
                    @crs.id_tp_crianca = cr.codigo;

                    isd.ResponsavelPorCrianca.Add(@crs);
                    domain.ResponsavelPorCrianca.Add(@crs);
                }

            isd.grauInstrucaoCidadao = grauInstrucaoCidadao;
            isd.ocupacaoCodigoCbo2002 = ocupacaoCodigoCbo2002;
            isd.orientacaoSexualCidadao = orientacaoSexualCidadao;
            isd.povoComunidadeTradicional = povoComunidadeTradicional;
            isd.relacaoParentescoCidadao = relacaoParentescoCidadao;
            isd.situacaoMercadoTrabalhoCidadao = situacaoMercadoTrabalhoCidadao;
            isd.statusDesejaInformarOrientacaoSexual = statusDesejaInformarOrientacaoSexual;
            isd.statusFrequentaBenzedeira = statusFrequentaBenzedeira;
            isd.statusFrequentaEscola = statusFrequentaEscola;
            isd.statusMembroPovoComunidadeTradicional = statusMembroPovoComunidadeTradicional;
            isd.statusParticipaGrupoComunitario = statusParticipaGrupoComunitario;
            isd.statusPossuiPlanoSaudePrivado = statusPossuiPlanoSaudePrivado;
            isd.statusTemAlgumaDeficiencia = statusTemAlgumaDeficiencia;
            isd.identidadeGeneroCidadao = identidadeGeneroCidadao;
            isd.statusDesejaInformarIdentidadeGenero = statusDesejaInformarIdentidadeGenero;

            return isd;
        }
    }

    /// <summary>
    /// identificação do usuário cidadão
    /// </summary>
    public class IdentificacaoUsuarioCidadaoViewModel
    {
        /// <summary>
        /// não usado no cadastro
        /// </summary>
        public Guid? id { get; set; } = null;
        /// <summary>
        /// nome social
        /// </summary>
        public string nomeSocial { get; set; } = null;
        /// <summary>
        /// codigo do municipio de nascimento
        /// </summary>
        public string codigoIbgeMunicipioNascimento { get; set; } = null;
        /// <summary>
        /// data de nascimento
        /// </summary>
        public DateTime? dataNascimentoCidadao { get; set; }
        /// <summary>
        /// desconhece o nome da mãe
        /// </summary>
        public bool desconheceNomeMae { get; set; }
        /// <summary>
        /// email do cidadão
        /// </summary>
        public string emailCidadao { get; set; } = null;
        /// <summary>
        /// nacionalidade do cidadão
        /// </summary>
        public int nacionalidadeCidadao { get; set; }
        /// <summary>
        /// nome do cidadão
        /// </summary>
        public string nomeCidadao { get; set; } = null;
        /// <summary>
        /// nome da mãe do cidadão
        /// </summary>
        public string nomeMaeCidadao { get; set; } = null;
        /// <summary>
        /// CNS do cidadão
        /// </summary>
        public string cnsCidadao { get; set; } = null;
        /// <summary>
        /// CNS do responsável familiar
        /// </summary>
        public string cnsResponsavelFamiliar { get; set; } = null;
        /// <summary>
        /// telefone celular
        /// </summary>
        public string telefoneCelular { get; set; } = null;
        /// <summary>
        /// NIS/PIS/PASEP
        /// </summary>
        public string numeroNisPisPasep { get; set; } = null;
        /// <summary>
        /// País de nascimento
        /// </summary>
        public int? paisNascimento { get; set; } = null;
        /// <summary>
        /// Raça/Cor
        /// </summary>
        public int racaCorCidadao { get; set; }
        /// <summary>
        /// Sexo
        /// </summary>
        public int sexoCidadao { get; set; }
        /// <summary>
        /// É responsável
        /// </summary>
        public bool statusEhResponsavel { get; set; }
        /// <summary>
        /// Etnia
        /// </summary>
        public int? etnia { get; set; } = null;
        /// <summary>
        /// Nome do Pai
        /// </summary>
        public string nomePaiCidadao { get; set; } = null;
        /// <summary>
        /// Desconhece o nome do Pai
        /// </summary>
        public bool desconheceNomePai { get; set; }
        /// <summary>
        /// Data da naturalização
        /// </summary>
        public DateTime? dtNaturalizacao { get; set; } = null;
        /// <summary>
        /// Portaria da Naturalização
        /// </summary>
        public string portariaNaturalizacao { get; set; } = null;
        /// <summary>
        /// Data de entrada no Brasil
        /// </summary>
        public DateTime? dtEntradaBrasil { get; set; } = null;
        /// <summary>
        /// Microárea
        /// </summary>
        public string microarea { get; set; } = null;
        /// <summary>
        /// Fora de área
        /// </summary>
        public bool stForaArea { get; set; }
        /// <summary>
        /// RG do cidadão
        /// </summary>
        public string RG { get; set; } = null;
        /// <summary>
        /// CPF do cidadão
        /// </summary>
        public string CPF { get; set; } = null;
        /// <summary>
        /// Cidadão é beneficiário do bolsa família?
        /// </summary>
        public bool beneficiarioBolsaFamilia { get; set; } = false;
        /// <summary>
        /// Estado Civil
        /// </summary>
        [Required(AllowEmptyStrings = false), DefaultValue("I")]
        public string EstadoCivil { get; set; } = "I";

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator IdentificacaoUsuarioCidadaoViewModel(IdentificacaoUsuarioCidadao model)
        {
            var vm = new IdentificacaoUsuarioCidadaoViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator IdentificacaoUsuarioCidadaoViewModel(VW_IdentificacaoUsuarioCidadao model)
        {
            var vm = new IdentificacaoUsuarioCidadaoViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        private void ApplyModel(VW_IdentificacaoUsuarioCidadao model)
        {
            if (model == null) return;

            id = model.id;
            nomeSocial = model.nomeSocial;
            codigoIbgeMunicipioNascimento = model.codigoIbgeMunicipioNascimento;
            dataNascimentoCidadao = model.dataNascimentoCidadao;
            desconheceNomeMae = model.desconheceNomeMae ?? false;
            emailCidadao = model.emailCidadao;
            nacionalidadeCidadao = model.nacionalidadeCidadao ?? 1;
            nomeCidadao = model.nomeCidadao;
            nomeMaeCidadao = model.nomeMaeCidadao;
            cnsCidadao = model.cnsCidadao;
            cnsResponsavelFamiliar = model.cnsResponsavelFamiliar;
            telefoneCelular = model.telefoneCelular;
            numeroNisPisPasep = model.numeroNisPisPasep;
            paisNascimento = model.paisNascimento;
            racaCorCidadao = model.racaCorCidadao;
            sexoCidadao = model.sexoCidadao;
            statusEhResponsavel = model.statusEhResponsavel ?? false;
            etnia = model.etnia;
            nomePaiCidadao = model.nomePaiCidadao;
            desconheceNomePai = model.desconheceNomePai ?? false;
            dtNaturalizacao = model.dtNaturalizacao;
            portariaNaturalizacao = model.portariaNaturalizacao;
            dtEntradaBrasil = model.dtEntradaBrasil;
            microarea = model.microarea;
            stForaArea = model.stForaArea ?? false;
            RG = model.RG;
            CPF = model.CPF;
            beneficiarioBolsaFamilia = model.beneficiarioBolsaFamilia??false;
            EstadoCivil = model.EstadoCivil;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        private void ApplyModel(IdentificacaoUsuarioCidadao model)
        {
            if (model == null) return;

            id = model.id;
            nomeSocial = model.nomeSocial;
            codigoIbgeMunicipioNascimento = model.codigoIbgeMunicipioNascimento;
            dataNascimentoCidadao = model.dataNascimentoCidadao;
            desconheceNomeMae = model.desconheceNomeMae;
            emailCidadao = model.emailCidadao;
            nacionalidadeCidadao = model.nacionalidadeCidadao;
            nomeCidadao = model.nomeCidadao;
            nomeMaeCidadao = model.nomeMaeCidadao;
            cnsCidadao = model.cnsCidadao;
            cnsResponsavelFamiliar = model.cnsResponsavelFamiliar;
            telefoneCelular = model.telefoneCelular;
            numeroNisPisPasep = model.numeroNisPisPasep;
            paisNascimento = model.paisNascimento;
            racaCorCidadao = model.racaCorCidadao;
            sexoCidadao = model.sexoCidadao;
            statusEhResponsavel = model.statusEhResponsavel;
            etnia = model.etnia;
            nomePaiCidadao = model.nomePaiCidadao;
            desconheceNomePai = model.desconheceNomePai;
            dtNaturalizacao = model.dtNaturalizacao;
            portariaNaturalizacao = model.portariaNaturalizacao;
            dtEntradaBrasil = model.dtEntradaBrasil;
            microarea = model.microarea;
            stForaArea = model.stForaArea;
            RG = model.RG;
            CPF = model.CPF;
            beneficiarioBolsaFamilia = model.beneficiarioBolsaFamilia??false;
            EstadoCivil = model.EstadoCivil;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="domain"></param>
        internal IdentificacaoUsuarioCidadao ToModel(DomainContainer domain)
        {
            var iuc = domain.IdentificacaoUsuarioCidadao.Create();

            iuc.id = Guid.NewGuid();
            iuc.nomeSocial = nomeSocial;
            iuc.codigoIbgeMunicipioNascimento = codigoIbgeMunicipioNascimento;
            iuc.dataNascimentoCidadao = dataNascimentoCidadao;
            iuc.desconheceNomeMae = desconheceNomeMae;
            iuc.emailCidadao = emailCidadao;
            iuc.nacionalidadeCidadao = nacionalidadeCidadao;
            iuc.nomeCidadao = nomeCidadao;
            iuc.nomeMaeCidadao = nomeMaeCidadao;
            iuc.cnsCidadao = cnsCidadao;
            iuc.cnsResponsavelFamiliar = cnsResponsavelFamiliar;
            iuc.telefoneCelular = telefoneCelular;
            iuc.numeroNisPisPasep = numeroNisPisPasep;
            iuc.paisNascimento = paisNascimento;
            iuc.racaCorCidadao = racaCorCidadao;
            iuc.sexoCidadao = sexoCidadao;
            iuc.statusEhResponsavel = statusEhResponsavel;
            iuc.etnia = etnia;
            iuc.nomePaiCidadao = nomePaiCidadao;
            iuc.desconheceNomePai = desconheceNomePai;
            iuc.dtNaturalizacao = dtNaturalizacao;
            iuc.portariaNaturalizacao = portariaNaturalizacao;
            iuc.dtEntradaBrasil = dtEntradaBrasil;
            iuc.microarea = microarea;
            iuc.stForaArea = stForaArea;
            iuc.RG = RG;
            iuc.CPF = CPF;
            iuc.beneficiarioBolsaFamilia = beneficiarioBolsaFamilia;
            iuc.EstadoCivil = EstadoCivil;

            return iuc;
        }
    }

    /// <summary>
    /// Em situação de rua
    /// </summary>
    public class EmSituacaoDeRuaViewModel
    {
        /// <summary>
        /// Grau de parentesco
        /// </summary>
        public string grauParentescoFamiliarFrequentado { get; set; } = null;
        /// <summary>
        /// Outra instituição
        /// </summary>
        public string outraInstituicaoQueAcompanha { get; set; } = null;
        /// <summary>
        /// Quantidade de refeições diária
        /// </summary>
        public int? quantidadeAlimentacoesAoDiaSituacaoRua { get; set; } = null;
        /// <summary>
        /// Acompanhado por outra instituição
        /// </summary>
        public bool statusAcompanhadoPorOutraInstituicao { get; set; }
        /// <summary>
        /// Possui referência familiar
        /// </summary>
        public bool statusPossuiReferenciaFamiliar { get; set; }
        /// <summary>
        /// Recebe beneficio
        /// </summary>
        public bool statusRecebeBeneficio { get; set; }
        /// <summary>
        /// Situação de Rua
        /// </summary>
        public bool statusSituacaoRua { get; set; }
        /// <summary>
        /// Tem acesso à higiene pessoa
        /// </summary>
        public bool statusTemAcessoHigienePessoalSituacaoRua { get; set; }
        /// <summary>
        /// Visita familiar frequentemente
        /// </summary>
        public bool statusVisitaFamiliarFrequentemente { get; set; }
        /// <summary>
        /// Tempo da situação de rua
        /// </summary>
        public int? tempoSituacaoRua { get; set; } = null;
        /// <summary>
        /// Lista de higiene pessoal
        /// </summary>
        public int[] higienePessoalSituacaoRua { get; set; } = new int[0];
        /// <summary>
        /// Lista de origem de alimentos
        /// </summary>
        public int[] origemAlimentoSituacaoRua { get; set; } = new int[0];

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator EmSituacaoDeRuaViewModel(EmSituacaoDeRua model)
        {
            var vm = new EmSituacaoDeRuaViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        private void ApplyModel(EmSituacaoDeRua model)
        {
            if (model == null) return;

            grauParentescoFamiliarFrequentado = model.grauParentescoFamiliarFrequentado;
            outraInstituicaoQueAcompanha = model.outraInstituicaoQueAcompanha;
            quantidadeAlimentacoesAoDiaSituacaoRua = model.quantidadeAlimentacoesAoDiaSituacaoRua;
            statusAcompanhadoPorOutraInstituicao = model.statusAcompanhadoPorOutraInstituicao;
            statusPossuiReferenciaFamiliar = model.statusPossuiReferenciaFamiliar;
            statusRecebeBeneficio = model.statusRecebeBeneficio;
            statusSituacaoRua = model.statusSituacaoRua;
            statusTemAcessoHigienePessoalSituacaoRua = model.statusTemAcessoHigienePessoalSituacaoRua;
            statusVisitaFamiliarFrequentemente = model.statusVisitaFamiliarFrequentemente;
            tempoSituacaoRua = model.tempoSituacaoRua;

            higienePessoalSituacaoRua = model.HigienePessoalSituacaoRua.Select(h => h.codigo_higiene_pessoal).ToArray();
            origemAlimentoSituacaoRua = model.OrigemAlimentoSituacaoRua.Select(o => o.id_tp_origem_alimento).ToArray();
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="domain"></param>
        internal async Task<EmSituacaoDeRua> ToModel(DomainContainer domain)
        {
            var esdr = domain.EmSituacaoDeRua.Create();

            esdr.id = Guid.NewGuid();
            esdr.grauParentescoFamiliarFrequentado = grauParentescoFamiliarFrequentado;
            esdr.outraInstituicaoQueAcompanha = outraInstituicaoQueAcompanha;
            esdr.quantidadeAlimentacoesAoDiaSituacaoRua = quantidadeAlimentacoesAoDiaSituacaoRua;
            esdr.statusAcompanhadoPorOutraInstituicao = statusAcompanhadoPorOutraInstituicao;
            esdr.statusPossuiReferenciaFamiliar = statusPossuiReferenciaFamiliar;
            esdr.statusRecebeBeneficio = statusRecebeBeneficio;
            esdr.statusSituacaoRua = statusSituacaoRua;
            esdr.statusTemAcessoHigienePessoalSituacaoRua = statusTemAcessoHigienePessoalSituacaoRua;
            esdr.statusVisitaFamiliarFrequentemente = statusVisitaFamiliarFrequentemente;
            esdr.tempoSituacaoRua = tempoSituacaoRua;

            TP_Higiene_Pessoal hpr;
            foreach (var _hpsr in higienePessoalSituacaoRua)
                if ((hpr = await domain.TP_Higiene_Pessoal.FirstOrDefaultAsync(y => y.codigo == _hpsr)) != null)
                {
                    var @dcs = domain.HigienePessoalSituacaoRua.Create();
                    @dcs.codigo_higiene_pessoal = hpr.codigo;
                    @dcs.EmSituacaoDeRua = esdr;

                    esdr.HigienePessoalSituacaoRua.Add(@dcs);
                    domain.HigienePessoalSituacaoRua.Add(@dcs);
                }

            TP_Origem_Alimentacao oasr;
            foreach (var _oasr in origemAlimentoSituacaoRua)
                if ((oasr = await domain.TP_Origem_Alimentacao.FirstOrDefaultAsync(y => y.codigo == _oasr)) != null)
                {
                    var @dcs = domain.OrigemAlimentoSituacaoRua.Create();
                    @dcs.id_tp_origem_alimento = oasr.codigo;
                    @dcs.EmSituacaoDeRua = esdr;

                    esdr.OrigemAlimentoSituacaoRua.Add(@dcs);
                    domain.OrigemAlimentoSituacaoRua.Add(@dcs);
                }

            return esdr;
        }
    }

    /// <summary>
    /// Condicoes de saáde
    /// </summary>
    public class CondicoesDeSaudeViewModel
    {
        /// <summary>
        /// Causa da internação
        /// </summary>
        public string descricaoCausaInternacaoEm12Meses { get; set; } = null;
        /// <summary>
        /// Descrição 1
        /// </summary>
        public string descricaoOutraCondicao1 { get; set; } = null;
        /// <summary>
        /// Descrição 2
        /// </summary>
        public string descricaoOutraCondicao2 { get; set; } = null;
        /// <summary>
        /// Descrição 3
        /// </summary>
        public string descricaoOutraCondicao3 { get; set; } = null;
        /// <summary>
        /// Plantas medicinais usadas
        /// </summary>
        public string descricaoPlantasMedicinaisUsadas { get; set; } = null;
        /// <summary>
        /// Maternidade
        /// </summary>
        public string maternidadeDeReferencia { get; set; } = null;
        /// <summary>
        /// Situação do Peso
        /// </summary>
        public int? situacaoPeso { get; set; } = null;
        /// <summary>
        /// É dependente de álcool
        /// </summary>
        public bool statusEhDependenteAlcool { get; set; }
        /// <summary>
        /// É dependente de outras drogas
        /// </summary>
        public bool statusEhDependenteOutrasDrogas { get; set; }
        /// <summary>
        /// É fumante
        /// </summary>
        public bool statusEhFumante { get; set; }
        /// <summary>
        /// É gestante
        /// </summary>
        public bool statusEhGestante { get; set; }
        /// <summary>
        /// Está acamado
        /// </summary>
        public bool statusEstaAcamado { get; set; }
        /// <summary>
        /// Está domiciliado
        /// </summary>
        public bool statusEstaDomiciliado { get; set; }
        /// <summary>
        /// Tem diabetes
        /// </summary>
        public bool statusTemDiabetes { get; set; }
        /// <summary>
        /// Tem doença respiratória
        /// </summary>
        public bool statusTemDoencaRespiratoria { get; set; }
        /// <summary>
        /// Tem hanseníase
        /// </summary>
        public bool statusTemHanseniase { get; set; }
        /// <summary>
        /// Tem hipertensão
        /// </summary>
        public bool statusTemHipertensaoArterial { get; set; }
        /// <summary>
        /// Tem/Teve câncer
        /// </summary>
        public bool statusTemTeveCancer { get; set; }
        /// <summary>
        /// Tem Doença renal
        /// </summary>
        public bool statusTemTeveDoencasRins { get; set; }
        /// <summary>
        /// Tem Tuberculose
        /// </summary>
        public bool statusTemTuberculose { get; set; }
        /// <summary>
        /// Teve AVC/Derrame
        /// </summary>
        public bool statusTeveAvcDerrame { get; set; }
        /// <summary>
        /// Teve/Tem doenca cardiaca
        /// </summary>
        public bool statusTeveDoencaCardiaca { get; set; }
        /// <summary>
        /// Teve/tem infarto
        /// </summary>
        public bool statusTeveInfarto { get; set; }
        /// <summary>
        /// Esteve/Está internado nos últimos 12 meses
        /// </summary>
        public bool statusTeveInternadoem12Meses { get; set; }
        /// <summary>
        /// Outras práticas integrativas / complementares
        /// </summary>
        public bool statusUsaOutrasPraticasIntegrativasOuComplementares { get; set; }
        /// <summary>
        /// Usa plantas medicionais
        /// </summary>
        public bool statusUsaPlantasMedicinais { get; set; }
        /// <summary>
        /// Diagnóstico mental
        /// </summary>
        public bool statusDiagnosticoMental { get; set; }
        /// <summary>
        /// Lista de doenças cardíacas
        /// </summary>
        public int[] doencaCardiaca { get; set; } = new int[0];
        /// <summary>
        /// Lista de doenças respiratórias
        /// </summary>
        public int[] doencaRespiratoria { get; set; } = new int[0];
        /// <summary>
        /// Lista de doenças renais
        /// </summary>
        public int[] doencaRins { get; set; } = new int[0];

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator CondicoesDeSaudeViewModel(CondicoesDeSaude model)
        {
            var vm = new CondicoesDeSaudeViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        private void ApplyModel(CondicoesDeSaude model)
        {
            if (model == null) return;

            descricaoCausaInternacaoEm12Meses = model.descricaoCausaInternacaoEm12Meses;
            descricaoOutraCondicao1 = model.descricaoOutraCondicao1;
            descricaoOutraCondicao2 = model.descricaoOutraCondicao2;
            descricaoOutraCondicao3 = model.descricaoOutraCondicao3;
            descricaoPlantasMedicinaisUsadas = model.descricaoPlantasMedicinaisUsadas;
            maternidadeDeReferencia = model.maternidadeDeReferencia;
            situacaoPeso = model.situacaoPeso;
            statusEhDependenteAlcool = model.statusEhDependenteAlcool;
            statusEhDependenteOutrasDrogas = model.statusEhDependenteOutrasDrogas;
            statusEhFumante = model.statusEhFumante;
            statusEhGestante = model.statusEhGestante;
            statusEstaAcamado = model.statusEstaAcamado;
            statusEstaDomiciliado = model.statusEstaDomiciliado;
            statusTemDiabetes = model.statusTemDiabetes;
            statusTemDoencaRespiratoria = model.statusTemDoencaRespiratoria;
            statusTemHanseniase = model.statusTemHanseniase;
            statusTemHipertensaoArterial = model.statusTemHipertensaoArterial;
            statusTemTeveCancer = model.statusTemTeveCancer;
            statusTemTeveDoencasRins = model.statusTemTeveDoencasRins;
            statusTemTuberculose = model.statusTemTuberculose;
            statusTeveAvcDerrame = model.statusTeveAvcDerrame;
            statusTeveDoencaCardiaca = model.statusTeveDoencaCardiaca;
            statusTeveInfarto = model.statusTeveInfarto;
            statusTeveInternadoem12Meses = model.statusTeveInternadoem12Meses;
            statusUsaOutrasPraticasIntegrativasOuComplementares = model.statusUsaOutrasPraticasIntegrativasOuComplementares;
            statusUsaPlantasMedicinais = model.statusUsaPlantasMedicinais;
            statusDiagnosticoMental = model.statusDiagnosticoMental;

            doencaCardiaca = model.DoencaCardiaca.Select(d => d.id_tp_doenca_cariaca).ToArray();
            doencaRespiratoria = model.DoencaRespiratoria.Select(d => d.id_tp_doenca_respiratoria).ToArray();
            doencaRins = model.DoencaRins.Select(x => x.id_tp_doenca_rins).ToArray();
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="domain"></param>
        internal async Task<CondicoesDeSaude> ToModel(DomainContainer domain)
        {
            var cds = domain.CondicoesDeSaude.Create();

            cds.id = Guid.NewGuid();
            cds.descricaoCausaInternacaoEm12Meses = descricaoCausaInternacaoEm12Meses;
            cds.descricaoOutraCondicao1 = descricaoOutraCondicao1;
            cds.descricaoOutraCondicao2 = descricaoOutraCondicao2;
            cds.descricaoOutraCondicao3 = descricaoOutraCondicao3;
            cds.descricaoPlantasMedicinaisUsadas = descricaoPlantasMedicinaisUsadas;
            cds.maternidadeDeReferencia = maternidadeDeReferencia;
            cds.situacaoPeso = situacaoPeso;
            cds.statusEhDependenteAlcool = statusEhDependenteAlcool;
            cds.statusEhDependenteOutrasDrogas = statusEhDependenteOutrasDrogas;
            cds.statusEhFumante = statusEhFumante;
            cds.statusEhGestante = statusEhGestante;
            cds.statusEstaAcamado = statusEstaAcamado;
            cds.statusEstaDomiciliado = statusEstaDomiciliado;
            cds.statusTemDiabetes = statusTemDiabetes;
            cds.statusTemDoencaRespiratoria = statusTemDoencaRespiratoria;
            cds.statusTemHanseniase = statusTemHanseniase;
            cds.statusTemHipertensaoArterial = statusTemHipertensaoArterial;
            cds.statusTemTeveCancer = statusTemTeveCancer;
            cds.statusTemTeveDoencasRins = statusTemTeveDoencasRins;
            cds.statusTemTuberculose = statusTemTuberculose;
            cds.statusTeveAvcDerrame = statusTeveAvcDerrame;
            cds.statusTeveDoencaCardiaca = statusTeveDoencaCardiaca;
            cds.statusTeveInfarto = statusTeveInfarto;
            cds.statusTeveInternadoem12Meses = statusTeveInternadoem12Meses;
            cds.statusUsaOutrasPraticasIntegrativasOuComplementares = statusUsaOutrasPraticasIntegrativasOuComplementares;
            cds.statusUsaPlantasMedicinais = statusUsaPlantasMedicinais;
            cds.statusDiagnosticoMental = statusDiagnosticoMental;

            TP_Doenca_Cardiaca dc;
            foreach (var _dcs in doencaCardiaca)
                if ((dc = await domain.TP_Doenca_Cardiaca.FirstOrDefaultAsync(y => y.codigo == _dcs)) != null)
                {
                    var @dcs = domain.DoencaCardiaca.Create();
                    @dcs.id_tp_doenca_cariaca = dc.codigo;
                    @dcs.CondicoesDeSaude = cds;

                    cds.DoencaCardiaca.Add(@dcs);
                    domain.DoencaCardiaca.Add(@dcs);
                }

            TP_Doenca_Respiratoria dr;
            foreach (var drs in doencaRespiratoria)
                if ((dr = await domain.TP_Doenca_Respiratoria.FirstOrDefaultAsync(y => y.codigo == drs)) != null)
                {
                    var @dcs = domain.DoencaRespiratoria.Create();
                    @dcs.id_tp_doenca_respiratoria = dr.codigo;
                    @dcs.CondicoesDeSaude = cds;

                    cds.DoencaRespiratoria.Add(@dcs);
                    domain.DoencaRespiratoria.Add(@dcs);
                }

            TP_Doenca_Renal dcr;
            foreach (var drr in doencaRins)
                if ((dcr = await domain.TP_Doenca_Renal.FirstOrDefaultAsync(y => y.codigo == drr)) != null)
                {
                    var @dcs = domain.DoencaRins.Create();
                    @dcs.id_tp_doenca_rins = dcr.codigo;
                    @dcs.CondicoesDeSaude = cds;

                    cds.DoencaRins.Add(@dcs);
                    domain.DoencaRins.Add(@dcs);
                }

            return cds;
        }
    }
}
