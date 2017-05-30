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
    /// Ficha de cadastro individual
    /// </summary>
    public partial class PrimitiveCadastroIndividualViewModel
    {
        /// <summary>
        /// Initializes a new instance of the
        /// PrimitiveCadastroIndividualViewModel class.
        /// </summary>
        public PrimitiveCadastroIndividualViewModel() { }

        /// <summary>
        /// Initializes a new instance of the
        /// PrimitiveCadastroIndividualViewModel class.
        /// </summary>
        public PrimitiveCadastroIndividualViewModel(Guid? token = default(Guid?), CondicoesDeSaudeViewModel condicoesDeSaude = default(CondicoesDeSaudeViewModel), EmSituacaoDeRuaViewModel emSituacaoDeRua = default(EmSituacaoDeRuaViewModel), bool? fichaAtualizada = default(bool?), IdentificacaoUsuarioCidadaoViewModel identificacaoUsuarioCidadao = default(IdentificacaoUsuarioCidadaoViewModel), InformacoesSocioDemograficasViewModel informacoesSocioDemograficas = default(InformacoesSocioDemograficasViewModel), bool? statusTermoRecusaCadastroIndividualAtencaoBasica = default(bool?), Guid? uuidFichaOriginadora = default(Guid?), SaidaCidadaoCadastroViewModel saidaCidadaoCadastro = default(SaidaCidadaoCadastroViewModel))
        {
            Token = token;
            CondicoesDeSaude = condicoesDeSaude;
            EmSituacaoDeRua = emSituacaoDeRua;
            FichaAtualizada = fichaAtualizada;
            IdentificacaoUsuarioCidadao = identificacaoUsuarioCidadao;
            InformacoesSocioDemograficas = informacoesSocioDemograficas;
            StatusTermoRecusaCadastroIndividualAtencaoBasica = statusTermoRecusaCadastroIndividualAtencaoBasica;
            UuidFichaOriginadora = uuidFichaOriginadora;
            SaidaCidadaoCadastro = saidaCidadaoCadastro;
        }

        /// <summary>
        /// Token de acesso
        /// </summary>
        [JsonProperty(PropertyName = "token")]
        public Guid? Token { get; set; }

        /// <summary>
        /// Condições de Saúde
        /// </summary>
        [JsonProperty(PropertyName = "condicoesDeSaude")]
        public CondicoesDeSaudeViewModel CondicoesDeSaude { get; set; }

        /// <summary>
        /// Em Situação de Rua
        /// </summary>
        [JsonProperty(PropertyName = "emSituacaoDeRua")]
        public EmSituacaoDeRuaViewModel EmSituacaoDeRua { get; set; }

        /// <summary>
        /// Ficha atualizada
        /// </summary>
        [JsonProperty(PropertyName = "fichaAtualizada")]
        public bool? FichaAtualizada { get; set; }

        /// <summary>
        /// Identificação do usuário cidadão
        /// </summary>
        [JsonProperty(PropertyName = "identificacaoUsuarioCidadao")]
        public IdentificacaoUsuarioCidadaoViewModel IdentificacaoUsuarioCidadao { get; set; }

        /// <summary>
        /// Informações socio demográficas
        /// </summary>
        [JsonProperty(PropertyName = "informacoesSocioDemograficas")]
        public InformacoesSocioDemograficasViewModel InformacoesSocioDemograficas { get; set; }

        /// <summary>
        /// Termo de cadastro recusado
        /// </summary>
        [JsonProperty(PropertyName = "statusTermoRecusaCadastroIndividualAtencaoBasica")]
        public bool? StatusTermoRecusaCadastroIndividualAtencaoBasica { get; set; }

        /// <summary>
        /// Ficha de origem, informar somente se a ficha for de atualização
        /// </summary>
        [JsonProperty(PropertyName = "uuidFichaOriginadora")]
        public Guid? UuidFichaOriginadora { get; set; }

        /// <summary>
        /// Dados da saída do cidadão do cadastro
        /// </summary>
        [JsonProperty(PropertyName = "saidaCidadaoCadastro")]
        public SaidaCidadaoCadastroViewModel SaidaCidadaoCadastro { get; set; }

    }
}