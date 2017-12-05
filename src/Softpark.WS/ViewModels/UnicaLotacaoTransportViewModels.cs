using Softpark.Models;
using System;
using System.Linq;
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
        /// Codigo INE da equipe
        /// </summary>
        [DataMember(Name = nameof(codine))]
        public string codine { get; set; }

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
        /// Código ASSMED_Cadastro
        /// </summary>
        [DataMember(Name = nameof(profissionalNome))]
        public string profissionalNome { get; set; }

        /// <summary>
        /// Equipe
        /// </summary>
        [DataMember(Name = nameof(Equipe))]
        public string Equipe { get; set; }

        /// <summary>
        /// Profissao
        /// </summary>
        [DataMember(Name = nameof(Profissao))]
        public string Profissao { get; set; }

        /// <summary>
        /// Unidade
        /// </summary>
        [DataMember(Name = nameof(Unidade))]
        public string Unidade { get; set; }

        /// <summary>
        /// Codigo ASSMED
        /// </summary>
        [DataMember(Name = nameof(Codigo))]
        public int Codigo { get; set; }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="domain"></param>
        internal UnicaLotacaoTransport ToModel(DomainContainer domain)
        {
            var ult = domain.UnicaLotacaoTransport.Create();

            var _ine = domain.SetoresINEs
                .FirstOrDefault(x => (x.Numero != null && x.Numero.Trim() == ine.Trim()) || x.CodINE.ToString() == ine.Trim());

            ult.id = Guid.NewGuid();
            ult.profissionalCNS = profissionalCNS;
            ult.cboCodigo_2002 = cboCodigo_2002;
            ult.cnes = cnes;
            ult.ine = _ine?.Numero?.Trim();
            ult.dataAtendimento = dataAtendimento;
            ult.codigoIbgeMunicipio = codigoIbgeMunicipio ?? domain.ASSMED_Contratos.First().CodigoIbgeMunicipio;

            return ult;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        /// <param name="db"></param>
        internal static UnicaLotacaoTransportCadastroViewModel ApplyModel(UnicaLotacaoTransport model, DomainContainer db)
        {
            var prof = db.VW_Profissional.FirstOrDefault(x =>
                x.CNS == model.profissionalCNS &&
                x.CBO == model.cboCodigo_2002 &&
                x.CNES == model.cnes &&
                x.INE == model.ine
            ) ?? new VW_Profissional();

            var _ine = db.SetoresINEs
                .FirstOrDefault(x => x.Numero != null && x.Numero.Trim() == model.ine);

            return new UnicaLotacaoTransportCadastroViewModel
            {
                cboCodigo_2002 = prof.CBO ?? "",
                cnes = prof.CNES ?? "",
                codigoIbgeMunicipio = model.codigoIbgeMunicipio ?? "",
                dataAtendimento = model.dataAtendimento,
                ine = prof.INE ?? "",
                codine = _ine?.CodINE.ToString() ?? "",
                profissionalCNS = prof.CNS ?? "",
                profissionalNome = prof.Nome ?? "",
                Equipe = prof.Equipe ?? "",
                Profissao = prof.Profissao ?? "",
                Unidade = prof.Unidade ?? "",
                Codigo = prof.CodUsu
            };
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator UnicaLotacaoTransportCadastroViewModel(UnicaLotacaoTransport model)
        {
            return ApplyModel(model, DomainContainer.Current);
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator UnicaLotacaoTransport(UnicaLotacaoTransportCadastroViewModel model)
        {
            return model.ToModel(DomainContainer.Current);
        }

    }
}
