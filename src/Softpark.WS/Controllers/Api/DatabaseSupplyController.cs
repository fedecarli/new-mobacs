using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Softpark.Models;
using Softpark.WS.ViewModels;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Softpark.Infrastructure.Extras;
using static Softpark.Infrastructure.Extensions.WithStatement;

namespace Softpark.WS.Controllers.Api
{
    /// <summary>
    /// Common datasets controller
    /// </summary>
    [System.Web.Mvc.OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
    [System.Web.Mvc.SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class DatabaseSupplyController : BaseApiController
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        /// <summary>
        /// Este construtor é usado para o sistema de documentação poder gerar o swagger e o Help
        /// </summary>
        protected DatabaseSupplyController() : base(new DomainContainer()) { }

        /// <summary>
        /// Este construtor é inicializado pelo asp.net usando injeção de dependência
        /// </summary>
        /// <param name="domain">Domínio do banco inicializado por injeção de dependência</param>
        public DatabaseSupplyController(DomainContainer domain) : base(domain)
        {
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Endpoint para retornar a versão atual da API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/version")]
        [ResponseType(typeof(string))]
        public IHttpActionResult GetVersion()
        {
            return Ok(Versions.Version);
        }

        /// <summary>
        /// Endpoint para conversão de DateTime para Epoch
        /// </summary>
        /// <param name="data">Data no formato DateTime (yyyy-MM-ddTHH:mm:ssZ)</param>
        /// <returns>Epoch</returns>
        [HttpGet]
        [Route("api/dados/epoch", Name = "GetEpochURI")]
        [ResponseType(typeof(long))]
        public IHttpActionResult GetEpoch([FromUri] DateTime data)
        {
            return Ok(data.ToUnix());
        }

        /// <summary>
        /// Log4Net
        /// </summary>
        private static log4net.ILog Log { get; set; } = log4net.LogManager.GetLogger(typeof(DatabaseSupplyController));

        /// <summary>
        /// Endpoint para download de dados básicos para carga de trabalho
        /// </summary>
        /// <param name="modelo">O nome do modelo de dados que deseja consultar.</param>
        /// <returns>Retorna uma coleção de dados básicos</returns>
        [HttpGet]
        [Route("api/dados/{modelo}", Name = "BasicSupplyAction")]
        [ResponseType(typeof(BasicViewModel[]))]
        public IHttpActionResult GetEntities([FromUri, Required] string modelo)
        {
            List<BasicViewModel> model;

            switch (modelo.ToLowerInvariant())
            {
                case "estadocivil":
                    model = Domain.TP_EstadoCivil
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "EstadoCivil",
                            Codigo = x.codigo,
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "doencacardiaca":
                    model = Domain.TP_Doenca_Cardiaca
                        .Where(x => x.ativo == 1).Select(x => new BasicViewModel
                        {
                            Modelo = "DoencaCardiaca",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "doencarespiratoria":
                    model = Domain.TP_Doenca_Respiratoria
                        .Where(x => x.ativo == 1).Select(x => new BasicViewModel
                        {
                            Modelo = "DoencaRespiratoria",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "formadeescoamentodobanheiroousanitario":
                    model = Domain.TP_Escoamento_Esgoto
                        .Where(x => x.ativo == 1).Select(x => new BasicViewModel
                        {
                            Modelo = "FormaDeEscoamentoDoBanheiroOuSanitario",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "problemarins":
                    model = Domain.TP_Doenca_Renal
                        .Where(x => x.ativo == 1).Select(x => new BasicViewModel
                        {
                            Modelo = "ProblemaRins",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "consideracaopeso":
                    model = Domain.TP_Consideracao_Peso
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "ConsideracaoPeso",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = x.observacao
                        }).ToList();
                    break;
                case "acessohigiene":
                    model = Domain.TP_Higiene_Pessoal
                        .Where(x => x.ativo == 1).Select(x => new BasicViewModel
                        {
                            Modelo = "AcessoHigiene",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "origemalimentacao":
                    model = Domain.TP_Origem_Alimentacao
                        .Where(x => x.ativo == 1).Select(x => new BasicViewModel
                        {
                            Modelo = "OrigemAlimentacao",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "quantasvezesalimentacao":
                    model = Domain.TP_Quantas_Vezes_Alimentacao
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "QuantasVezesAlimentacao",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = x.observacao
                        }).ToList();
                    break;
                case "temposituacaoderua":
                    model = Domain.TP_Sit_Rua
                        .Where(x => x.ativo == 1)
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "TempoSituacaoDeRua",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "municipios":
                    model = Domain.Cidade
                        .Where(x => x.CodIbge != null && x.UF != null)
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "Municipios",
                            Codigo = x.CodIbge.ToString(),
                            Descricao = x.NomeCidade,
                            Observacao = x.UF
                        }).ToList();
                    break;
                case "nacionalidade":
                    model = Domain.TP_Nacionalidade
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "Nacionalidade",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = x.observacao
                        }).ToList();
                    break;
                case "pais":
                    model = Domain.Nacionalidade
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "Pais",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.DesNacao,
                            Observacao = null
                        }).ToList();
                    break;
                case "racacor":
                    model = Domain.TP_Raca_Cor
                        .Where(x => x.ativo == 1)
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "RacaCor",
                            Codigo = x.id_tp_raca_cor.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "sexo":
                    model = Domain.TP_Sexo
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "Sexo",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = x.observacao
                        }).ToList();
                    break;
                case "etnia":
                    model = Domain.Etnia
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "Etnia",
                            Codigo = x.CodEtnia.ToString(),
                            Descricao = x.DesEtnia,
                            Observacao = null
                        }).ToList();
                    break;
                case "deficienciacidadao":
                    model = Domain.TP_Deficiencia
                        .Where(x => x.ativo == 1)
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "DeficienciaCidadao",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "cursomaiselevado":
                    model = Domain.TP_Curso
                        .Where(x => x.ativo == 1)
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "CursoMaisElevado",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "cbo":
                    model = Domain.AS_ProfissoesTab
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "CBO",
                            Codigo = x.CodProfTab.ToString(),
                            Descricao = x.DesProfTab,
                            Observacao = null
                        }).ToList();
                    break;
                case "orientacaosexual":
                    model = Domain.TP_Orientacao_Sexual
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "OrientacaoSexual",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = x.observacoes
                        }).ToList();
                    break;
                case "relacaoparentesco":
                    model = Domain.TP_Relacao_Parentesco
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "RelacaoParentesco",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = x.observacoes
                        }).ToList();
                    break;
                case "situacaomercadotrabalho":
                    model = Domain.TP_Sit_Mercado
                        .Where(x => x.ativo == 1)
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "SituacaoMercadoTrabalho",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "identidadegenerocidadao":
                    model = Domain.TP_Identidade_Genero_Cidadao
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "identidadeGeneroCidadao",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = x.observacao
                        }).ToList();
                    break;
                case "responsavelcrianca":
                    model = Domain.TP_Crianca
                        .Where(x => x.ativo == 1)
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "ResponsavelCrianca",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "motivosaida":
                    model = Domain.TP_Motivo_Saida
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "MotivoSaida",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = x.observacao
                        }).ToList();
                    break;
                case "cnes":
                    model = (from p in Domain.AS_SetoresPar
                             let s = p.Setores
                             where p.CNES != null && p.CNES.Trim().Length > 0
                             select new BasicViewModel
                             {
                                 Modelo = "CNES",
                                 Codigo = p.CNES,
                                 Descricao = s.DesSetor,
                                 Observacao = s.DesSetorRes
                             })
                             .ToList();
                    break;
                case "ine":
                    model = Domain.SetoresINEs
                        .Where(x => x.Numero != null)
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "INE",
                            Codigo = x.Numero,
                            Descricao = x.Descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "animalnodomicilio":
                    model = Domain.TP_Animais
                        .Where(x => x.ativo == 1)
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "AnimalNoDomicilio",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "abastecimentodeagua":
                    model = Domain.TP_Abastecimento_Agua
                        .Where(x => x.ativo == 1)
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "AbastecimentoDeAgua",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "condicaodeposseeusodaterra":
                    model = Domain.TP_Cond_Posse_Uso_Terra
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "CondicaoDePosseEUsoDaTerra",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = x.observacoes
                        }).ToList();
                    break;
                case "destinodolixo":
                    model = Domain.TP_Destino_Lixo
                        .Where(x => x.ativo == 1)
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "DestinoDoLixo",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "localizacaodamoradia":
                    model = Domain.TP_Localizacao
                        .Where(x => x.ativo == 1)
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "LocalizacaoDaMoradia",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "materialpredominantenaconstrucao":
                    model = Domain.TP_Construcao_Domicilio
                        .Where(x => x.ativo == 1)
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "MaterialPredominanteNaConstrucao",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "situacaodemoradia":
                    model = Domain.TP_Situacao_Moradia
                        .Where(x => x.ativo == 1)
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "SituacaoDeMoradia",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "tipodeacessoaodomicilio":
                    model = Domain.TP_Acesso_Domicilio
                        .Where(x => x.ativo == 1)
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "TipoDeAcessoAoDomicilio",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "tipodedomicilio":
                    model = Domain.TP_Domicilio
                        .Where(x => x.ativo == 1)
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "TipoDeDomicilio",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "aguaconsumodomicilio":
                    model = Domain.TP_Tratamento_Agua
                        .Where(x => x.ativo == 1)
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "AguaConsumoDomicilio",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "uf":
                    var n = 1;

                    model = Domain.UF.ToList()
                        .OrderBy(x => x.DesUF)
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "UF",
                            Codigo = (n++).ToString().PadLeft(2, '0'),
                            Descricao = x.DesUF,
                            Observacao = x.UF1
                        })
                        .ToList();
                    break;
                case "tipodelogradouro":
                    model = Domain.TB_MS_TIPO_LOGRADOURO
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "TipoDeLogradouro",
                            Codigo = x.CO_TIPO_LOGRADOURO.ToString(),
                            Descricao = x.DS_TIPO_LOGRADOURO,
                            Observacao = x.DS_TIPO_LOGRADOURO_ABREV
                        }).ToList();
                    break;
                case "rendafamiliar":
                    model = Domain.TP_Renda_Familiar
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "RendaFamiliar",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = x.observacoes
                        }).ToList();
                    break;
                case "tipodeimovel":
                    model = Domain.TP_Imovel
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "tipoDeImovel",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.descricao,
                            Observacao = null
                        }).ToList();
                    break;
                case "turno":
                    model = new[] { new BasicViewModel
                        {
                            Modelo = "Turno",
                            Codigo = "1",
                            Descricao = "Manhã"
                        }, new BasicViewModel {
                            Modelo = "Turno",
                            Codigo = "2",
                            Descricao = "Tarde"
                        }, new BasicViewModel {
                            Modelo = "Turno",
                            Codigo = "3",
                            Descricao = "Noite"
                        } }.ToList();
                    break;
                case "motivovisita":
                    model = Domain.SIGSM_MotivoVisita
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "MotivoVisita",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.nome,
                            Observacao = x.observacoes
                        }).ToList();
                    break;
                case "desfecho":
                    model = new[] { new BasicViewModel
                        {
                            Modelo = "Desfecho",
                            Codigo = "1",
                            Descricao = "Visita realizada"
                        }, new BasicViewModel {
                            Modelo = "Desfecho",
                            Codigo = "2",
                            Descricao = "Visita recusada"
                        }, new BasicViewModel {
                            Modelo = "Desfecho",
                            Codigo = "3",
                            Descricao = "Ausente"
                        }
                    }.ToList();
                    break;
                default:
                    throw new ArgumentException("O modelo solicitado é inválido.", nameof(modelo));
            }

            return Ok(model.ToArray());
        }

        /// <summary>
        /// Endpoint para download de dados dos profissionais
        /// </summary>
        /// <returns>Coleção com os dados dos profissionais</returns>
        [HttpGet]
        [Route("api/dados/profissional", Name = "ProfessionalSupplyAction")]
        [ResponseType(typeof(ProfissionalViewModel[]))]
        public IHttpActionResult GetProfissionais()
        {
            var profs = Domain.VW_Profissional.ToList();

            var ps = new Dictionary<string, ProfissionalViewModel>();

            Func<VW_Profissional, ProfissionalViewModel> __ = prof =>
            {
                var _ = new ProfissionalViewModel();

                var cns = prof?.CNS?.Trim();

                if (cns != null)
                    ps.Add(cns, _);

                return _;
            };

            foreach (var prof in profs)
            {
                var p = ps.ContainsKey(prof.CNS) ? ps[prof.CNS] : __.Invoke(prof);

                p.CNS = prof.CNS?.Trim();
                p.Nome = prof.Nome?.Trim();

                p.Append(prof);
            }

            return Ok(ps.Values.ToArray());
        }

        /// <summary>
        /// Endpoint para listar os modelos de dados consultáveis
        /// </summary>
        /// <returns>Coleção de modelos de dados</returns>
        [HttpGet]
        [Route("api/dados/modelos", Name = "ModelSupplyAction")]
        [ResponseType(typeof(BasicViewModel[]))]
        public IHttpActionResult GetModels()
        {
            return Ok(new[] {
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "DoencaCardiaca", Descricao = "DoencaCardiaca", Observacao = "/api/dados/DoencaCardiaca" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "DoencaRespiratoria", Descricao = "DoencaRespiratoria", Observacao = "/api/dados/DoencaRespiratoria" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "FormaDeEscoamentoDoBanheiroOuSanitario", Descricao = "FormaDeEscoamentoDoBanheiroOuSanitario", Observacao = "/api/dados/FormaDeEscoamentoDoBanheiroOuSanitario" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "ProblemaRins", Descricao = "ProblemaRins", Observacao = "/api/dados/ProblemaRins" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "ConsideracaoPeso", Descricao = "ConsideracaoPeso", Observacao = "/api/dados/ConsideracaoPeso" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "AcessoHigiene", Descricao = "AcessoHigiene", Observacao = "/api/dados/AcessoHigiene" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "OrigemAlimentacao", Descricao = "OrigemAlimentacao", Observacao = "/api/dados/OrigemAlimentacao" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "QuantasVezesAlimentacao", Descricao = "QuantasVezesAlimentacao", Observacao = "/api/dados/QuantasVezesAlimentacao" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "TempoSituacaoDeRua", Descricao = "TempoSituacaoDeRua", Observacao = "/api/dados/TempoSituacaoDeRua" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "Municipios", Descricao = "Municipios", Observacao = "/api/dados/Municipios" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "Nacionalidade", Descricao = "Nacionalidade", Observacao = "/api/dados/Nacionalidade" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "Pais", Descricao = "Pais", Observacao = "/api/dados/Pais" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "RacaCor", Descricao = "RacaCor", Observacao = "/api/dados/RacaCor" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "Sexo", Descricao = "Sexo", Observacao = "/api/dados/Sexo" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "Etnia", Descricao = "Etnia", Observacao = "/api/dados/Etnia" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "DeficienciaCidadao", Descricao = "DeficienciaCidadao", Observacao = "/api/dados/DeficienciaCidadao" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "CursoMaisElevado", Descricao = "CursoMaisElevado", Observacao = "/api/dados/CursoMaisElevado" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "CBO", Descricao = "CBO", Observacao = "/api/dados/CBO" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "OrientacaoSexual", Descricao = "OrientacaoSexual", Observacao = "/api/dados/OrientacaoSexual" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "RelacaoParentesco", Descricao = "RelacaoParentesco", Observacao = "/api/dados/RelacaoParentesco" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "SituacaoMercadoTrabalho", Descricao = "SituacaoMercadoTrabalho", Observacao = "/api/dados/SituacaoMercadoTrabalho" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "identidadeGeneroCidadao", Descricao = "identidadeGeneroCidadao", Observacao = "/api/dados/identidadeGeneroCidadao" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "ResponsavelCrianca", Descricao = "ResponsavelCrianca", Observacao = "/api/dados/ResponsavelCrianca" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "MotivoSaida", Descricao = "MotivoSaida", Observacao = "/api/dados/MotivoSaida" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "CNES", Descricao = "CNES", Observacao = "/api/dados/CNES" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "INE", Descricao = "INE", Observacao = "/api/dados/INE" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "AnimalNoDomicilio", Descricao = "AnimalNoDomicilio", Observacao = "/api/dados/AnimalNoDomicilio" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "AbastecimentoDeAgua", Descricao = "AbastecimentoDeAgua", Observacao = "/api/dados/AbastecimentoDeAgua" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "CondicaoDePosseEUsoDaTerra", Descricao = "CondicaoDePosseEUsoDaTerra", Observacao = "/api/dados/CondicaoDePosseEUsoDaTerra" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "DestinoDoLixo", Descricao = "DestinoDoLixo", Observacao = "/api/dados/DestinoDoLixo" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "LocalizacaoDaMoradia", Descricao = "LocalizacaoDaMoradia", Observacao = "/api/dados/LocalizacaoDaMoradia" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "MaterialPredominanteNaConstrucao", Descricao = "MaterialPredominanteNaConstrucao", Observacao = "/api/dados/MaterialPredominanteNaConstrucao" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "SituacaoDeMoradia", Descricao = "SituacaoDeMoradia", Observacao = "/api/dados/SituacaoDeMoradia" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "TipoDeAcessoAoDomicilio", Descricao = "TipoDeAcessoAoDomicilio", Observacao = "/api/dados/TipoDeAcessoAoDomicilio" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "TipoDeDomicilio", Descricao = "TipoDeDomicilio", Observacao = "/api/dados/TipoDeDomicilio" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "AguaConsumoDomicilio", Descricao = "AguaConsumoDomicilio", Observacao = "/api/dados/AguaConsumoDomicilio" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "UF", Descricao = "UF", Observacao = "/api/dados/UF" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "TipoDeLogradouro", Descricao = "TipoDeLogradouro", Observacao = "/api/dados/TipoDeLogradouro" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "RendaFamiliar", Descricao = "RendaFamiliar", Observacao = "/api/dados/RendaFamiliar" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "tipoDeImovel", Descricao = "tipoDeImovel", Observacao = "/api/dados/tipoDeImovel" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "Turno", Descricao = "Turno", Observacao = "/api/dados/Turno" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "MotivoVisita", Descricao = "MotivoVisita", Observacao = "/api/dados/MotivoVisita" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "Desfecho", Descricao = "Desfecho", Observacao = "/api/dados/Desfecho" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "EstadoCivil", Descricao = "EstadoCivil", Observacao = "/api/dados/EstadoCivil" },
                new BasicViewModel { Modelo = "ProfissionalViewModel", Codigo = "Profissional", Descricao = "Profissional", Observacao = "/api/dados/profissional" }
            }.OrderBy(x => x.Codigo).ToArray());
        }

        /// <summary>
        /// Método para buscar o cabeçalho
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<UnicaLotacaoTransport> GetHeader(Guid token)
        {
            return await Domain.UnicaLotacaoTransport.FirstOrDefaultAsync(u => u.token == token && !u.OrigemVisita.finalizado);
        }

        /// <summary>
        /// Método para buscar todos os cabeçalhos de um profissional
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private IQueryable<UnicaLotacaoTransport> GetHeadersBy(UnicaLotacaoTransport header)
        {
            return Domain.UnicaLotacaoTransport.Where(u => u.profissionalCNS == header.profissionalCNS && u.OrigemVisita.finalizado);
        }

        /// <summary>
        /// Endpoint para buscar pacientes atendidos pelo profissional informado
        /// </summary>
        /// <param name="token">Token de acesso</param>
        /// <returns>Coleção de Pacientes</returns>
        [HttpGet]
        [Route("api/dados/paciente/{token:guid}", Name = "PacienteSupplyAction")]
        [ResponseType(typeof(GetCadastroIndividualViewModel[]))]
        public async Task<IHttpActionResult> GetPacientes([FromUri, Required] Guid token)
        {
            Log.Info("-----");
            Log.Info($"GET api/dados/paciente/{token}");

            var headerToken = await GetHeader(token);

            if (headerToken == null)
            {
                var error = BadRequest("Token Inválido.");

                Log.Fatal("Token inválido");

                return error;
            }

            var prof = Domain.GetProfissionalMobile(headerToken.cnes, headerToken.ine, headerToken.cboCodigo_2002, headerToken.profissionalCNS);

            var pessoas = (from scc in Domain.SIGSM_Check_Cadastros
                           join mcv in Domain.SIGSM_MicroArea_CredenciadoVinc
                           on scc.CodCred equals mcv.CodCred
                           join cad in Domain.ASSMED_Cadastro
                           on scc.Codigo equals cad.Codigo
                           let end = (from end in Domain.ASSMED_Endereco
                                      where cad.Codigo == end.Codigo
                                      orderby end.ItemEnd descending
                                      select end).FirstOrDefault()
                           let cpf = (from cf in Domain.ASSMED_CadastroPF
                                      where cad.Codigo == cf.Codigo
                                      select cf).FirstOrDefault()
                           where !scc.BaixarEndereco &&
                           scc.DataDownload == null &&
                           scc.AS_Credenciados.Codigo == prof.CodUsu
                           select new { scc, end, cad, cpf, mcv }).ToList();

            pessoas.ForEach(x =>
            {
                x.scc.DataDownload = DateTime.Now;
            });

            await Domain.SaveChangesAsync();

            var cads = pessoas.Select(x =>
            {
                var cad = x.cad.IdentificacaoUsuarioCidadao?.CadastroIndividual.FirstOrDefault();

                if (cad == null)
                {
                    cad = new CadastroIndividual
                    {
                        id = Guid.NewGuid(),
                        DataRegistro = DateTime.Now,
                        UnicaLotacaoTransport = headerToken,
                        tpCdsOrigem = 3
                    };
                }

                var iden = cad.IdentificacaoUsuarioCidadao1;

                if (iden == null)
                {
                    iden = new IdentificacaoUsuarioCidadao
                    {
                        id = Guid.NewGuid()
                    };
                    cad.IdentificacaoUsuarioCidadao1 = iden;
                }

                var codc = x.cad.ASSMED_PesFisica?.MUNICIPIONASC ?? x.cpf?.MUNICIPIONASC;

                var cid = Domain.Cidade.FirstOrDefault(y => y.CodCidade == codc)?.CodIbge?.Trim() ??
                headerToken.codigoIbgeMunicipio;

                iden.ASSMED_Cadastro1 = x.cad;
                iden.cnsCidadao = x.cad.ASSMED_CadastroDocPessoal.LastOrDefault(y => y.CodTpDocP == 6 && y.Numero != null
                && y.Numero.Trim().Length == 15)?.Numero;
                iden.cnsResponsavelFamiliar = iden.cnsCidadao;
                iden.Codigo = x.cad.Codigo;
                iden.codigoIbgeMunicipioNascimento = cid;
                var rg = x.cad.ASSMED_CadastroDocPessoal.FirstOrDefault(y => y.CodTpDocP == 1 && y.Numero != null);
                iden.RG = rg != null ? (rg.Numero.Trim() + (rg.ComplementoRG ?? "").Trim()) : null;
                iden.CPF = x.cad.ASSMED_PesFisica?.CPF ?? x.cpf?.CPF;
                iden.dataNascimentoCidadao = x.cad.ASSMED_PesFisica?.DtNasc ?? x.cpf?.DtNasc ?? DateTime.MinValue;
                iden.dtEntradaBrasil = x.cad.ASSMED_PesFisica?.ESTRANGEIRADATA ?? x.cpf?.ESTRANGEIRADATA;
                iden.dtNaturalizacao = x.cad.ASSMED_PesFisica?.NATURALIZADADATA ?? x.cpf?.NATURALIZADADATA;
                iden.emailCidadao = x.cad.ASSMED_CadEmails.Select(y => y.EMail.ToLower()).LastOrDefault();
                iden.EstadoCivil = x.cad.ASSMED_PesFisica?.EstCivil ?? x.cpf?.EstCivil ?? "I";
                iden.etnia = x.cad.ASSMED_PesFisica?.CodEtnia ?? x.cpf?.CodEtnia;
                iden.microarea = x.cad.ASSMED_Endereco.OrderByDescending(y => y.ItemEnd).Select(y => y.MicroArea)
                .FirstOrDefault() ?? x.cad.MicroArea;
                iden.nacionalidadeCidadao = x.cad.ASSMED_PesFisica?.Nacionalidade ?? x.cpf?.Nacionalidade ?? 10;
                iden.nomeCidadao = x.cad.Nome;
                iden.nomeSocial = x.cad.NomeSocial;
                iden.nomeMaeCidadao = x.cad.ASSMED_PesFisica?.NomeMae ?? x.cpf?.NomeMae;
                iden.nomePaiCidadao = x.cad.ASSMED_PesFisica?.NomePai ?? x.cpf?.NomePai;
                iden.numeroNisPisPasep = x.cad.ASSMED_CadastroDocPessoal.FirstOrDefault(y => y.CodTpDocP == 7 && y.Numero != null)?
                .Numero?.Trim();
                iden.num_contrato = 22;
                iden.paisNascimento = x.cad.ASSMED_PesFisica?.ENDPAIS ?? x.cpf?.ENDPAIS;
                iden.portariaNaturalizacao = x.cad.ASSMED_PesFisica?.NATURALIZACAOPORTARIA ?? x.cpf?.NATURALIZACAOPORTARIA;
                iden.racaCorCidadao = x.cad.ASSMED_PesFisica?.CodCor ?? x.cpf?.CodCor ?? 6;
                var sexo = (x.cad.ASSMED_PesFisica?.Sexo ?? x.cpf?.Sexo ?? "I");
                iden.sexoCidadao = sexo == "M" ? 0 : sexo == "F" ? 1 : 4;
                iden.stForaArea = iden.microarea == null;
                var fone = x.cad.ASSMED_CadTelefones.FirstOrDefault(y => y.TipoTel == "P" && y.NumTel != null);
                iden.telefoneCelular = fone == null ? null : $"{fone.DDD}{fone.NumTel}";

                return cad;
            }).Distinct().ToArray();

            CadastroIndividualViewModelCollection results = cads;

            var serializer = new JavaScriptSerializer();
            Log.Info(serializer.Serialize(results.ToArray()));

            return Ok(results.ToArray());
        }

        /// <summary>
        /// Endpoint para buscar os domicílios atendidos pelo profissional informado
        /// </summary>
        /// <param name="token">Token de acesso</param>
        /// <returns>Coleção de domicilios</returns>
        [HttpGet]
        [Route("api/dados/domicilio/{token:guid}", Name = "DomicilioSupplyAction")]
        [ResponseType(typeof(GetCadastroDomiciliarViewModel[]))]
        public async Task<IHttpActionResult> GetDomicilios([FromUri, Required] Guid token)
        {
            try
            {
                Log.Info("----");
                Log.Info($"api/dados/domicilio/{token}");

                var headerToken = await GetHeader(token);

                if (headerToken == null) return BadRequest("Token Inválido.");

                var prof = Domain.GetProfissionalMobile(headerToken.cnes, headerToken.ine, headerToken.cboCodigo_2002, headerToken.profissionalCNS);

                var domicilios = (from scc in Domain.SIGSM_Check_Cadastros
                                  join mcv in Domain.SIGSM_MicroArea_CredenciadoVinc
                                  on scc.CodCred equals mcv.CodCred
                                  join cad in Domain.ASSMED_Cadastro
                                  on scc.Codigo equals cad.Codigo
                                  let end = (from end in Domain.ASSMED_Endereco
                                             where cad.Codigo == end.Codigo
                                             orderby end.ItemEnd descending
                                             select end).FirstOrDefault()
                                  let cpf = (from cf in Domain.ASSMED_CadastroPF
                                             where cad.Codigo == cf.Codigo
                                             select cf).FirstOrDefault()
                                  where scc.BaixarEndereco &&
                                  scc.DataDownload == null &&
                                  scc.AS_Credenciados.Codigo == prof.CodUsu
                                  select new { scc, end, cad, cpf, mcv }).ToList();

                domicilios.ForEach(x =>
                {
                    x.scc.DataDownload = DateTime.Now;
                });

                await Domain.SaveChangesAsync();

                var cadastros = domicilios.Select(x =>
                {
                    var dom = x.end?.EnderecoLocalPermanencia?.CadastroDomiciliar?.FirstOrDefault();

                    if (dom == null)
                    {
                        dom = new CadastroDomiciliar
                        {
                            id = Guid.NewGuid(),
                            AnimalNoDomicilio = new AnimalNoDomicilio[0],
                            DataRegistro = DateTime.Now,
                            Erro = false,
                            FamiliaRow = new[] {
                                new FamiliaRow {
                                    dataNascimentoResponsavel = x.cpf?.DtNasc??x.cad.ASSMED_PesFisica?.DtNasc,
                                    id = Guid.NewGuid(),
                                    numeroCnsResponsavel = x.cad.ASSMED_CadastroDocPessoal.FirstOrDefault(y => y.CodTpDocP == 6 && y.Numero != null && y.Numero.Trim().Length == 15)?
                                    .Numero?.Trim(),
                                    numeroMembrosFamilia = 1
                                }
                            },
                            fichaAtualizada = false,
                            UnicaLotacaoTransport = headerToken,
                            tpCdsOrigem = 3
                        };
                    }

                    var end = dom.EnderecoLocalPermanencia1;

                    if (end == null)
                    {
                        end = new EnderecoLocalPermanencia
                        {
                            id = Guid.NewGuid()
                        };
                        dom.EnderecoLocalPermanencia1 = end;
                    }

                    end.microarea = x.end?.MicroArea ?? x.cad.MicroArea ?? x.mcv.SIGSM_MicroArea_Unidade.MicroArea;
                    end.stForaArea = end.microarea == null;
                    end.nomeLogradouro = x.end?.Logradouro;
                    end.numero = x.end?.Numero;
                    end.numeroDneUf = x.end != null ? Domain.UF.SingleOrDefault(y => y.UF1 == x.end.UF)?.DNE : null;
                    end.pontoReferencia = x.end?.ENDREFERENCIA;
                    end.stSemNumero = (end.numero ?? "").Length == 0;
                    var fone = x.cad.ASSMED_CadTelefones.LastOrDefault(y => y.TipoTel != "R" && y.NumTel != null);
                    end.telefoneContato = fone == null ? null : (fone.DDD + fone.NumTel);
                    fone = x.cad.ASSMED_CadTelefones.LastOrDefault(y => y.TipoTel == "R" && y.NumTel != null);
                    end.telefoneResidencia = fone == null ? null : (fone.DDD + fone.NumTel);
                    end.tipoLogradouroNumeroDne = x.end == null ? null :
                        Domain.TB_MS_TIPO_LOGRADOURO.FirstOrDefault(y => y.DS_TIPO_LOGRADOURO_ABREV == x.end.TipoEnd)?
                        .CO_TIPO_LOGRADOURO;
                    return dom;
                }).ToArray();

                CadastroDomiciliarViewModelCollection results = cadastros;
                
                var resultados = results.ToArray();

                var serializer = new JavaScriptSerializer();
                Log.Info(serializer.Serialize(resultados));

                return Ok(resultados);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message, ex);
                throw new ValidationException(ex.Message);
            }
        }

        /// <summary>
        /// Endpoint para buscar as visitas realizadas pelo profissional informado
        /// </summary>
        /// <param name="token">Token de acesso</param>
        /// <param name="microarea">Microárea</param>
        /// <returns>Coleção de visitas</returns>
        [HttpGet]
        [Route("api/dados/visita/{token:guid}", Name = "VisitaSupplyAction")]
        [ResponseType(typeof(FichaVisitaDomiciliarChildCadastroViewModel[]))]
        public async Task<IHttpActionResult> GetVisitas([FromUri, Required] Guid token, [FromUri] string microarea = null)
        {
            Log.Info("----");
            Log.Info($"api/dados/visita/{token}");

            var headerToken = await GetHeader(token);

            if (headerToken == null) return BadRequest("Token Inválido.");

            FichaVisitaDomiciliarChildCadastroViewModelCollection results = GetHeadersBy(headerToken)
                .SelectMany(f => f.FichaVisitaDomiciliarMaster).SelectMany(f => f.FichaVisitaDomiciliarChild).ToArray();

            if (microarea != null && Regex.IsMatch(microarea, "^([0-9][0-9])$"))
            {
                results = results.Where(r => r.microarea == null || r.microarea == microarea).ToArray();
            }

            var serializer = new JavaScriptSerializer();
            Log.Info(serializer.Serialize(results.ToArray()));

            return Ok(results.ToArray());
        }
    }

    /// <summary>
    /// ViewModel de Profissional
    /// </summary>
    public class ProfissionalViewModel
    {
        /// <summary>
        /// Coleção de CBOs
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public List<BasicViewModel> CBOs { get; set; } = new List<BasicViewModel>();

        /// <summary>
        /// Coleção de CNESs
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public List<BasicViewModel> CNESs { get; set; } = new List<BasicViewModel>();

        /// <summary>
        /// Coleção de INEs
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public List<BasicViewModel> INEs { get; set; } = new List<BasicViewModel>();

        /// <summary>
        /// CNS do profissional
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string CNS { get; set; }

        /// <summary>
        /// Nome do profissional
        /// </summary>
        public string Nome { get; set; }

        // ReSharper disable once InconsistentNaming
        internal static BasicViewModel GetINE(VW_Profissional arg)
        {
            return new BasicViewModel { Codigo = arg.INE?.Trim(), Descricao = arg.Equipe?.Trim(), Modelo = "INE" };
        }

        // ReSharper disable once InconsistentNaming
        internal static BasicViewModel GetCNES(VW_Profissional arg)
        {
            return new BasicViewModel { Codigo = arg.CNES?.Trim(), Descricao = arg.Unidade?.Trim(), Modelo = "CNES" };
        }

        // ReSharper disable once InconsistentNaming
        internal static BasicViewModel GetCBO(VW_Profissional arg)
        {
            return new BasicViewModel { Codigo = arg.CBO?.Trim(), Descricao = arg.Profissao?.Trim(), Modelo = "CBO" };
        }

        internal void Append(VW_Profissional prof)
        {
            if (CBOs.All(x => x.Codigo?.Trim() != prof.CBO?.Trim()))
                CBOs.Add(GetCBO(prof));

            if (CNESs.All(x => x.Codigo?.Trim() != prof.CNES?.Trim()))
                CNESs.Add(GetCNES(prof));

            if (INEs.All(x => x.Codigo?.Trim() != prof.INE?.Trim()))
                INEs.Add(GetINE(prof));
        }
    }

    /// <summary>
    /// ViewModel de Dados básicos no padrão e-SUS
    /// </summary>
    public class BasicViewModel
    {
        /// <summary>
        /// Nome do modelo de dados ou estrutura do modelo
        /// </summary>
        public string Modelo { get; set; }

        /// <summary>
        /// Código e-SUS
        /// </summary>
        public string Codigo { get; set; }

        /// <summary>
        /// Descrição do item
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Observações do item
        /// </summary>
        public string Observacao { get; set; }
    }
}
