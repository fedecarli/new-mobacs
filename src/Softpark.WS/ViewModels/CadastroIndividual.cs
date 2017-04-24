using Softpark.Infrastructure.Extras;
using Softpark.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Softpark.WS.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class CadastroIndividualViewModelCollection : List<GetCadastroIndividualViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        public CadastroIndividualViewModelCollection() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        public CadastroIndividualViewModelCollection(IEnumerable<CadastroIndividual> models)
        {
            AddRange(models);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        public CadastroIndividualViewModelCollection(IEnumerable<GetCadastroIndividualViewModel> models)
        {
            AddRange(models);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static implicit operator CadastroIndividualViewModelCollection(CadastroIndividual[] models)
        {
            return new CadastroIndividualViewModelCollection(models);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static implicit operator CadastroIndividualViewModelCollection(GetCadastroIndividualViewModel[] models)
        {
            return new CadastroIndividualViewModelCollection(models);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        public void AddRange(IEnumerable<CadastroIndividual> models)
        {
            foreach (var model in models)
            {
                Add(model);
            }
        }
    }

    /// <summary>
    /// Ficha de cadastro individual
    /// </summary>
    public class CadastroIndividualViewModel
    {
        /// <summary>
        /// Token de acesso
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public Guid token { get; set; }
        /// <summary>
        /// Condições de Saúde
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public CondicoesDeSaudeViewModel condicoesDeSaude { get; set; }
        /// <summary>
        /// Em Situação de Rua
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public EmSituacaoDeRuaViewModel emSituacaoDeRua { get; set; }
        /// <summary>
        /// Ficha atualizada
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool fichaAtualizada { get; set; }
        /// <summary>
        /// Identificação do usuário cidadão
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public IdentificacaoUsuarioCidadaoViewModel identificacaoUsuarioCidadao { get; set; }
        /// <summary>
        /// Informações socio demográficas
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public InformacoesSocioDemograficasViewModel informacoesSocioDemograficas { get; set; }
        /// <summary>
        /// Termo de cadastro recusado
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusTermoRecusaCadastroIndividualAtencaoBasica { get; set; }
        /// <summary>
        /// Ficha de origem, informar somente se a ficha for de atualização
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string uuidFichaOriginadora { get; set; }
        /// <summary>
        /// Dados da saída do cidadão do cadastro
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public SaidaCidadaoCadastroViewModel saidaCidadaoCadastro { get; set; }

        /// <summary>
        /// Latitude de demarcação do início do cadastro
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string latitude { get; set; }

        /// <summary>
        /// Latitude de demarcação do início do cadastro
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string longitude { get; set; }

        internal async Task<CadastroIndividual> ToModel()
        {
            var ci = DomainContainer.Current.CadastroIndividual.Create();

            ci.id = Guid.NewGuid();
            var model = condicoesDeSaude?.ToModel();
            if (model != null) ci.CondicoesDeSaude1 = await model;
            var task = emSituacaoDeRua?.ToModel();
            if (task != null) ci.EmSituacaoDeRua1 = await task;
            ci.fichaAtualizada = fichaAtualizada;
            ci.IdentificacaoUsuarioCidadao1 = identificacaoUsuarioCidadao?.ToModel();
            var o = informacoesSocioDemograficas?.ToModel();
            if (o != null)
                ci.InformacoesSocioDemograficas1 = await o;
            ci.statusTermoRecusaCadastroIndividualAtencaoBasica = statusTermoRecusaCadastroIndividualAtencaoBasica;
            ci.uuidFichaOriginadora = uuidFichaOriginadora;
            ci.SaidaCidadaoCadastro1 = saidaCidadaoCadastro?.ToModel();
            ci.latitude = latitude;
            ci.longitude = longitude;

            return ci;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator CadastroIndividualViewModel(CadastroIndividual model)
        {
            var vm = new CadastroIndividualViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        internal void ApplyModel(CadastroIndividual model)
        {
            if (model == null) return;

            token = model.UnicaLotacaoTransport.token ?? Guid.Empty;
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
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetCadastroIndividualViewModel : CadastroIndividualViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string uuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public UnicaLotacaoTransport headerTransport { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator GetCadastroIndividualViewModel(CadastroIndividual model)
        {
            var vm = new GetCadastroIndividualViewModel
            {
                uuid = model.UnicaLotacaoTransport.OrigemVisita.id_tipo_origem
                != 1 ? model.UnicaLotacaoTransport.cnes + "-" + model.id : null,
                headerTransport = model.UnicaLotacaoTransport
            };

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
        // ReSharper disable once InconsistentNaming
        public int? motivoSaidaCidadao { get; set; }
        /// <summary>
        /// Data de óbito
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public long? dataObito { get; set; }
        /// <summary>
        /// Número da Declaração
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string numeroDO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator SaidaCidadaoCadastroViewModel(SaidaCidadaoCadastro model)
        {
            var vm = new SaidaCidadaoCadastroViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        private void ApplyModel(SaidaCidadaoCadastro model)
        {
            if (model == null) return;

            motivoSaidaCidadao = model.motivoSaidaCidadao;
            dataObito = model.dataObito?.ToUnix();
            numeroDO = model.numeroDO;
        }

        internal SaidaCidadaoCadastro ToModel()
        {
            var scc = DomainContainer.Current.SaidaCidadaoCadastro.Create();

            scc.id = Guid.NewGuid();
            scc.motivoSaidaCidadao = motivoSaidaCidadao;
            scc.dataObito = dataObito?.FromUnix();
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
        // ReSharper disable once InconsistentNaming
        public int? grauInstrucaoCidadao { get; set; }
        /// <summary>
        /// ocupação
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string ocupacaoCodigoCbo2002 { get; set; }
        /// <summary>
        /// orientação sexual
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int? orientacaoSexualCidadao { get; set; }
        /// <summary>
        /// comunidade
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string povoComunidadeTradicional { get; set; }
        /// <summary>
        /// parentesco
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int? relacaoParentescoCidadao { get; set; }
        /// <summary>
        /// situação / trabalho
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int? situacaoMercadoTrabalhoCidadao { get; set; }
        /// <summary>
        /// Deseja informar a orientação sexual
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusDesejaInformarOrientacaoSexual { get; set; }
        /// <summary>
        /// frequenta benzedeira
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusFrequentaBenzedeira { get; set; }
        /// <summary>
        /// frequenta escola
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusFrequentaEscola { get; set; }
        /// <summary>
        /// membro de comunidade
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusMembroPovoComunidadeTradicional { get; set; }
        /// <summary>
        /// participa do grupo comunitário
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusParticipaGrupoComunitario { get; set; }
        /// <summary>
        /// possui plano de saude privado
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusPossuiPlanoSaudePrivado { get; set; }
        /// <summary>
        /// tem alguma deficiencia
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusTemAlgumaDeficiencia { get; set; }
        /// <summary>
        /// identidade de gênero
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int? identidadeGeneroCidadao { get; set; }
        /// <summary>
        /// deseja informar a identidade de genero
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusDesejaInformarIdentidadeGenero { get; set; }
        /// <summary>
        /// Lista de deficiências do cidadão
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public List<int> deficienciasCidadao { get; set; } = new List<int>();
        /// <summary>
        /// Lista de tipo de responsabilidade por crianças
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public List<int> responsavelPorCrianca { get; set; } = new List<int>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator InformacoesSocioDemograficasViewModel(InformacoesSocioDemograficas model)
        {
            var vm = new InformacoesSocioDemograficasViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        private void ApplyModel(InformacoesSocioDemograficas model)
        {
            if (model == null) return;

            deficienciasCidadao.AddRange(model.DeficienciasCidadao.Select(d => d.id_tp_deficiencia_cidadao));

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
            responsavelPorCrianca.AddRange(model.ResponsavelPorCrianca.Select(d => d.id_tp_crianca));
        }

        internal async Task<InformacoesSocioDemograficas> ToModel()
        {
            var isd = DomainContainer.Current.InformacoesSocioDemograficas.Create();
            isd.id = Guid.NewGuid();

            // ReSharper disable once InconsistentNaming
            foreach (var _dc in deficienciasCidadao)
            {
                TP_Deficiencia dc;
                if ((dc = await DomainContainer.Current.TP_Deficiencia.FirstOrDefaultAsync(y => y.codigo == _dc)) ==
                    null) continue;
                var dcs = DomainContainer.Current.DeficienciasCidadao.Create();
                dcs.id_tp_deficiencia_cidadao = dc.codigo;
                dcs.InformacoesSocioDemograficas = isd;

                isd.DeficienciasCidadao.Add(dcs);
                DomainContainer.Current.DeficienciasCidadao.Add(dcs);
            }

            // ReSharper disable once InconsistentNaming
            foreach (var _cr in responsavelPorCrianca)
            {
                TP_Crianca cr;
                if ((cr = await DomainContainer.Current.TP_Crianca.FirstOrDefaultAsync(y => y.codigo == _cr)) ==
                    null) continue;
                var crs = DomainContainer.Current.ResponsavelPorCrianca.Create();
                crs.InformacoesSocioDemograficas = isd;
                crs.id_tp_crianca = cr.codigo;

                isd.ResponsavelPorCrianca.Add(crs);
                DomainContainer.Current.ResponsavelPorCrianca.Add(crs);
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
        /// 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public Guid id { get; set; }
        /// <summary>
        /// nome social
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string nomeSocial { get; set; }
        /// <summary>
        /// codigo do municipio de nascimento
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string codigoIbgeMunicipioNascimento { get; set; }
        /// <summary>
        /// data de nascimento
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public long dataNascimentoCidadao { get; set; }
        /// <summary>
        /// desconhece o nome da mãe
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool desconheceNomeMae { get; set; }
        /// <summary>
        /// email do cidadão
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string emailCidadao { get; set; }
        /// <summary>
        /// nacionalidade do cidadão
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int nacionalidadeCidadao { get; set; }
        /// <summary>
        /// nome do cidadão
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string nomeCidadao { get; set; }
        /// <summary>
        /// nome da mãe do cidadão
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string nomeMaeCidadao { get; set; }
        /// <summary>
        /// CNS do cidadão
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string cnsCidadao { get; set; }
        /// <summary>
        /// CNS do responsável familiar
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string cnsResponsavelFamiliar { get; set; }
        /// <summary>
        /// telefone celular
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string telefoneCelular { get; set; }
        /// <summary>
        /// NIS/PIS/PASEP
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string numeroNisPisPasep { get; set; }
        /// <summary>
        /// País de nascimento
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int? paisNascimento { get; set; }
        /// <summary>
        /// Raça/Cor
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int racaCorCidadao { get; set; }
        /// <summary>
        /// Sexo
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int sexoCidadao { get; set; }
        /// <summary>
        /// É responsável
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusEhResponsavel { get; set; }
        /// <summary>
        /// Etnia
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int? etnia { get; set; }
        /// <summary>
        /// Nome do Pai
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string nomePaiCidadao { get; set; }
        /// <summary>
        /// Desconhece o nome do Pai
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool desconheceNomePai { get; set; }
        /// <summary>
        /// Data da naturalização
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public long? dtNaturalizacao { get; set; }
        /// <summary>
        /// Portaria da Naturalização
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string portariaNaturalizacao { get; set; }
        /// <summary>
        /// Data de entrada no Brasil
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public long? dtEntradaBrasil { get; set; }
        /// <summary>
        /// Microárea
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string microarea { get; set; }
        /// <summary>
        /// Fora de área
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool stForaArea { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator IdentificacaoUsuarioCidadaoViewModel(IdentificacaoUsuarioCidadao model)
        {
            var vm = new IdentificacaoUsuarioCidadaoViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator IdentificacaoUsuarioCidadaoViewModel(VW_IdentificacaoUsuarioCidadao model)
        {
            var vm = new IdentificacaoUsuarioCidadaoViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        private void ApplyModel(VW_IdentificacaoUsuarioCidadao model)
        {
            if (model == null) return;

            id = model.id;
            nomeSocial = model.nomeSocial;
            codigoIbgeMunicipioNascimento = model.codigoIbgeMunicipioNascimento;
            dataNascimentoCidadao = model.dataNascimentoCidadao.ToUnix();
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
            dtNaturalizacao = model.dtNaturalizacao?.ToUnix();
            portariaNaturalizacao = model.portariaNaturalizacao;
            dtEntradaBrasil = model.dtEntradaBrasil?.ToUnix();
            microarea = model.microarea;
            stForaArea = model.stForaArea;
        }

        private void ApplyModel(IdentificacaoUsuarioCidadao model)
        {
            if (model == null) return;

            id = model.id;
            nomeSocial = model.nomeSocial;
            codigoIbgeMunicipioNascimento = model.codigoIbgeMunicipioNascimento;
            dataNascimentoCidadao = model.dataNascimentoCidadao.ToUnix();
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
            dtNaturalizacao = model.dtNaturalizacao?.ToUnix();
            portariaNaturalizacao = model.portariaNaturalizacao;
            dtEntradaBrasil = model.dtEntradaBrasil?.ToUnix();
            microarea = model.microarea;
            stForaArea = model.stForaArea;
        }

        internal IdentificacaoUsuarioCidadao ToModel()
        {
            var iuc = DomainContainer.Current.IdentificacaoUsuarioCidadao.Create();

            iuc.id = Guid.NewGuid();
            iuc.nomeSocial = nomeSocial;
            iuc.codigoIbgeMunicipioNascimento = codigoIbgeMunicipioNascimento;
            iuc.dataNascimentoCidadao = dataNascimentoCidadao.FromUnix();
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
            iuc.dtNaturalizacao = dtNaturalizacao?.FromUnix();
            iuc.portariaNaturalizacao = portariaNaturalizacao;
            iuc.dtEntradaBrasil = dtEntradaBrasil?.FromUnix();
            iuc.microarea = microarea;
            iuc.stForaArea = stForaArea;

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
        // ReSharper disable once InconsistentNaming
        public string grauParentescoFamiliarFrequentado { get; set; }
        /// <summary>
        /// Outra instituição
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string outraInstituicaoQueAcompanha { get; set; }
        /// <summary>
        /// Quantidade de refeições diária
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int? quantidadeAlimentacoesAoDiaSituacaoRua { get; set; }
        /// <summary>
        /// Acompanhado por outra instituição
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusAcompanhadoPorOutraInstituicao { get; set; }
        /// <summary>
        /// Possui referência familiar
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusPossuiReferenciaFamiliar { get; set; }
        /// <summary>
        /// Recebe beneficio
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusRecebeBeneficio { get; set; }
        /// <summary>
        /// Situação de Rua
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusSituacaoRua { get; set; }
        /// <summary>
        /// Tem acesso à higiene pessoa
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusTemAcessoHigienePessoalSituacaoRua { get; set; }
        /// <summary>
        /// Visita familiar frequentemente
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusVisitaFamiliarFrequentemente { get; set; }
        /// <summary>
        /// Tempo da situação de rua
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int? tempoSituacaoRua { get; set; }
        /// <summary>
        /// Lista de higiene pessoal
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public List<int> higienePessoalSituacaoRua { get; set; } = new List<int>();
        /// <summary>
        /// Lista de origem de alimentos
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public List<int> origemAlimentoSituacaoRua { get; set; } = new List<int>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator EmSituacaoDeRuaViewModel(EmSituacaoDeRua model)
        {
            var vm = new EmSituacaoDeRuaViewModel();

            vm.ApplyModel(model);

            return vm;
        }

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

            higienePessoalSituacaoRua.AddRange(model.HigienePessoalSituacaoRua.Select(h => h.codigo_higiene_pessoal));
            origemAlimentoSituacaoRua.AddRange(model.OrigemAlimentoSituacaoRua.Select(o => o.id_tp_origem_alimento));
        }

        internal async Task<EmSituacaoDeRua> ToModel()
        {
            var esdr = DomainContainer.Current.EmSituacaoDeRua.Create();

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

            foreach (var hpsr in higienePessoalSituacaoRua)
            {
                TP_Higiene_Pessoal hpr;
                if ((hpr = await DomainContainer.Current.TP_Higiene_Pessoal
                        .FirstOrDefaultAsync(y => y.codigo == hpsr)) == null) continue;
                var dcs = DomainContainer.Current.HigienePessoalSituacaoRua.Create();
                dcs.codigo_higiene_pessoal = hpr.codigo;
                dcs.EmSituacaoDeRua = esdr;

                esdr.HigienePessoalSituacaoRua.Add(dcs);
                DomainContainer.Current.HigienePessoalSituacaoRua.Add(dcs);
            }

            // ReSharper disable once InconsistentNaming
            foreach (var _oasr in origemAlimentoSituacaoRua)
            {
                TP_Origem_Alimentacao oasr;
                if ((oasr = await DomainContainer.Current.TP_Origem_Alimentacao.FirstOrDefaultAsync(
                        y => y.codigo == _oasr)) == null) continue;
                var dcs = DomainContainer.Current.OrigemAlimentoSituacaoRua.Create();
                dcs.id_tp_origem_alimento = oasr.codigo;
                dcs.EmSituacaoDeRua = esdr;

                esdr.OrigemAlimentoSituacaoRua.Add(dcs);
                DomainContainer.Current.OrigemAlimentoSituacaoRua.Add(dcs);
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
        // ReSharper disable once InconsistentNaming
        public string descricaoCausaInternacaoEm12Meses { get; set; }
        /// <summary>
        /// Descrição 1
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string descricaoOutraCondicao1 { get; set; }
        /// <summary>
        /// Descrição 2
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string descricaoOutraCondicao2 { get; set; }
        /// <summary>
        /// Descrição 3
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string descricaoOutraCondicao3 { get; set; }
        /// <summary>
        /// Plantas medicinais usadas
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string descricaoPlantasMedicinaisUsadas { get; set; }
        /// <summary>
        /// Maternidade
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string maternidadeDeReferencia { get; set; }
        /// <summary>
        /// Situação do Peso
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int? situacaoPeso { get; set; }
        /// <summary>
        /// É dependente de álcool
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusEhDependenteAlcool { get; set; }
        /// <summary>
        /// É dependente de outras drogas
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusEhDependenteOutrasDrogas { get; set; }
        /// <summary>
        /// É fumante
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusEhFumante { get; set; }
        /// <summary>
        /// É gestante
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusEhGestante { get; set; }
        /// <summary>
        /// Está acamado
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusEstaAcamado { get; set; }
        /// <summary>
        /// Está domiciliado
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusEstaDomiciliado { get; set; }
        /// <summary>
        /// Tem diabetes
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusTemDiabetes { get; set; }
        /// <summary>
        /// Tem doença respiratória
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusTemDoencaRespiratoria { get; set; }
        /// <summary>
        /// Tem hanseníase
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusTemHanseniase { get; set; }
        /// <summary>
        /// Tem hipertensão
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusTemHipertensaoArterial { get; set; }
        /// <summary>
        /// Tem/Teve câncer
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusTemTeveCancer { get; set; }
        /// <summary>
        /// Tem Doença renal
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusTemTeveDoencasRins { get; set; }
        /// <summary>
        /// Tem Tuberculose
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusTemTuberculose { get; set; }
        /// <summary>
        /// Teve AVC/Derrame
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusTeveAvcDerrame { get; set; }
        /// <summary>
        /// Teve/Tem doenca cardiaca
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusTeveDoencaCardiaca { get; set; }
        /// <summary>
        /// Teve/tem infarto
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusTeveInfarto { get; set; }
        /// <summary>
        /// Esteve/Está internado nos últimos 12 meses
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusTeveInternadoem12Meses { get; set; }
        /// <summary>
        /// Outras práticas integrativas / complementares
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusUsaOutrasPraticasIntegrativasOuComplementares { get; set; }
        /// <summary>
        /// Usa plantas medicionais
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusUsaPlantasMedicinais { get; set; }
        /// <summary>
        /// Diagnóstico mental
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusDiagnosticoMental { get; set; }
        /// <summary>
        /// Lista de doenças cardíacas
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public List<int> doencaCardiaca { get; set; } = new List<int>();
        /// <summary>
        /// Lista de doenças respiratórias
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public List<int> doencaRespiratoria { get; set; } = new List<int>();
        /// <summary>
        /// Lista de doenças renais
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public List<int> doencaRins { get; set; } = new List<int>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator CondicoesDeSaudeViewModel(CondicoesDeSaude model)
        {
            var vm = new CondicoesDeSaudeViewModel();

            vm.ApplyModel(model);

            return vm;
        }

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

            doencaCardiaca.AddRange(model.DoencaCardiaca.Select(d => d.id_tp_doenca_cariaca));
            doencaRespiratoria.AddRange(model.DoencaRespiratoria.Select(d => d.id_tp_doenca_respiratoria));
            doencaRins.AddRange(model.DoencaRins.Select(x => x.id_tp_doenca_rins));
        }

        internal async Task<CondicoesDeSaude> ToModel()
        {
            var cds = DomainContainer.Current.CondicoesDeSaude.Create();

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

            // ReSharper disable once InconsistentNaming
            foreach (var _dcs in doencaCardiaca)
            {
                TP_Doenca_Cardiaca dc;
                if ((dc = await DomainContainer.Current.TP_Doenca_Cardiaca
                        .FirstOrDefaultAsync(y => y.codigo == _dcs)) == null) continue;
                var dcs = DomainContainer.Current.DoencaCardiaca.Create();
                dcs.id_tp_doenca_cariaca = dc.codigo;
                dcs.CondicoesDeSaude = cds;

                cds.DoencaCardiaca.Add(dcs);
                DomainContainer.Current.DoencaCardiaca.Add(dcs);
            }

            foreach (var drs in doencaRespiratoria)
            {
                TP_Doenca_Respiratoria dr;
                if ((dr = await DomainContainer.Current.TP_Doenca_Respiratoria
                        .FirstOrDefaultAsync(y => y.codigo == drs)) == null) continue;
                var dcs = DomainContainer.Current.DoencaRespiratoria.Create();
                dcs.id_tp_doenca_respiratoria = dr.codigo;
                dcs.CondicoesDeSaude = cds;

                cds.DoencaRespiratoria.Add(dcs);
                DomainContainer.Current.DoencaRespiratoria.Add(dcs);
            }

            foreach (var drr in doencaRins)
            {
                TP_Doenca_Renal dcr;
                if ((dcr = await DomainContainer.Current.TP_Doenca_Renal.FirstOrDefaultAsync(y => y.codigo == drr)) ==
                    null) continue;
                var dcs = DomainContainer.Current.DoencaRins.Create();
                dcs.id_tp_doenca_rins = dcr.codigo;
                dcs.CondicoesDeSaude = cds;

                cds.DoencaRins.Add(dcs);
                DomainContainer.Current.DoencaRins.Add(dcs);
            }

            return cds;
        }
    }
}