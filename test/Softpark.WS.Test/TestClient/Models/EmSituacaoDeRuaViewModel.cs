﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Softpark.WS.Test.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    /// <summary>
    /// Em situação de rua
    /// </summary>
    public partial class EmSituacaoDeRuaViewModel
    {
        /// <summary>
        /// Initializes a new instance of the EmSituacaoDeRuaViewModel class.
        /// </summary>
        public EmSituacaoDeRuaViewModel() { }

        /// <summary>
        /// Initializes a new instance of the EmSituacaoDeRuaViewModel class.
        /// </summary>
        public EmSituacaoDeRuaViewModel(string grauParentescoFamiliarFrequentado = default(string), string outraInstituicaoQueAcompanha = default(string), int? quantidadeAlimentacoesAoDiaSituacaoRua = default(int?), bool? statusAcompanhadoPorOutraInstituicao = default(bool?), bool? statusPossuiReferenciaFamiliar = default(bool?), bool? statusRecebeBeneficio = default(bool?), bool? statusSituacaoRua = default(bool?), bool? statusTemAcessoHigienePessoalSituacaoRua = default(bool?), bool? statusVisitaFamiliarFrequentemente = default(bool?), int? tempoSituacaoRua = default(int?), IList<int?> higienePessoalSituacaoRua = default(IList<int?>), IList<int?> origemAlimentoSituacaoRua = default(IList<int?>))
        {
            GrauParentescoFamiliarFrequentado = grauParentescoFamiliarFrequentado;
            OutraInstituicaoQueAcompanha = outraInstituicaoQueAcompanha;
            QuantidadeAlimentacoesAoDiaSituacaoRua = quantidadeAlimentacoesAoDiaSituacaoRua;
            StatusAcompanhadoPorOutraInstituicao = statusAcompanhadoPorOutraInstituicao;
            StatusPossuiReferenciaFamiliar = statusPossuiReferenciaFamiliar;
            StatusRecebeBeneficio = statusRecebeBeneficio;
            StatusSituacaoRua = statusSituacaoRua;
            StatusTemAcessoHigienePessoalSituacaoRua = statusTemAcessoHigienePessoalSituacaoRua;
            StatusVisitaFamiliarFrequentemente = statusVisitaFamiliarFrequentemente;
            TempoSituacaoRua = tempoSituacaoRua;
            HigienePessoalSituacaoRua = higienePessoalSituacaoRua;
            OrigemAlimentoSituacaoRua = origemAlimentoSituacaoRua;
        }

        /// <summary>
        /// Grau de parentesco
        /// </summary>
        [JsonProperty(PropertyName = "grauParentescoFamiliarFrequentado")]
        public string GrauParentescoFamiliarFrequentado { get; set; }

        /// <summary>
        /// Outra instituição
        /// </summary>
        [JsonProperty(PropertyName = "outraInstituicaoQueAcompanha")]
        public string OutraInstituicaoQueAcompanha { get; set; }

        /// <summary>
        /// Quantidade de refeições diária
        /// </summary>
        [JsonProperty(PropertyName = "quantidadeAlimentacoesAoDiaSituacaoRua")]
        public int? QuantidadeAlimentacoesAoDiaSituacaoRua { get; set; }

        /// <summary>
        /// Acompanhado por outra instituição
        /// </summary>
        [JsonProperty(PropertyName = "statusAcompanhadoPorOutraInstituicao")]
        public bool? StatusAcompanhadoPorOutraInstituicao { get; set; }

        /// <summary>
        /// Possui referência familiar
        /// </summary>
        [JsonProperty(PropertyName = "statusPossuiReferenciaFamiliar")]
        public bool? StatusPossuiReferenciaFamiliar { get; set; }

        /// <summary>
        /// Recebe beneficio
        /// </summary>
        [JsonProperty(PropertyName = "statusRecebeBeneficio")]
        public bool? StatusRecebeBeneficio { get; set; }

        /// <summary>
        /// Situação de Rua
        /// </summary>
        [JsonProperty(PropertyName = "statusSituacaoRua")]
        public bool? StatusSituacaoRua { get; set; }

        /// <summary>
        /// Tem acesso à higiene pessoa
        /// </summary>
        [JsonProperty(PropertyName = "statusTemAcessoHigienePessoalSituacaoRua")]
        public bool? StatusTemAcessoHigienePessoalSituacaoRua { get; set; }

        /// <summary>
        /// Visita familiar frequentemente
        /// </summary>
        [JsonProperty(PropertyName = "statusVisitaFamiliarFrequentemente")]
        public bool? StatusVisitaFamiliarFrequentemente { get; set; }

        /// <summary>
        /// Tempo da situação de rua
        /// </summary>
        [JsonProperty(PropertyName = "tempoSituacaoRua")]
        public int? TempoSituacaoRua { get; set; }

        /// <summary>
        /// Lista de higiene pessoal
        /// </summary>
        [JsonProperty(PropertyName = "higienePessoalSituacaoRua")]
        public IList<int?> HigienePessoalSituacaoRua { get; set; }

        /// <summary>
        /// Lista de origem de alimentos
        /// </summary>
        [JsonProperty(PropertyName = "origemAlimentoSituacaoRua")]
        public IList<int?> OrigemAlimentoSituacaoRua { get; set; }

    }
}