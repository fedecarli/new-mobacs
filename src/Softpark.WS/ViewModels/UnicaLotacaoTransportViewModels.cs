using Softpark.Infrastructure.Extras;
using Softpark.Models;
using Softpark.WS.Validators;
using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Runtime.Serialization;

namespace Softpark.WS.ViewModels
{
    /// <summary>
    /// Unica Lotação Header Transport
    /// </summary>
    /// <remarks>
    /// http://esusab.github.io/integracao/docs/header-transport.html#unicalotacaoheader
    /// </remarks>
    [DataContract(Name = nameof(UnicaLotacaoTransport))]
    public class UnicaLotacaoTransportCadastroViewModel
    {
        /// <summary>
        /// CNS do profissional
        /// </summary>
        /// <remarks>
        /// http://esusab.github.io/integracao/docs/algoritmo_CNS.html
        /// </remarks>
        [DataMember(Name = nameof(profissionalCNS))]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Informe o CNS do profissional.")]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "O CNS do profissional deve ter 15 digitos.")]
        [CnsValidation(ErrorMessage = "CNS inválido.")]
        public string profissionalCNS { get; set; }

        /// <summary>
        /// CBO do profissional
        /// </summary>
        /// <remarks>
        /// http://esusab.github.io/integracao/docs/cbo.html
        /// </remarks>
        [DataMember(Name = nameof(cboCodigo_2002))]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Informe o CBO do profissional.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "O CBO informado deve ter 6 digitos.")]
        [CboValidation(ErrorMessage = "O CBO informado é inválido ou não encontrado.")]
        public string cboCodigo_2002 { get; set; }

        /// <summary>
        /// CNES da unidade
        /// </summary>
        [DataMember(Name = nameof(cnes))]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Informe o CNES da unidade.")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "O CNES deve ter 7 digitos.")]
        [CnesValidation(ErrorMessage = "O CNES informado é inválido ou não foi encontrado.")]
        public string cnes { get; set; }

        /// <summary>
        /// INE da equipe
        /// </summary>
        [DataMember(Name = nameof(ine))]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "O INE da equipe deve ter 10 digitos.")]
        [IneValidation(ErrorMessage = "O INE informado é inválido ou não foi encontrado.")]
        public string ine { get; set; }

        /// <summary>
        /// Data do atendimento no formato UTC Unix Timestamp
        /// </summary>
        /// <remarks>
        /// https://pt.wikipedia.org/wiki/Era_Unix
        /// </remarks>
        [DataMember(Name = nameof(dataAtendimento))]
        [Required(ErrorMessage = "Informe a data de atendimento.")]
        [CustomValidation(typeof(Epoch), nameof(Epoch.ValidateESUSDate), ErrorMessage = "Data de Atendimento inválida.")]
        public long dataAtendimento { get; set; }

        /// <summary>
        /// Código IBGE do município
        /// </summary>
        /// <remarks>
        /// Se nulo, será substituído pelo valor atribuido ao arquivo de configuração da aplicação
        /// http://esusab.github.io/integracao/docs/municipios.html
        /// http://www.cidades.ibge.gov.br/v3/cidades/home-cidades
        /// </remarks>
        [DataMember(Name = nameof(codigoIbgeMunicipio))]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "O código do IBGE deve ter 7 digitos.")]
        public string codigoIbgeMunicipio { get; set; }
        
        internal UnicaLotacaoTransport ToModel()
        {
            var ult = DomainContainer.Current.UnicaLotacaoTransport.Create();

            ult.id = Guid.NewGuid();
            ult.profissionalCNS = profissionalCNS;
            ult.cboCodigo_2002 = cboCodigo_2002;
            ult.cnes = cnes;
            ult.ine = ine;
            ult.dataAtendimento = dataAtendimento.FromUnix();
            ult.codigoIbgeMunicipio = codigoIbgeMunicipio ?? "3547304";

            return ult;
        }

        internal static UnicaLotacaoTransportCadastroViewModel ApplyModel(UnicaLotacaoTransport model)
        {
            return new UnicaLotacaoTransportCadastroViewModel {
                cboCodigo_2002 = model.cboCodigo_2002,
                cnes = model.cnes,
                codigoIbgeMunicipio = model.codigoIbgeMunicipio,
                dataAtendimento = model.dataAtendimento.ToUnix(),
                ine = model.ine,
                profissionalCNS = model.profissionalCNS
            };
        }
    }
}
