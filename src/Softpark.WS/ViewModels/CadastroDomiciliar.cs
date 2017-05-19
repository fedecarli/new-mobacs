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
    public class CadastroDomiciliarViewModelCollection : List<GetCadastroDomiciliarViewModel>
    {
        public CadastroDomiciliarViewModelCollection() { }

        public CadastroDomiciliarViewModelCollection(IEnumerable<GetCadastroDomiciliarViewModel> models) : base(models) { }

        public CadastroDomiciliarViewModelCollection(CadastroDomiciliar[] models)
        {
            AddRange(models);
        }

        public static implicit operator CadastroDomiciliarViewModelCollection(CadastroDomiciliar[] models)
        {
            return new CadastroDomiciliarViewModelCollection(models);
        }

        public static implicit operator CadastroDomiciliarViewModelCollection(GetCadastroDomiciliarViewModel[] models)
        {
            return new CadastroDomiciliarViewModelCollection(models);
        }

        public void AddRange(CadastroDomiciliar[] models)
        {
            foreach (var model in models)
            {
                Add(model);
            }
        }
    }

    public class PrimitiveCadastroDomiciliarViewModel
    {
        public virtual Guid? token { get; set; } = null;
        public CondicaoMoradiaViewModel condicaoMoradia { get; set; } = null;
        public EnderecoLocalPermanenciaViewModel enderecoLocalPermanencia { get; set; } = null;
        public bool fichaAtualizada { get; set; }
        public int? quantosAnimaisNoDomicilio { get; set; } = null;
        public bool stAnimaisNoDomicilio { get; set; }
        public bool statusTermoRecusa { get; set; }
        public Guid? uuidFichaOriginadora { get; set; } = null;
        public int tipoDeImovel { get; set; }
        public InstituicaoPermanenciaViewModel instituicaoPermanencia { get; set; } = null;

        public int[] animalNoDomicilio { get; set; } = new int[0];
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
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<CadastroDomiciliar> ToModel()
        {
            var dc = DomainContainer.Current.CadastroDomiciliar.Create();

            dc.id = Guid.NewGuid();
            dc.CondicaoMoradia1 = condicaoMoradia?.ToModel();
            dc.EnderecoLocalPermanencia1 = enderecoLocalPermanencia?.ToModel();
            dc.fichaAtualizada = fichaAtualizada;
            dc.quantosAnimaisNoDomicilio = quantosAnimaisNoDomicilio;
            dc.stAnimaisNoDomicilio = stAnimaisNoDomicilio;
            dc.statusTermoRecusa = statusTermoRecusa;
            dc.tpCdsOrigem = 3;
            dc.uuidFichaOriginadora = uuidFichaOriginadora;
            dc.tipoDeImovel = tipoDeImovel;
            dc.InstituicaoPermanencia1 = instituicaoPermanencia?.ToModel();
            dc.latitude = latitude;
            dc.longitude = longitude;

            TP_Animais an;
            foreach (var a in animalNoDomicilio)
                if ((an = await DomainContainer.Current.TP_Animais.FirstOrDefaultAsync(x => x.codigo == a)) != null)
                {
                    var animal = DomainContainer.Current.AnimalNoDomicilio.Create();
                    animal.id_tp_animal = an.codigo;
                    animal.CadastroDomiciliar = dc;
                    dc.AnimalNoDomicilio.Add(animal);
                    DomainContainer.Current.AnimalNoDomicilio.Add(animal);
                }

            foreach (var fr in familiaRow)
            {
                dc.FamiliaRow.Add(fr.ToModel());
            }

            return dc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator PrimitiveCadastroDomiciliarViewModel(CadastroDomiciliar model)
        {
            var vm = new PrimitiveCadastroDomiciliarViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        internal void ApplyModel(CadastroDomiciliar model)
        {
            if (model == null) return;

            var db = DomainContainer.Current;

            token = model.UnicaLotacaoTransport.token ?? Guid.Empty;
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

            animalNoDomicilio = model.AnimalNoDomicilio.Select(a => a.id_tp_animal).ToArray();

            FamiliaRowViewModelCollection rows = model.FamiliaRow.ToArray();

            familiaRow = rows.ToArray();
        }
    }

    public class CadastroDomiciliarViewModel : PrimitiveCadastroDomiciliarViewModel
    {
        [Required]
        public new Guid token { get { return base.token ?? Guid.Empty; } set { base.token = value; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator CadastroDomiciliarViewModel(CadastroDomiciliar model)
        {
            var vm = new CadastroDomiciliarViewModel();

            vm.ApplyModel(model);

            return vm;
        }
    }

    public class GetCadastroDomiciliarViewModel : CadastroDomiciliarViewModel
    {
        public Guid uuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator GetCadastroDomiciliarViewModel(CadastroDomiciliar model)
        {
            var vm = new GetCadastroDomiciliarViewModel { uuid = model.id };

            vm.ApplyModel(model);

            return vm;
        }

    }

    public class FamiliaRowViewModelCollection : List<FamiliaRowViewModel>
    {
        public static implicit operator FamiliaRowViewModelCollection(FamiliaRow[] models)
        {
            var collection = new FamiliaRowViewModelCollection();
            collection.AddRange(models);
            return collection;
        }

        public void AddRange(FamiliaRow[] models)
        {
            foreach (var model in models)
            {
                Add(model);
            }
        }
    }

    public class FamiliaRowViewModel
    {
        public long? dataNascimentoResponsavel { get; set; } = null;
        public string numeroCnsResponsavel { get; set; } = null;
        public int? numeroMembrosFamilia { get; set; } = null;
        public string numeroProntuario { get; set; } = null;
        public int? rendaFamiliar { get; set; } = null;
        public long? resideDesde { get; set; } = null;
        public bool stMudanca { get; set; }

        public FamiliaRow ToModel()
        {
            var fr = DomainContainer.Current.FamiliaRow.Create();

            fr.id = Guid.NewGuid();
            fr.dataNascimentoResponsavel = dataNascimentoResponsavel?.FromUnix();
            fr.numeroCnsResponsavel = numeroCnsResponsavel;
            fr.numeroMembrosFamilia = numeroMembrosFamilia;
            fr.numeroProntuario = numeroProntuario;
            fr.rendaFamiliar = rendaFamiliar;
            fr.resideDesde = resideDesde?.FromUnix();
            fr.stMudanca = stMudanca;

            DomainContainer.Current.FamiliaRow.Add(fr);

            return fr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator FamiliaRowViewModel(FamiliaRow model)
        {
            var vm = new FamiliaRowViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        private void ApplyModel(FamiliaRow model)
        {
            if (model == null) return;

            dataNascimentoResponsavel = model.dataNascimentoResponsavel?.ToUnix();
            numeroCnsResponsavel = model.numeroCnsResponsavel;
            numeroMembrosFamilia = model.numeroMembrosFamilia;
            numeroProntuario = model.numeroProntuario;
            rendaFamiliar = model.rendaFamiliar;
            resideDesde = model.resideDesde?.ToUnix();
            stMudanca = model.stMudanca;
        }
    }

    public class InstituicaoPermanenciaViewModel
    {
        public string nomeInstituicaoPermanencia { get; set; } = null;
        public bool stOutrosProfissionaisVinculados { get; set; }
        public string nomeResponsavelTecnico { get; set; } = null;
        public string cnsResponsavelTecnico { get; set; } = null;
        public string cargoInstituicao { get; set; } = null;
        public string telefoneResponsavelTecnico { get; set; } = null;

        internal InstituicaoPermanencia ToModel()
        {
            var ip = DomainContainer.Current.InstituicaoPermanencia.Create();

            ip.id = Guid.NewGuid();
            ip.nomeInstituicaoPermanencia = nomeInstituicaoPermanencia;
            ip.stOutrosProfissionaisVinculados = stOutrosProfissionaisVinculados;
            ip.nomeResponsavelTecnico = nomeResponsavelTecnico;
            ip.cnsResponsavelTecnico = cnsResponsavelTecnico;
            ip.cargoInstituicao = cargoInstituicao;
            ip.telefoneResponsavelTecnico = telefoneResponsavelTecnico;

            DomainContainer.Current.InstituicaoPermanencia.Add(ip);

            return ip;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator InstituicaoPermanenciaViewModel(InstituicaoPermanencia model)
        {
            var vm = new InstituicaoPermanenciaViewModel();

            vm.ApplyModel(model);

            return vm;
        }

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

    public class EnderecoLocalPermanenciaViewModel
    {
        public string bairro { get; set; } = null;
        public string cep { get; set; } = null;
        public string codigoIbgeMunicipio { get; set; } = null;
        public string complemento { get; set; } = null;
        public string nomeLogradouro { get; set; } = null;
        public string numero { get; set; } = null;
        public string numeroDneUf { get; set; } = null;
        public string telefoneContato { get; set; } = null;
        public string telelefoneResidencia { get; set; } = null;
        public string tipoLogradouroNumeroDne { get; set; } = null;
        public bool stSemNumero { get; set; }
        public string pontoReferencia { get; set; } = null;
        public string microarea { get; set; } = null;
        public bool stForaArea { get; set; }

        internal EnderecoLocalPermanencia ToModel()
        {
            var elp = DomainContainer.Current.EnderecoLocalPermanencia.Create();

            elp.id = Guid.NewGuid();
            elp.bairro = bairro;
            elp.cep = cep;
            elp.codigoIbgeMunicipio = codigoIbgeMunicipio;
            elp.complemento = complemento;
            elp.nomeLogradouro = nomeLogradouro;
            elp.numero = numero;
            elp.numeroDneUf = numeroDneUf;
            elp.telefoneContato = telefoneContato;
            elp.telelefoneResidencia = telelefoneResidencia;
            elp.tipoLogradouroNumeroDne = tipoLogradouroNumeroDne;
            elp.stSemNumero = stSemNumero;
            elp.pontoReferencia = pontoReferencia;
            elp.microarea = microarea;
            elp.stForaArea = stForaArea;

            DomainContainer.Current.EnderecoLocalPermanencia.Add(elp);

            return elp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator EnderecoLocalPermanenciaViewModel(EnderecoLocalPermanencia model)
        {
            var vm = new EnderecoLocalPermanenciaViewModel();

            vm.ApplyModel(model);

            return vm;
        }

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
            telelefoneResidencia = model.telelefoneResidencia;
            tipoLogradouroNumeroDne = model.tipoLogradouroNumeroDne;
            stSemNumero = model.stSemNumero;
            pontoReferencia = model.pontoReferencia;
            microarea = model.microarea;
            stForaArea = model.stForaArea;
        }
    }

    public class CondicaoMoradiaViewModel
    {
        public int? abastecimentoAgua { get; set; } = null;
        public int? areaProducaoRural { get; set; } = null;
        public int? destinoLixo { get; set; } = null;
        public int? formaEscoamentoBanheiro { get; set; } = null;
        public int? localizacao { get; set; } = null;
        public int? materialPredominanteParedesExtDomicilio { get; set; } = null;
        public int? nuComodos { get; set; } = null;
        public int? nuMoradores { get; set; } = null;
        public int? situacaoMoradiaPosseTerra { get; set; } = null;
        public bool stDisponibilidadeEnergiaEletrica { get; set; }
        public int? tipoAcessoDomicilio { get; set; } = null;
        public int? tipoDomicilio { get; set; } = null;
        public int? aguaConsumoDomicilio { get; set; } = null;

        internal CondicaoMoradia ToModel()
        {
            var cm = DomainContainer.Current.CondicaoMoradia.Create();

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

            DomainContainer.Current.CondicaoMoradia.Add(cm);

            return cm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator CondicaoMoradiaViewModel(CondicaoMoradia model)
        {
            var vm = new CondicaoMoradiaViewModel();

            vm.ApplyModel(model);

            return vm;
        }

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
