﻿using Softpark.Models;
using Softpark.WS.Validators;
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
        public static implicit operator CadastroDomiciliarViewModelCollection(CadastroDomiciliar[] models)
        {
            var collection = new CadastroDomiciliarViewModelCollection();
            collection.AddRange(models);
            return collection;
        }

        public void AddRange(CadastroDomiciliar[] models)
        {
            foreach (var model in models)
            {
                Add(model);
            }
        }
    }

    public class CadastroDomiciliarViewModel
    {
        [Required]
        public Guid token { get; set; }
        public CondicaoMoradiaViewModel condicaoMoradia { get; set; }
        public EnderecoLocalPermanenciaViewModel enderecoLocalPermanencia { get; set; }
        public bool fichaAtualizada { get; set; }
        public int? quantosAnimaisNoDomicilio { get; set; }
        public bool stAnimaisNoDomicilio { get; set; }
        public bool statusTermoRecusa { get; set; }
        public string uuidFichaOriginadora { get; set; }
        [TipoImovelValidation]
        public int tipoDeImovel { get; set; }
        public InstituicaoPermanenciaViewModel instituicaoPermanencia { get; set; }
        
        public List<int> animalNoDomicilio { get; set; } = new List<int>();
        public List<FamiliaRowViewModel> familiaRow { get; set; } = new List<FamiliaRowViewModel>();

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

            TP_Animais an;
            foreach(var a in animalNoDomicilio)
                if((an = await DomainContainer.Current.TP_Animais.FirstOrDefaultAsync(x => x.codigo == a)) != null)
                {
                    var animal = DomainContainer.Current.AnimalNoDomicilio.Create();
                    animal.id_tp_animal = an.codigo;
                    animal.CadastroDomiciliar = dc;
                    dc.AnimalNoDomicilio.Add(animal);
                    DomainContainer.Current.AnimalNoDomicilio.Add(animal);
                }

            foreach(var fr in familiaRow)
            {
                dc.FamiliaRow.Add(fr.ToModel());
            }

            return dc;
        }

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

            animalNoDomicilio.AddRange(model.AnimalNoDomicilio.Select(a => a.id_tp_animal));

            FamiliaRowViewModelCollection rows = model.FamiliaRow.ToArray();

            familiaRow.AddRange(rows);
        }
    }

    public class GetCadastroDomiciliarViewModel : CadastroDomiciliarViewModel
    {
        public string uuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator GetCadastroDomiciliarViewModel(CadastroDomiciliar model)
        {
            var vm = new GetCadastroDomiciliarViewModel { uuid = model.UnicaLotacaoTransport.cnes + "-" + model.id };

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
        public int? dataNascimentoResponsavel { get; set; }
        public string numeroCnsResponsavel { get; set; }
        public int? numeroMembrosFamilia { get; set; }
        public string numeroProntuario { get; set; }
        public int? rendaFamiliar { get; set; }
        public int? resideDesde { get; set; }
        public bool stMudanca { get; set; }

        public FamiliaRow ToModel()
        {
            var fr = DomainContainer.Current.FamiliaRow.Create();

            fr.id = Guid.NewGuid();
            fr.dataNascimentoResponsavel = dataNascimentoResponsavel;
            fr.numeroCnsResponsavel = numeroCnsResponsavel;
            fr.numeroMembrosFamilia = numeroMembrosFamilia;
            fr.numeroProntuario = numeroProntuario;
            fr.rendaFamiliar = rendaFamiliar;
            fr.resideDesde = resideDesde;
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
            
            dataNascimentoResponsavel = model.dataNascimentoResponsavel;
            numeroCnsResponsavel = model.numeroCnsResponsavel;
            numeroMembrosFamilia = model.numeroMembrosFamilia;
            numeroProntuario = model.numeroProntuario;
            rendaFamiliar = model.rendaFamiliar;
            resideDesde = model.resideDesde;
            stMudanca = model.stMudanca;
        }
    }

    public class InstituicaoPermanenciaViewModel
    {
        public string nomeInstituicaoPermanencia { get; set; }
        public bool stOutrosProfissionaisVinculados { get; set; }
        public string nomeResponsavelTecnico { get; set; }
        public string cnsResponsavelTecnico { get; set; }
        public string cargoInstituicao { get; set; }
        public string telefoneResponsavelTecnico { get; set; }

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
        public string bairro { get; set; }
        public string cep { get; set; }
        public string codigoIbgeMunicipio { get; set; }
        public string complemento { get; set; }
        public string nomeLogradouro { get; set; }
        public string numero { get; set; }
        public string numeroDneUf { get; set; }
        public string telefoneContato { get; set; }
        public string telelefoneResidencia { get; set; }
        public string tipoLogradouroNumeroDne { get; set; }
        public bool stSemNumero { get; set; }
        public string pontoReferencia { get; set; }
        public string microarea { get; set; }
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
        public int? abastecimentoAgua { get; set; }
        public int? areaProducaoRural { get; set; }
        public int? destinoLixo { get; set; }
        public int? formaEscoamentoBanheiro { get; set; }
        public int? localizacao { get; set; }
        public int? materialPredominanteParedesExtDomicilio { get; set; }
        public int? nuComodos { get; set; }
        public int? nuMoradores { get; set; }
        public int? situacaoMoradiaPosseTerra { get; set; }
        public bool stDisponibilidadeEnergiaEletrica { get; set; }
        public int? tipoAcessoDomicilio { get; set; }
        public int? tipoDomicilio { get; set; }
        public int? aguaConsumoDomicilio { get; set; }

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