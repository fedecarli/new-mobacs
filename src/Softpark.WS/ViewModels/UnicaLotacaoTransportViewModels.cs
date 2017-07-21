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
        public string profissionalCNS { get; set; }

        /// <summary>
        /// CBO do profissional
        /// </summary>
        /// <remarks>
        /// http://esusab.github.io/integracao/docs/cbo.html
        /// </remarks>
        [DataMember(Name = nameof(cboCodigo_2002))]
        public string cboCodigo_2002 { get; set; }

        /// <summary>
        /// CNES da unidade
        /// </summary>
        [DataMember(Name = nameof(cnes))]
        public string cnes { get; set; }

        /// <summary>
        /// INE da equipe
        /// </summary>
        [DataMember(Name = nameof(ine))]
        public string ine { get; set; }

        /// <summary>
        /// Data do atendimento no formato UTC Unix Timestamp
        /// </summary>
        /// <remarks>
        /// https://pt.wikipedia.org/wiki/Era_Unix
        /// </remarks>
        [DataMember(Name = nameof(dataAtendimento))]
        public DateTime dataAtendimento { get; set; }

        /// <summary>
        /// Código IBGE do município
        /// </summary>
        /// <remarks>
        /// Se nulo, será substituído pelo valor atribuido ao arquivo de configuração da aplicação
        /// http://esusab.github.io/integracao/docs/municipios.html
        /// http://www.cidades.ibge.gov.br/v3/cidades/home-cidades
        /// </remarks>
        [DataMember(Name = nameof(codigoIbgeMunicipio))]
        public string codigoIbgeMunicipio { get; set; }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="domain"></param>
        internal UnicaLotacaoTransport ToModel(DomainContainer domain)
        {
            var ult = domain.UnicaLotacaoTransport.Create();

            ult.id = Guid.NewGuid();
            ult.profissionalCNS = profissionalCNS;
            ult.cboCodigo_2002 = cboCodigo_2002;
            ult.cnes = cnes;
            ult.ine = ine;
            ult.dataAtendimento = dataAtendimento;
            ult.codigoIbgeMunicipio = codigoIbgeMunicipio??ConfigurationManager.AppSettings["idMunicipioCliente"];

            return ult;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        internal static UnicaLotacaoTransportCadastroViewModel ApplyModel(UnicaLotacaoTransport model)
        {
            return new UnicaLotacaoTransportCadastroViewModel {
                cboCodigo_2002 = model.cboCodigo_2002,
                cnes = model.cnes,
                codigoIbgeMunicipio = model.codigoIbgeMunicipio,
                dataAtendimento = model.dataAtendimento,
                ine = model.ine,
                profissionalCNS = model.profissionalCNS
            };
        }
    }
}
