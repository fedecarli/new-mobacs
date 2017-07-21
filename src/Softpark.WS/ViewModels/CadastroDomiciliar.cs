using Newtonsoft.Json;
using Softpark.Infrastructure.Extras;
using Softpark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Softpark.WS.ViewModels
{
    /// <summary>
    /// ViewModel Collection de Cadastro Domiciliar
    /// </summary>
    public class CadastroDomiciliarViewModelCollection : List<GetCadastroDomiciliarViewModel>
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public CadastroDomiciliarViewModelCollection() { }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public CadastroDomiciliarViewModelCollection(IEnumerable<GetCadastroDomiciliarViewModel> models) : base(models) { }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public CadastroDomiciliarViewModelCollection(CadastroDomiciliar[] models)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            AddRange(models);
        }

        /// <summary>
        /// Conversor - DataBind
        /// </summary>
        /// <param name="models"></param>
        public static implicit operator CadastroDomiciliarViewModelCollection(CadastroDomiciliar[] models)
        {
            return new CadastroDomiciliarViewModelCollection(models);
        }

        /// <summary>
        /// Conversor - DataBind
        /// </summary>
        /// <param name="models"></param>
        public static implicit operator CadastroDomiciliarViewModelCollection(GetCadastroDomiciliarViewModel[] models)
        {
            return new CadastroDomiciliarViewModelCollection(models);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void AddRange(CadastroDomiciliar[] models)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            foreach (var model in models)
            {
                Add(model);
            }
        }
    }

    /// <summary>
    /// ViewModel de dados promitivos
    /// </summary>
    public class PrimitiveCadastroDomiciliarViewModel
    {
#pragma warning disable IDE1006 // Naming Styles
        /// <summary>
        /// Token
        /// </summary>
        public virtual Guid? token { get; set; } = null;
        /// <summary>
        /// Condição de Moradia
        /// </summary>
        public CondicaoMoradiaViewModel condicaoMoradia { get; set; } = null;
        /// <summary>
        /// Endereço
        /// </summary>
        public EnderecoLocalPermanenciaViewModel enderecoLocalPermanencia { get; set; } = null;
        /// <summary>
        /// É uma atualização?
        /// </summary>
        public bool fichaAtualizada { get; set; }
        /// <summary>
        /// Quantidade de animais no domicílio
        /// </summary>
        public int? quantosAnimaisNoDomicilio { get; set; } = null;
        /// <summary>
        /// Há animais no domicílio?
        /// </summary>
        public bool stAnimaisNoDomicilio { get; set; }
        /// <summary>
        /// Responsável recusou-se à fornecer informações?
        /// </summary>
        public bool statusTermoRecusa { get; set; }
        /// <summary>
        /// Se for atualização, qual o ID de origem?
        /// </summary>
        public Guid? uuidFichaOriginadora { get; set; } = null;
        /// <summary>
        /// Tipo de Imóvel
        /// </summary>
        public int tipoDeImovel { get; set; }
        /// <summary>
        /// Instituição de permanência
        /// </summary>
        public InstituicaoPermanenciaViewModel instituicaoPermanencia { get; set; } = null;
        /// <summary>
        /// Justificativa
        /// </summary>
        public string Justificativa { get; set; } = null;

        /// <summary>
        /// Data de registro da ficha no app
        /// </summary>
        public DateTime? DataRegistro { get; set; } = null;

        /// <summary>
        /// Quais animais?
        /// </summary>
        public int[] animalNoDomicilio { get; set; } = new int[0];

        /// <summary>
        /// Famnilias
        /// </summary>
        public FamiliaRowViewModel[] familiaRow { get; set; } = new FamiliaRowViewModel[0];

        /// <summary>
        /// Latitude de demarcação do início do cadastro
        /// </summary>
        public string latitude { get; set; } = null;

        /// <summary>
        /// Latitude de demarcação do início do cadastro
        /// </summary>
        public string longitude { get; set; } = null;

        /// <summary>
        /// DataBind
        /// </summary>
        /// <returns></returns>
        public async Task<CadastroDomiciliar> ToModel(DomainContainer domain)
        {
            var dc = domain.CadastroDomiciliar.Create();

            dc.id = Guid.NewGuid();
            dc.CondicaoMoradia1 = condicaoMoradia?.ToModel(domain);
            dc.EnderecoLocalPermanencia1 = enderecoLocalPermanencia?.ToModel(domain);
            dc.fichaAtualizada = fichaAtualizada;
            dc.quantosAnimaisNoDomicilio = quantosAnimaisNoDomicilio;
            dc.stAnimaisNoDomicilio = stAnimaisNoDomicilio;
            dc.statusTermoRecusa = statusTermoRecusa;
            dc.tpCdsOrigem = 3;
            dc.uuidFichaOriginadora = uuidFichaOriginadora;
            dc.tipoDeImovel = tipoDeImovel;
            dc.InstituicaoPermanencia1 = instituicaoPermanencia?.ToModel(domain);
            dc.latitude = latitude;
            dc.longitude = longitude;
            dc.Justificativa = Justificativa;
            dc.DataRegistro = DataRegistro;

            TP_Animais an;
            foreach (var a in animalNoDomicilio)
                if ((an = await domain.TP_Animais.FirstOrDefaultAsync(x => x.codigo == a)) != null)
                {
                    var animal = domain.AnimalNoDomicilio.Create();
                    animal.id_tp_animal = an.codigo;
                    animal.CadastroDomiciliar = dc;
                    dc.AnimalNoDomicilio.Add(animal);
                    domain.AnimalNoDomicilio.Add(animal);
                }

            foreach (var fr in familiaRow)
            {
                dc.FamiliaRow.Add(fr.ToModel(domain));
            }

            return dc;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator PrimitiveCadastroDomiciliarViewModel(CadastroDomiciliar model)
        {
            var vm = new PrimitiveCadastroDomiciliarViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        internal void ApplyModel(CadastroDomiciliar model)
        {
            if (model == null) return;

            token = model.UnicaLotacaoTransport.token;
            condicaoMoradia = model.CondicaoMoradia1;
            enderecoLocalPermanencia = model.EnderecoLocalPermanencia1;
            fichaAtualizada = model.fichaAtualizada;
            quantosAnimaisNoDomicilio = model.quantosAnimaisNoDomicilio;
            stAnimaisNoDomicilio = model.stAnimaisNoDomicilio;
            statusTermoRecusa = model.statusTermoRecusa;
            uuidFichaOriginadora = model.uuidFichaOriginadora;
            tipoDeImovel = model.tipoDeImovel;
            instituicaoPermanencia = model.InstituicaoPermanencia1;
            latitude = model.latitude;
            longitude = model.longitude;
            Justificativa = model.Justificativa;
            DataRegistro = model.DataRegistro;

            animalNoDomicilio = model.AnimalNoDomicilio.Select(a => a.id_tp_animal).ToArray();

            FamiliaRowViewModelCollection rows = model.FamiliaRow.ToArray();

            familiaRow = rows.ToArray();
        }
#pragma warning restore IDE1006 // Naming Styles
    }

    /// <summary>
    /// ViewModel de Cadastro Domiciliar
    /// </summary>
    public class CadastroDomiciliarViewModel : PrimitiveCadastroDomiciliarViewModel
    {
        /// <inheritdoc />
        [Required]
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required
        public new Guid token { get => base.token ?? Guid.Empty; set => base.token = value; }
#pragma warning restore CS0109 // Member does not hide an inherited member; new keyword is not required
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator CadastroDomiciliarViewModel(CadastroDomiciliar model)
        {
            var vm = new CadastroDomiciliarViewModel();

            vm.ApplyModel(model);

            return vm;
        }
    }

    /// <summary>
    /// ViewModel de consulta de cadastro domiciliar
    /// </summary>
    public class GetCadastroDomiciliarViewModel : CadastroDomiciliarViewModel
    {
#pragma warning disable IDE1006 // Naming Styles
        /// <summary>
        /// ID
        /// </summary>
        public Guid uuid { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator GetCadastroDomiciliarViewModel(CadastroDomiciliar model)
        {
            var vm = new GetCadastroDomiciliarViewModel { uuid = model.id };

            vm.ApplyModel(model);

            return vm;
        }

    }

    /// <summary>
    /// ViewModel collection de familias
    /// </summary>
    public class FamiliaRowViewModelCollection : List<FamiliaRowViewModel>
    {
        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="models"></param>
        public static implicit operator FamiliaRowViewModelCollection(FamiliaRow[] models)
        {
            var collection = new FamiliaRowViewModelCollection();
            collection.AddRange(models);
            return collection;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="models"></param>
        public void AddRange(FamiliaRow[] models)
        {
            foreach (var model in models)
            {
                Add(model);
            }
        }
    }

    /// <summary>
    /// ViewModel de familia
    /// </summary>
    public class FamiliaRowViewModel
    {
        /// <summary>
        /// Data de nascimento do responsável
        /// </summary>
        public DateTime? dataNascimentoResponsavel { get; set; } = null;
        /// <summary>
        /// CNS do responsável
        /// </summary>
        public string numeroCnsResponsavel { get; set; } = null;
        /// <summary>
        /// Quantidade de pessoas na familia
        /// </summary>
        public int? numeroMembrosFamilia { get; set; } = null;
        /// <summary>
        /// Numero do prontuário da familia
        /// </summary>
        public string numeroProntuario { get; set; } = null;
        /// <summary>
        /// Renda total bruta familiar
        /// </summary>
        public int? rendaFamiliar { get; set; } = null;
        /// <summary>
        /// A Familia reside no endereço desde
        /// </summary>
        public DateTime? resideDesde { get; set; } = null;
        /// <summary>
        /// A familia mudou-se?
        /// </summary>
        public bool stMudanca { get; set; }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public FamiliaRow ToModel(DomainContainer domain)
        {
            var fr = domain.FamiliaRow.Create();

            fr.id = Guid.NewGuid();
            fr.dataNascimentoResponsavel = dataNascimentoResponsavel;
            fr.numeroCnsResponsavel = numeroCnsResponsavel;
            fr.numeroMembrosFamilia = numeroMembrosFamilia;
            fr.numeroProntuario = numeroProntuario;
            fr.rendaFamiliar = rendaFamiliar;
            fr.resideDesde = resideDesde;
            fr.stMudanca = stMudanca;

            domain.FamiliaRow.Add(fr);

            return fr;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator FamiliaRowViewModel(FamiliaRow model)
        {
            var vm = new FamiliaRowViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        private void ApplyModel(FamiliaRow model)
        {
            if (model == null) return;

            dataNascimentoResponsavel = model.dataNascimentoResponsavel;
            numeroCnsResponsavel = model.numeroCnsResponsavel;
            numeroMembrosFamilia = model.numeroMembrosFamilia;
            numeroProntuario = model.numeroProntuario;
            rendaFamiliar = model.rendaFamiliar;
            resideDesde = model.resideDesde;
            stMudanca = model.stMudanca;
        }
    }

    /// <summary>
    /// ViewModel da Intituição de Permanência
    /// </summary>
    public class InstituicaoPermanenciaViewModel
    {
        /// <summary>
        /// Noem da instituição
        /// </summary>
        public string nomeInstituicaoPermanencia { get; set; } = null;
        /// <summary>
        /// Possui outros profissionais vinculados?
        /// </summary>
        public bool stOutrosProfissionaisVinculados { get; set; }
        /// <summary>
        /// Nome do responsável técnico da instituição
        /// </summary>
        public string nomeResponsavelTecnico { get; set; } = null;
        /// <summary>
        /// CNS do responsável técnico da instituição
        /// </summary>
        public string cnsResponsavelTecnico { get; set; } = null;
        /// <summary>
        /// Cargo do responsável técnico da instituição
        /// </summary>
        public string cargoInstituicao { get; set; } = null;
        /// <summary>
        /// Telefone do responsável técnico da instituição
        /// </summary>
        public string telefoneResponsavelTecnico { get; set; } = null;

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        internal InstituicaoPermanencia ToModel(DomainContainer domain)
        {
            var ip = domain.InstituicaoPermanencia.Create();

            ip.id = Guid.NewGuid();
            ip.nomeInstituicaoPermanencia = nomeInstituicaoPermanencia;
            ip.stOutrosProfissionaisVinculados = stOutrosProfissionaisVinculados;
            ip.nomeResponsavelTecnico = nomeResponsavelTecnico;
            ip.cnsResponsavelTecnico = cnsResponsavelTecnico;
            ip.cargoInstituicao = cargoInstituicao;
            ip.telefoneResponsavelTecnico = telefoneResponsavelTecnico;

            domain.InstituicaoPermanencia.Add(ip);

            return ip;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator InstituicaoPermanenciaViewModel(InstituicaoPermanencia model)
        {
            var vm = new InstituicaoPermanenciaViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        private void ApplyModel(InstituicaoPermanencia model)
        {
            if (model == null) return;

            nomeInstituicaoPermanencia = model.nomeInstituicaoPermanencia;
            stOutrosProfissionaisVinculados = model.stOutrosProfissionaisVinculados;
            nomeResponsavelTecnico = model.nomeResponsavelTecnico;
            cnsResponsavelTecnico = model.cnsResponsavelTecnico;
            cargoInstituicao = model.cargoInstituicao;
            telefoneResponsavelTecnico = model.telefoneResponsavelTecnico;
        }
    }

    /// <summary>
    /// Endereço do local
    /// </summary>
    public class EnderecoLocalPermanenciaViewModel
    {
        /// <summary>
        /// Bairro
        /// </summary>
        public string bairro { get; set; } = null;
        /// <summary>
        /// CEP
        /// </summary>
        public string cep { get; set; } = null;
        /// <summary>
        /// Municipio
        /// </summary>
        public string codigoIbgeMunicipio { get; set; } = null;
        /// <summary>
        /// Complemento
        /// </summary>
        public string complemento { get; set; } = null;
        /// <summary>
        /// Logradouro
        /// </summary>
        public string nomeLogradouro { get; set; } = null;
        /// <summary>
        /// Número
        /// </summary>
        public string numero { get; set; } = null;
        /// <summary>
        /// UF
        /// </summary>
        public string numeroDneUf { get; set; } = null;
        /// <summary>
        /// Contato
        /// </summary>
        public string telefoneContato { get; set; } = null;
        /// <summary>
        /// Telefone da residencia
        /// </summary>
        /// <deprecated/>
        public string telelefoneResidencia { get; set; } = null;
        /// <summary>
        /// Telefone da residencia
        /// </summary>
        public string telefoneResidencia { get; set; } = null;
        /// <summary>
        /// Tipo do Logradouro
        /// </summary>
        public string tipoLogradouroNumeroDne { get; set; } = null;
        /// <summary>
        /// Residência sem número
        /// </summary>
        public bool stSemNumero { get; set; }
        /// <summary>
        /// Ponto de referência
        /// </summary>
        public string pontoReferencia { get; set; } = null;
        /// <summary>
        /// Microárea
        /// </summary>
        public string microarea { get; set; } = null;
        /// <summary>
        /// Residência fora de área
        /// </summary>
        public bool stForaArea { get; set; }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        internal EnderecoLocalPermanencia ToModel(DomainContainer domain)
        {
            var elp = domain.EnderecoLocalPermanencia.Create();

            elp.id = Guid.NewGuid();
            elp.bairro = bairro;
            elp.cep = cep;
            elp.codigoIbgeMunicipio = codigoIbgeMunicipio;
            elp.complemento = complemento;
            elp.nomeLogradouro = nomeLogradouro;
            elp.numero = numero;
            elp.numeroDneUf = numeroDneUf;
            elp.telefoneContato = telefoneContato;
            elp.telefoneResidencia = telefoneResidencia ?? telelefoneResidencia;
            elp.tipoLogradouroNumeroDne = tipoLogradouroNumeroDne;
            elp.stSemNumero = stSemNumero;
            elp.pontoReferencia = pontoReferencia;
            elp.microarea = microarea;
            elp.stForaArea = stForaArea;

            domain.EnderecoLocalPermanencia.Add(elp);

            return elp;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator EnderecoLocalPermanenciaViewModel(EnderecoLocalPermanencia model)
        {
            var vm = new EnderecoLocalPermanenciaViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        private void ApplyModel(EnderecoLocalPermanencia model)
        {
            if (model == null) return;

            bairro = model.bairro;
            cep = model.cep;
            codigoIbgeMunicipio = model.codigoIbgeMunicipio;
            complemento = model.complemento;
            nomeLogradouro = model.nomeLogradouro;
            numero = model.numero;
            numeroDneUf = model.numeroDneUf;
            telefoneContato = model.telefoneContato;
            telefoneResidencia = telelefoneResidencia = model.telefoneResidencia;
            tipoLogradouroNumeroDne = model.tipoLogradouroNumeroDne;
            stSemNumero = model.stSemNumero;
            pontoReferencia = model.pontoReferencia;
            microarea = model.microarea;
            stForaArea = model.stForaArea;
        }
    }

    /// <summary>
    /// ViewModel de condição de moradia
    /// </summary>
    public class CondicaoMoradiaViewModel
    {
        /// <summary>
        /// Tipo de abasteciomento de água
        /// </summary>
        public int? abastecimentoAgua { get; set; } = null;
        /// <summary>
        /// Tipo de área de produção rural
        /// </summary>
        public int? areaProducaoRural { get; set; } = null;
        /// <summary>
        /// O lixo tem como destino
        /// </summary>
        public int? destinoLixo { get; set; } = null;
        /// <summary>
        /// Forma do escoamento de resíduos do banheiro
        /// </summary>
        public int? formaEscoamentoBanheiro { get; set; } = null;
        /// <summary>
        /// Localização
        /// </summary>
        public int? localizacao { get; set; } = null;
        /// <summary>
        /// As paredes são feitas de
        /// </summary>
        public int? materialPredominanteParedesExtDomicilio { get; set; } = null;
        /// <summary>
        /// Quantidade de cômodos
        /// </summary>
        public int? nuComodos { get; set; } = null;
        /// <summary>
        /// Quantidade de moradores
        /// </summary>
        public int? nuMoradores { get; set; } = null;
        /// <summary>
        /// Situação de posse da moradia
        /// </summary>
        public int? situacaoMoradiaPosseTerra { get; set; } = null;
        /// <summary>
        /// Possui energia elétrica?
        /// </summary>
        public bool stDisponibilidadeEnergiaEletrica { get; set; }
        /// <summary>
        /// Tipo de acesso ao domicilio
        /// </summary>
        public int? tipoAcessoDomicilio { get; set; } = null;
        /// <summary>
        /// Tipo do domicilio
        /// </summary>
        public int? tipoDomicilio { get; set; } = null;
        /// <summary>
        /// Forma de consumo de água
        /// </summary>
        public int? aguaConsumoDomicilio { get; set; } = null;

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        internal CondicaoMoradia ToModel(DomainContainer domain)
        {
            var cm = domain.CondicaoMoradia.Create();

            cm.id = Guid.NewGuid();
            cm.abastecimentoAgua = abastecimentoAgua;
            cm.areaProducaoRural = areaProducaoRural;
            cm.destinoLixo = destinoLixo;
            cm.formaEscoamentoBanheiro = formaEscoamentoBanheiro;
            cm.localizacao = localizacao;
            cm.materialPredominanteParedesExtDomicilio = materialPredominanteParedesExtDomicilio;
            cm.nuComodos = nuComodos;
            cm.nuMoradores = nuMoradores;
            cm.situacaoMoradiaPosseTerra = situacaoMoradiaPosseTerra;
            cm.stDisponibilidadeEnergiaEletrica = stDisponibilidadeEnergiaEletrica;
            cm.tipoAcessoDomicilio = tipoAcessoDomicilio;
            cm.tipoDomicilio = tipoDomicilio;
            cm.aguaConsumoDomicilio = aguaConsumoDomicilio;

            domain.CondicaoMoradia.Add(cm);

            return cm;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator CondicaoMoradiaViewModel(CondicaoMoradia model)
        {
            var vm = new CondicaoMoradiaViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        private void ApplyModel(CondicaoMoradia model)
        {
            if (model == null) return;

            abastecimentoAgua = model.abastecimentoAgua;
            areaProducaoRural = model.areaProducaoRural;
            destinoLixo = model.destinoLixo;
            formaEscoamentoBanheiro = model.formaEscoamentoBanheiro;
            localizacao = model.localizacao;
            materialPredominanteParedesExtDomicilio = model.materialPredominanteParedesExtDomicilio;
            nuComodos = model.nuComodos;
            nuMoradores = model.nuMoradores;
            situacaoMoradiaPosseTerra = model.situacaoMoradiaPosseTerra;
            stDisponibilidadeEnergiaEletrica = model.stDisponibilidadeEnergiaEletrica;
            tipoAcessoDomicilio = model.tipoAcessoDomicilio;
            tipoDomicilio = model.tipoDomicilio;
            aguaConsumoDomicilio = model.aguaConsumoDomicilio;
        }
    }
}
