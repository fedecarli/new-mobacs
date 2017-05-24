﻿using Softpark.Infrastructure.Extras;
using Softpark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Softpark.WS.ViewModels
{
    public class CadastroIndividualViewModelCollection : List<GetCadastroIndividualViewModel>
    {
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

        internal async Task<CadastroIndividual> ToModel(CadastroIndividual ci, Repository rep)
        {
            ci.id = Guid.NewGuid();
            var cds = rep.Create(c => c.CondicoesDeSaude, (c, f) => condicoesDeSaude?.ToModel(f, c, rep));
            var sdr = rep.Create(c => c.EmSituacaoDeRua, (c, f) => emSituacaoDeRua?.ToModel(f, c, rep));
            var iuc = rep.Create(c => c.IdentificacaoUsuarioCidadao, (c, f) => Task.FromResult(identificacaoUsuarioCidadao?.ToModel(f)));
            var isd = rep.Create(c => c.InformacoesSocioDemograficas, (c, f) => informacoesSocioDemograficas?.ToModel(f, c, rep));
            var scc = rep.Create(c => c.SaidaCidadaoCadastro, (c, f) => Task.FromResult(saidaCidadaoCadastro?.ToModel(f)));

            await Task.WhenAll(cds, sdr, isd);

            ci.condicoesDeSaude = (await cds)?.id;
            ci.emSituacaoDeRua = (await sdr)?.id;
            ci.fichaAtualizada = fichaAtualizada;
            ci.identificacaoUsuarioCidadao = (await iuc)?.id;
            ci.informacoesSocioDemograficas = (await isd)?.id;
            ci.statusTermoRecusaCadastroIndividualAtencaoBasica = statusTermoRecusaCadastroIndividualAtencaoBasica;
            ci.uuidFichaOriginadora = uuidFichaOriginadora;
            ci.saidaCidadaoCadastro = (await scc)?.id;

            return ci;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator PrimitiveCadastroIndividualViewModel(CadastroIndividual model)
        {
            var vm = new PrimitiveCadastroIndividualViewModel();

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
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator CadastroIndividualViewModel(CadastroIndividual model)
        {
            var vm = new CadastroIndividualViewModel();

            vm.ApplyModel(model);

            return vm;
        }
    }

    public class GetCadastroIndividualViewModel : CadastroIndividualViewModel
    {
        public Guid uuid { get; set; }

        /// <summary>
        /// 
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
        public long? dataObito { get; set; } = null;
        /// <summary>
        /// Número da Declaração
        /// </summary>
        public string numeroDO { get; set; } = null;

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

        internal SaidaCidadaoCadastro ToModel(SaidaCidadaoCadastro scc)
        {
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

        internal async Task<InformacoesSocioDemograficas> ToModel(InformacoesSocioDemograficas isd, DomainContainer c, Repository rep)
        {
            isd.id = Guid.NewGuid();

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

            var dc = rep.GetModel(_c => _c.TP_Deficiencia.Where(x => deficienciasCidadao.Contains(x.codigo)).ToListAsync(), (l) => l.Select(e =>
            {
                var d = c.DeficienciasCidadao.Create();

                d.id_tp_deficiencia_cidadao = e.codigo;
                d.InformacoesSocioDemograficas = isd;

                isd.DeficienciasCidadao.Add(d);
                c.DeficienciasCidadao.Add(d);

                return d;
            }));

            var cr = rep.GetModel(_c => _c.TP_Crianca.Where(x => responsavelPorCrianca.Contains(x.codigo)).ToListAsync(), (l) => l.Select(e =>
            {
                var d = c.ResponsavelPorCrianca.Create();

                d.InformacoesSocioDemograficas = isd;
                d.id_tp_crianca = e.codigo;

                isd.ResponsavelPorCrianca.Add(d);
                c.ResponsavelPorCrianca.Add(d);

                return d;
            }));

            Task.WaitAll(dc, cr);

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
        public long dataNascimentoCidadao { get; set; }
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
        public long? dtNaturalizacao { get; set; } = null;
        /// <summary>
        /// Portaria da Naturalização
        /// </summary>
        public string portariaNaturalizacao { get; set; } = null;
        /// <summary>
        /// Data de entrada no Brasil
        /// </summary>
        public long? dtEntradaBrasil { get; set; } = null;
        /// <summary>
        /// Microárea
        /// </summary>
        public string microarea { get; set; } = null;
        /// <summary>
        /// Fora de área
        /// </summary>
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
            dataNascimentoCidadao = model.dataNascimentoCidadao?.ToUnix() ?? 0;
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
            dtNaturalizacao = model.dtNaturalizacao?.ToUnix();
            portariaNaturalizacao = model.portariaNaturalizacao;
            dtEntradaBrasil = model.dtEntradaBrasil?.ToUnix();
            microarea = model.microarea;
            stForaArea = model.stForaArea ?? false;
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

        internal IdentificacaoUsuarioCidadao ToModel(IdentificacaoUsuarioCidadao iuc)
        {
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

            higienePessoalSituacaoRua = model.HigienePessoalSituacaoRua.Select(h => h.codigo_higiene_pessoal).ToArray();
            origemAlimentoSituacaoRua = model.OrigemAlimentoSituacaoRua.Select(o => o.id_tp_origem_alimento).ToArray();
        }

        internal async Task<EmSituacaoDeRua> ToModel(EmSituacaoDeRua esdr, DomainContainer c, Repository rep)
        {
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

            var hpr = rep.GetModel(_c => _c.TP_Higiene_Pessoal.Where(x => higienePessoalSituacaoRua.Contains(x.codigo)).ToListAsync(), (doencas) => doencas.Select(dcs =>
            {
                var _dcs = c.HigienePessoalSituacaoRua.Create();

                _dcs.codigo_higiene_pessoal = dcs.codigo;
                _dcs.id_em_situacao_de_rua = esdr.id;

                esdr.HigienePessoalSituacaoRua.Add(_dcs);
                c.HigienePessoalSituacaoRua.Add(_dcs);

                return _dcs;
            }));

            var oasr = rep.GetModel(_c => _c.TP_Origem_Alimentacao.Where(x => origemAlimentoSituacaoRua.Contains(x.codigo)).ToListAsync(), (doencas) => doencas.Select(dcs =>
            {
                var _dcs = c.OrigemAlimentoSituacaoRua.Create();

                _dcs.id_tp_origem_alimento = dcs.codigo;
                _dcs.id_em_situacao_rua = esdr.id;

                esdr.OrigemAlimentoSituacaoRua.Add(_dcs);
                c.OrigemAlimentoSituacaoRua.Add(_dcs);

                return _dcs;
            }));

            await Task.WhenAll(hpr, oasr);

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

            doencaCardiaca = model.DoencaCardiaca.Select(d => d.id_tp_doenca_cariaca).ToArray();
            doencaRespiratoria = model.DoencaRespiratoria.Select(d => d.id_tp_doenca_respiratoria).ToArray();
            doencaRins = model.DoencaRins.Select(x => x.id_tp_doenca_rins).ToArray();
        }

        internal async Task<CondicoesDeSaude> ToModel(CondicoesDeSaude cds, DomainContainer c, Repository rep)
        {
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

            var dc = rep.GetModel((_c) => _c.TP_Doenca_Cardiaca.Where(x => doencaCardiaca.Contains(x.codigo)).ToListAsync(), (doencas) => doencas.Select(dcs =>
            {
                var _dcs = c.DoencaCardiaca.Create();

                _dcs.id_tp_doenca_cariaca = dcs.codigo;
                _dcs.id_condicao_de_saude = cds.id;

                cds.DoencaCardiaca.Add(_dcs);
                c.DoencaCardiaca.Add(_dcs);

                return _dcs;
            }));

            var dr = rep.GetModel(_c => _c.TP_Doenca_Respiratoria.Where(x => doencaRespiratoria.Contains(x.codigo)).ToListAsync(), (doencas) => doencas.Select(dcs =>
            {
                var _dcs = c.DoencaRespiratoria.Create();

                _dcs.id_tp_doenca_respiratoria = dcs.codigo;
                _dcs.id_condicao_de_saude = cds.id;

                cds.DoencaRespiratoria.Add(_dcs);
                c.DoencaRespiratoria.Add(_dcs);

                return _dcs;
            }));

            var dcr = rep.GetModel(_c => _c.TP_Doenca_Renal.Where(x => doencaRins.Contains(x.codigo)).ToListAsync(), (doencas) => doencas.Select(dcs =>
            {
                var _dcs = c.DoencaRins.Create();

                _dcs.id_tp_doenca_rins = dcs.codigo;
                _dcs.id_condicao_de_saude = cds.id;

                cds.DoencaRins.Add(_dcs);
                c.DoencaRins.Add(_dcs);

                return _dcs;
            }));

            await Task.WhenAll(dc, dr, dcr);

            return cds;
        }
    }
}
