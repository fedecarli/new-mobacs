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
    [RoutePrefix("api/dados")]
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
        [Route("version")]
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
        [Route("epoch", Name = "GetEpochURI")]
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
        /// <param name="token"></param>
        /// <returns>Retorna uma coleção de dados básicos</returns>
        [HttpGet]
        [Route("buscar/{modelo}/{token:guid}", Name = "BasicSupplyAction")]
        [ResponseType(typeof(BasicViewModel[]))]
        public async Task<IHttpActionResult> GetEntities([FromUri, Required] string modelo, [FromUri, Required] Guid token)
        {
            var acesso = await BuscarAcesso(token);

            if (acesso == null)
            {
                ModelState.AddModelError(nameof(token), "Token Inválido.");
                return BadRequest(ModelState);
            }

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
        /// Endpoint para buscar informações do acesso
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Retorna dados sobre a sessão baseada em token</returns>
        [HttpGet]
        [Route("acesso/{token:guid}", Name = "AccessTokenSupplyAction")]
        [ResponseType(typeof(DadosAcessoViewModel))]
        public async Task<IHttpActionResult> GetAccessTokenInfo(Guid token)
        {
            var acesso = await BuscarAcesso(token);

            if (acesso == null)
            {
                return BadRequest("Token Inválido ou Expirado!");
            }

            var cfg = await Domain.SIGSM_ServicoSerializador_Config.FindAsync("limiteHorasToken");

            var horas = Convert.ToInt32(cfg?.Valor ?? (2 * 24).ToString());

            var dadosAcesso = new DadosAcessoViewModel
            {
                TokenAcesso = token,
                DadosAtrelados = acesso.profissional,
                ValidoAte = acesso.acs.DtUltVer.Value.AddHours(horas).AddMilliseconds(-1)
            };

            // retorna o token gerado
            return Ok(dadosAcesso);
        }

        /// <summary>
        /// Endpoint para download de dados dos profissionais
        /// </summary>
        /// <returns>Coleção com os dados dos profissionais</returns>
        [HttpGet]
        [Route("profissional/{token:guid}", Name = "ProfessionalSupplyAction")]
        [ResponseType(typeof(ProfissionalViewModel[]))]
        public async Task<IHttpActionResult> GetProfissionais(Guid token)
        {
            var acesso = await BuscarAcesso(token);

            if (acesso == null)
            {
                ModelState.AddModelError(nameof(token), "Token Inválido.");
                return BadRequest(ModelState);
            }

            var profs = await Domain.VW_Profissional.ToListAsync();

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
        [Route("modelos/{token:guid}", Name = "ModelSupplyAction")]
        [ResponseType(typeof(BasicViewModel[]))]
        public async Task<IHttpActionResult> GetModels(Guid token)
        {
            var acesso = await BuscarAcesso(token);

            if (acesso == null)
            {
                ModelState.AddModelError(nameof(token), "Token Inválido.");
                return BadRequest(ModelState);
            }

            return Ok(new[] {
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "DoencaCardiaca", Descricao = "DoencaCardiaca", Observacao = $"/api/dados/buscar/DoencaCardiaca/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "DoencaRespiratoria", Descricao = "DoencaRespiratoria", Observacao = $"/api/dados/buscar/DoencaRespiratoria/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "FormaDeEscoamentoDoBanheiroOuSanitario", Descricao = "FormaDeEscoamentoDoBanheiroOuSanitario", Observacao = $"/api/dados/buscar/FormaDeEscoamentoDoBanheiroOuSanitario/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "ProblemaRins", Descricao = "ProblemaRins", Observacao = $"/api/dados/buscar/ProblemaRins/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "ConsideracaoPeso", Descricao = "ConsideracaoPeso", Observacao = $"/api/dados/buscar/ConsideracaoPeso/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "AcessoHigiene", Descricao = "AcessoHigiene", Observacao = $"/api/dados/buscar/AcessoHigiene/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "OrigemAlimentacao", Descricao = "OrigemAlimentacao", Observacao = $"/api/dados/buscar/OrigemAlimentacao/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "QuantasVezesAlimentacao", Descricao = "QuantasVezesAlimentacao", Observacao = $"/api/dados/buscar/QuantasVezesAlimentacao/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "TempoSituacaoDeRua", Descricao = "TempoSituacaoDeRua", Observacao = $"/api/dados/buscar/TempoSituacaoDeRua/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "Municipios", Descricao = "Municipios", Observacao = $"/api/dados/buscar/Municipios/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "Nacionalidade", Descricao = "Nacionalidade", Observacao = $"/api/dados/buscar/Nacionalidade/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "Pais", Descricao = "Pais", Observacao = $"/api/dados/buscar/Pais/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "RacaCor", Descricao = "RacaCor", Observacao = $"/api/dados/buscar/RacaCor/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "Sexo", Descricao = "Sexo", Observacao = $"/api/dados/buscar/Sexo/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "Etnia", Descricao = "Etnia", Observacao = $"/api/dados/buscar/Etnia/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "DeficienciaCidadao", Descricao = "DeficienciaCidadao", Observacao = $"/api/dados/buscar/DeficienciaCidadao/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "CursoMaisElevado", Descricao = "CursoMaisElevado", Observacao = $"/api/dados/buscar/CursoMaisElevado/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "CBO", Descricao = "CBO", Observacao = $"/api/dados/buscar/CBO/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "OrientacaoSexual", Descricao = "OrientacaoSexual", Observacao = $"/api/dados/buscar/OrientacaoSexual/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "RelacaoParentesco", Descricao = "RelacaoParentesco", Observacao = $"/api/dados/buscar/RelacaoParentesco/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "SituacaoMercadoTrabalho", Descricao = "SituacaoMercadoTrabalho", Observacao = $"/api/dados/buscar/SituacaoMercadoTrabalho/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "identidadeGeneroCidadao", Descricao = "identidadeGeneroCidadao", Observacao = $"/api/dados/buscar/identidadeGeneroCidadao/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "ResponsavelCrianca", Descricao = "ResponsavelCrianca", Observacao = $"/api/dados/buscar/ResponsavelCrianca/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "MotivoSaida", Descricao = "MotivoSaida", Observacao = $"/api/dados/buscar/MotivoSaida/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "CNES", Descricao = "CNES", Observacao = $"/api/dados/buscar/CNES/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "INE", Descricao = "INE", Observacao = $"/api/dados/buscar/INE/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "AnimalNoDomicilio", Descricao = "AnimalNoDomicilio", Observacao = $"/api/dados/buscar/AnimalNoDomicilio/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "AbastecimentoDeAgua", Descricao = "AbastecimentoDeAgua", Observacao = $"/api/dados/buscar/AbastecimentoDeAgua/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "CondicaoDePosseEUsoDaTerra", Descricao = "CondicaoDePosseEUsoDaTerra", Observacao = $"/api/dados/buscar/CondicaoDePosseEUsoDaTerra/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "DestinoDoLixo", Descricao = "DestinoDoLixo", Observacao = $"/api/dados/buscar/DestinoDoLixo/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "LocalizacaoDaMoradia", Descricao = "LocalizacaoDaMoradia", Observacao = $"/api/dados/buscar/LocalizacaoDaMoradia/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "MaterialPredominanteNaConstrucao", Descricao = "MaterialPredominanteNaConstrucao", Observacao = $"/api/dados/buscar/MaterialPredominanteNaConstrucao/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "SituacaoDeMoradia", Descricao = "SituacaoDeMoradia", Observacao = $"/api/dados/buscar/SituacaoDeMoradia/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "TipoDeAcessoAoDomicilio", Descricao = "TipoDeAcessoAoDomicilio", Observacao = $"/api/dados/buscar/TipoDeAcessoAoDomicilio/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "TipoDeDomicilio", Descricao = "TipoDeDomicilio", Observacao = $"/api/dados/buscar/TipoDeDomicilio/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "AguaConsumoDomicilio", Descricao = "AguaConsumoDomicilio", Observacao = $"/api/dados/buscar/AguaConsumoDomicilio/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "UF", Descricao = "UF", Observacao = $"/api/dados/buscar/UF/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "TipoDeLogradouro", Descricao = "TipoDeLogradouro", Observacao = $"/api/dados/buscar/TipoDeLogradouro/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "RendaFamiliar", Descricao = "RendaFamiliar", Observacao = $"/api/dados/buscar/RendaFamiliar/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "tipoDeImovel", Descricao = "tipoDeImovel", Observacao = $"/api/dados/buscar/tipoDeImovel/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "Turno", Descricao = "Turno", Observacao = $"/api/dados/buscar/Turno/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "MotivoVisita", Descricao = "MotivoVisita", Observacao = $"/api/dados/buscar/MotivoVisita/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "Desfecho", Descricao = "Desfecho", Observacao = $"/api/dados/buscar/Desfecho/{token}" },
                new BasicViewModel { Modelo = "BasicViewModel", Codigo = "EstadoCivil", Descricao = "EstadoCivil", Observacao = $"/api/dados/buscar/EstadoCivil/{token}" },
                new BasicViewModel { Modelo = "ProfissionalViewModel", Codigo = "Profissional", Descricao = "Profissional", Observacao = $"/api/dados/buscar/profissional/{token}" }
            }.OrderBy(x => x.Codigo).ToArray());
        }

        private CadastroIndividual Converge(ASSMED_Cadastro cad, CadastroIndividual data, UsuarioVM acesso)
        {
            var prof = acesso.profissional;

            var cns = cad.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 6)?.Numero;
            var rg = cad.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 1);
            var nis = cad.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 7)?.Numero;
            var mun = cad.ASSMED_PesFisica?.MUNICIPIONASC;
            var nac = cad.ASSMED_PesFisica?.Nacionalidade;
            var naca = cad.ASSMED_PesFisica?.CodNacao ?? 10;

            var cont = Domain.ASSMED_Contratos.First();

            var cpf = cad.ASSMED_PesFisica?.CPF?.Trim()?.Replace("([^0-9])", "") ?? "00000000000";

            var iden = data?.IdentificacaoUsuarioCidadao1;

            var header = data?.UnicaLotacaoTransport;
            var nh = header == null;

            var origem = header?.OrigemVisita;
            var nor = origem == null;

            With(ref origem, () => new OrigemVisita
            {
                enviado = false,
                enviarParaThrift = false,
                finalizado = true,
                id_tipo_origem = 2,
                token = Guid.Empty
            }, nor ? new string[0] : new[] {
                nameof(OrigemVisita.enviado),
                nameof(OrigemVisita.enviarParaThrift),
                nameof(OrigemVisita.finalizado),
                nameof(OrigemVisita.id_tipo_origem),
                nameof(OrigemVisita.token),
                nameof(OrigemVisita.UnicaLotacaoTransport)
            });

            With(ref header, () => new UnicaLotacaoTransport
            {
                cnes = prof.CNES.Trim(),
                codigoIbgeMunicipio = cont.CodigoIbgeMunicipio,
                dataAtendimento = DateTime.Now,
                id = Guid.Empty,
                token = origem.token,
                OrigemVisita = origem,
                profissionalCNS = prof.CNS.Trim(),
                cboCodigo_2002 = prof.CBO.Trim()
            }, nh ? new string[0] : new[] {
                nameof(UnicaLotacaoTransport.cnes),
                nameof(UnicaLotacaoTransport.codigoIbgeMunicipio),
                nameof(UnicaLotacaoTransport.cboCodigo_2002),
                nameof(UnicaLotacaoTransport.dataAtendimento),
                nameof(UnicaLotacaoTransport.id),
                nameof(UnicaLotacaoTransport.FichaVisitaDomiciliarMaster),
                nameof(UnicaLotacaoTransport.OrigemVisita),
                nameof(UnicaLotacaoTransport.token),
                nameof(UnicaLotacaoTransport.profissionalCNS),
                nameof(UnicaLotacaoTransport.CadastroDomiciliar),
                nameof(UnicaLotacaoTransport.CadastroIndividual),
                nameof(UnicaLotacaoTransport.DataDeAtendimento),
                nameof(UnicaLotacaoTransport.ine)
            });

            if (nh)
            {
                origem.UnicaLotacaoTransport.Add(header);
                header.OrigemVisita = origem;
            }

            var nd = data == null;
            With(ref data, () => new CadastroIndividual
            {
                CondicoesDeSaude1 = null,
                DataRegistro = DateTime.Now,
                EmSituacaoDeRua1 = null,
                fichaAtualizada = false,
                id = Guid.Empty,
                idAuto = 0,
                InformacoesSocioDemograficas1 = null,
                Justificativa = null,
                SaidaCidadaoCadastro1 = null,
                statusTermoRecusaCadastroIndividualAtencaoBasica = false,
                uuidFichaOriginadora = Guid.Empty,
                tpCdsOrigem = 3,
                UnicaLotacaoTransport = header
            }, nd ? new string[0] : new[] {
                nameof(CadastroIndividual.IdentificacaoUsuarioCidadao1),
                nameof(CadastroIndividual.CondicoesDeSaude1),
                nameof(CadastroIndividual.condicoesDeSaude),
                nameof(CadastroIndividual.CadastroIndividual_recusa),
                nameof(CadastroIndividual.DataRegistro),
                nameof(CadastroIndividual.emSituacaoDeRua),
                nameof(CadastroIndividual.EmSituacaoDeRua1),
                nameof(CadastroIndividual.fichaAtualizada),
                nameof(CadastroIndividual.headerTransport),
                nameof(CadastroIndividual.id),
                nameof(CadastroIndividual.idAuto),
                nameof(CadastroIndividual.identificacaoUsuarioCidadao),
                nameof(CadastroIndividual.IdentificacaoUsuarioCidadao1),
                nameof(CadastroIndividual.informacoesSocioDemograficas),
                nameof(CadastroIndividual.InformacoesSocioDemograficas1),
                nameof(CadastroIndividual.Justificativa),
                nameof(CadastroIndividual.latitude),
                nameof(CadastroIndividual.longitude),
                nameof(CadastroIndividual.saidaCidadaoCadastro),
                nameof(CadastroIndividual.SaidaCidadaoCadastro1),
                nameof(CadastroIndividual.statusTermoRecusaCadastroIndividualAtencaoBasica),
                nameof(CadastroIndividual.tpCdsOrigem),
                nameof(CadastroIndividual.UnicaLotacaoTransport),
                nameof(CadastroIndividual.uuidFichaOriginadora)
            });

            var ni = iden == null;
            With(ref iden, () => new IdentificacaoUsuarioCidadao
            {
                Codigo = cad.Codigo,
                beneficiarioBolsaFamilia = false,
                cnsCidadao = cns,
                cnsResponsavelFamiliar = null,
                codigoIbgeMunicipioNascimento = Domain.Cidade.SingleOrDefault(x => x.CodCidade == mun)?.CodIbge,
                ComplementoRG = rg?.ComplementoRG,
                CPF = cpf.Length == 11 ? cpf : "00000000000",
                dataNascimentoCidadao = cad.ASSMED_PesFisica?.DtNasc,
                desconheceNomeMae = cad.ASSMED_PesFisica?.NomeMae == null,
                desconheceNomePai = cad.ASSMED_PesFisica?.NomePai == null,
                nomeCidadao = cad.Nome,
                nomeMaeCidadao = cad.ASSMED_PesFisica?.NomeMae,
                nomePaiCidadao = cad.ASSMED_PesFisica?.NomePai,
                nomeSocial = cad.NomeSocial,
                dtEntradaBrasil = cad.ASSMED_PesFisica?.ESTRANGEIRADATA,
                dtNaturalizacao = cad.ASSMED_PesFisica?.NATURALIZADADATA,
                emailCidadao = cad.ASSMED_CadEmails.OrderByDescending(x => x.DtSistema).FirstOrDefault()?.EMail,
                EstadoCivil = cad.ASSMED_PesFisica?.EstCivil ?? "I",
                etnia = cad.ASSMED_PesFisica?.CodEtnia > 0 ? cad.ASSMED_PesFisica?.CodEtnia : null,
                id = Guid.Empty,
                microarea = null,
                nacionalidadeCidadao = nac ?? 1,
                paisNascimento = Domain.Nacionalidade.FirstOrDefault(x => x.CodNacao == nac)?.codigo,
                numeroNisPisPasep = cad.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 7)?.Numero,
                num_contrato = 22,
                portariaNaturalizacao = cad.ASSMED_PesFisica?.NATURALIZACAOPORTARIA,
                racaCorCidadao = (int)(cad.ASSMED_PesFisica?.CodCor > 0 ? cad.ASSMED_PesFisica?.CodCor : 6),
                telefoneCelular = cad.ASSMED_CadTelefones.Where(x => x.TipoTel == "C").Select(x => $"{x.DDD}{x.NumTel}".Trim()).FirstOrDefault(),
                RG = rg?.Numero,
                sexoCidadao = cad.ASSMED_PesFisica?.Sexo == "M" ? 0 : cad.ASSMED_PesFisica?.Sexo == "F" ? 1 : 4,
                statusEhResponsavel = true,
                stForaArea = true
            }, ni ? new string[0] : new[] {
                nameof(IdentificacaoUsuarioCidadao.Codigo),
                nameof(IdentificacaoUsuarioCidadao.num_contrato),
                nameof(IdentificacaoUsuarioCidadao.id),
                nameof(IdentificacaoUsuarioCidadao.beneficiarioBolsaFamilia),
                nameof(IdentificacaoUsuarioCidadao.cnsResponsavelFamiliar),
                nameof(IdentificacaoUsuarioCidadao.microarea),
                nameof(IdentificacaoUsuarioCidadao.statusEhResponsavel),
                nameof(IdentificacaoUsuarioCidadao.stForaArea)
            });

            if (ni)
            {
                data.IdentificacaoUsuarioCidadao1 = iden;

                data.IdentificacaoUsuarioCidadao1.CadastroIndividual.Add(data);

                cad.IdFicha = iden.id;
                cad.IdentificacaoUsuarioCidadao = iden;
            }

            data.IdentificacaoUsuarioCidadao1.Codigo = cad.Codigo;
            data.IdentificacaoUsuarioCidadao1.cnsCidadao = cns;
            data.IdentificacaoUsuarioCidadao1.codigoIbgeMunicipioNascimento = Domain.Cidade
                .SingleOrDefault(x => x.CodCidade == mun)?.CodIbge;
            data.IdentificacaoUsuarioCidadao1.ComplementoRG = rg?.ComplementoRG;
            data.IdentificacaoUsuarioCidadao1.CPF = cad.ASSMED_PesFisica?.CPF ?? data.IdentificacaoUsuarioCidadao1.CPF;
            data.IdentificacaoUsuarioCidadao1.dataNascimentoCidadao = cad.ASSMED_PesFisica?.DtNasc;
            data.IdentificacaoUsuarioCidadao1.desconheceNomeMae = cad.ASSMED_PesFisica?.MaeDesconhecida == 1;
            data.IdentificacaoUsuarioCidadao1.desconheceNomePai = cad.ASSMED_PesFisica?.PaiDesconhecido == 1;
            data.IdentificacaoUsuarioCidadao1.nomeCidadao = cad.Nome;
            data.IdentificacaoUsuarioCidadao1.nomeMaeCidadao = cad.ASSMED_PesFisica?.NomeMae;
            data.IdentificacaoUsuarioCidadao1.nomePaiCidadao = cad.ASSMED_PesFisica?.NomePai;
            data.IdentificacaoUsuarioCidadao1.nomeSocial = cad.NomeSocial;
            data.IdentificacaoUsuarioCidadao1.dtEntradaBrasil = cad.ASSMED_PesFisica?.ESTRANGEIRADATA;
            data.IdentificacaoUsuarioCidadao1.dtNaturalizacao = cad.ASSMED_PesFisica?.NATURALIZADADATA;
            data.IdentificacaoUsuarioCidadao1.emailCidadao = cad.ASSMED_CadEmails.OrderByDescending(x => x.DtSistema).FirstOrDefault()?.EMail;
            data.IdentificacaoUsuarioCidadao1.EstadoCivil = cad.ASSMED_PesFisica?.EstCivil ?? "I";
            data.IdentificacaoUsuarioCidadao1.etnia = cad.ASSMED_PesFisica?.CodEtnia;
            data.IdentificacaoUsuarioCidadao1.nacionalidadeCidadao = nac ?? 1;
            data.IdentificacaoUsuarioCidadao1.paisNascimento = Domain.Nacionalidade.FirstOrDefault(x => x.CodNacao == naca)?.codigo;
            data.IdentificacaoUsuarioCidadao1.numeroNisPisPasep = nis;
            data.IdentificacaoUsuarioCidadao1.portariaNaturalizacao = cad.ASSMED_PesFisica?.NATURALIZACAOPORTARIA;
            data.IdentificacaoUsuarioCidadao1.racaCorCidadao = cad.ASSMED_PesFisica?.CodCor ?? 0;
            data.IdentificacaoUsuarioCidadao1.telefoneCelular = cad.ASSMED_CadTelefones.Where(x => x.TipoTel == "C").Select(x => $"{x.DDD}{x.NumTel}".Trim()).FirstOrDefault();
            data.IdentificacaoUsuarioCidadao1.RG = rg?.Numero;
            data.IdentificacaoUsuarioCidadao1.sexoCidadao = cad.ASSMED_PesFisica?.Sexo == "M" ? 0 : cad.ASSMED_PesFisica?.Sexo == "F" ? 1 : 4;

            return data;
        }

        private CadastroDomiciliar Converge(ASSMED_Endereco cad, CadastroDomiciliar data, UsuarioVM acesso)
        {
            var prof = acesso.profissional;
            var cont = Domain.ASSMED_Contratos.First();

            var header = data?.UnicaLotacaoTransport;
            var nh = header == null;

            var origem = header?.OrigemVisita;
            var nor = origem == null;

            With(ref origem, () => new OrigemVisita
            {
                enviado = false,
                enviarParaThrift = false,
                finalizado = true,
                id_tipo_origem = 2,
                token = Guid.Empty
            }, nor ? new string[0] : new[] {
                nameof(OrigemVisita.enviado),
                nameof(OrigemVisita.enviarParaThrift),
                nameof(OrigemVisita.finalizado),
                nameof(OrigemVisita.id_tipo_origem),
                nameof(OrigemVisita.token),
                nameof(OrigemVisita.UnicaLotacaoTransport)
            });

            With(ref header, () => new UnicaLotacaoTransport
            {
                cnes = prof.CNES.Trim(),
                codigoIbgeMunicipio = cont.CodigoIbgeMunicipio,
                dataAtendimento = DateTime.Now,
                id = Guid.Empty,
                token = origem.token,
                OrigemVisita = origem,
                profissionalCNS = prof.CNS.Trim(),
                cboCodigo_2002 = prof.CBO.Trim()
            }, nh ? new string[0] : new[] {
                nameof(UnicaLotacaoTransport.cnes),
                nameof(UnicaLotacaoTransport.codigoIbgeMunicipio),
                nameof(UnicaLotacaoTransport.cboCodigo_2002),
                nameof(UnicaLotacaoTransport.dataAtendimento),
                nameof(UnicaLotacaoTransport.id),
                nameof(UnicaLotacaoTransport.FichaVisitaDomiciliarMaster),
                nameof(UnicaLotacaoTransport.OrigemVisita),
                nameof(UnicaLotacaoTransport.token),
                nameof(UnicaLotacaoTransport.profissionalCNS),
                nameof(UnicaLotacaoTransport.CadastroDomiciliar),
                nameof(UnicaLotacaoTransport.CadastroIndividual),
                nameof(UnicaLotacaoTransport.DataDeAtendimento),
                nameof(UnicaLotacaoTransport.ine)
            });

            if (nh)
            {
                origem.UnicaLotacaoTransport.Add(header);
                header.OrigemVisita = origem;
            }

            var nd = data == null;
            With(ref data, () => new CadastroDomiciliar
            {
                condicaoMoradia = null,
                enderecoLocalPermanencia = null,
                DataRegistro = DateTime.Now,
                fichaAtualizada = false,
                id = Guid.Empty,
                idAuto = 0,
                Justificativa = null,
                uuidFichaOriginadora = Guid.Empty,
                tpCdsOrigem = 3,
                UnicaLotacaoTransport = header,
                statusTermoRecusa = false
            }, nd ? new string[0] : new[] {
                nameof(CadastroDomiciliar.AnimalNoDomicilio),
                nameof(CadastroDomiciliar.CondicaoMoradia1),
                nameof(CadastroDomiciliar.condicaoMoradia),
                nameof(CadastroDomiciliar.DataRegistro),
                nameof(CadastroDomiciliar.enderecoLocalPermanencia),
                nameof(CadastroDomiciliar.EnderecoLocalPermanencia1),
                nameof(CadastroDomiciliar.fichaAtualizada),
                nameof(CadastroDomiciliar.headerTransport),
                nameof(CadastroDomiciliar.id),
                nameof(CadastroDomiciliar.idAuto),
                nameof(CadastroDomiciliar.FamiliaRow),
                nameof(CadastroDomiciliar.instituicaoPermanencia),
                nameof(CadastroDomiciliar.InstituicaoPermanencia1),
                nameof(CadastroDomiciliar.Justificativa),
                nameof(CadastroDomiciliar.latitude),
                nameof(CadastroDomiciliar.longitude),
                nameof(CadastroDomiciliar.quantosAnimaisNoDomicilio),
                nameof(CadastroDomiciliar.stAnimaisNoDomicilio),
                nameof(CadastroDomiciliar.statusTermoRecusa),
                nameof(CadastroDomiciliar.tipoDeImovel),
                nameof(CadastroDomiciliar.tpCdsOrigem),
                nameof(CadastroDomiciliar.TP_Imovel),
                nameof(CadastroDomiciliar.UnicaLotacaoTransport),
                nameof(CadastroDomiciliar.uuidFichaOriginadora)
            });

            var end = data?.EnderecoLocalPermanencia1;

            var contrato = Domain.ASSMED_Contratos.First();
            var mun = Domain.Cidade.SingleOrDefault(x => x.CodIbge == contrato.CodigoIbgeMunicipio);

            var ne = end == null;
            With(ref end, () => new EnderecoLocalPermanencia
            {
                bairro = cad.Bairro,
                cep = cad.CEP?.Replace("-", ""),
                codigoIbgeMunicipio = mun.CodIbge,
                complemento = cad.Complemento,
                nomeLogradouro = cad.Logradouro,
                numero = cad.Numero,
                numeroDneUf = mun.CodDNE?.ToString()?.Trim()?.PadLeft(2, '0'),
                pontoReferencia = cad.ENDREFERENCIA,
                stForaArea = cad.ENDSEMAREA == 1,
                stSemNumero = cad.SEMNUMERO == 1,
                telefoneContato = cad.ASSMED_Cadastro.ASSMED_CadTelefones
                    .Where(x => x.TipoTel == "C").Select(x => $"{x.DDD}{x.NumTel}").FirstOrDefault(),
                telefoneResidencia = cad.ASSMED_Cadastro.ASSMED_CadTelefones
                    .Where(x => x.TipoTel == "R").Select(x => $"{x.DDD}{x.NumTel}").FirstOrDefault(),
                tipoLogradouroNumeroDne = Domain.TB_MS_TIPO_LOGRADOURO.FirstOrDefault(x => x.DS_TIPO_LOGRADOURO_ABREV == cad.TipoEnd)?.CO_TIPO_LOGRADOURO
            }, ne ? new string[0] : new[] {
                nameof(EnderecoLocalPermanencia.bairro),
                nameof(EnderecoLocalPermanencia.cep),
                nameof(EnderecoLocalPermanencia.codigoIbgeMunicipio),
                nameof(EnderecoLocalPermanencia.complemento),
                nameof(EnderecoLocalPermanencia.nomeLogradouro),
                nameof(EnderecoLocalPermanencia.numero),
                nameof(EnderecoLocalPermanencia.numeroDneUf),
                nameof(EnderecoLocalPermanencia.pontoReferencia),
                nameof(EnderecoLocalPermanencia.stForaArea),
                nameof(EnderecoLocalPermanencia.stSemNumero),
                nameof(EnderecoLocalPermanencia.telefoneContato),
                nameof(EnderecoLocalPermanencia.telefoneResidencia),
                nameof(EnderecoLocalPermanencia.tipoLogradouroNumeroDne)
            });

            if (ne)
            {
                var cns = cad.ASSMED_Cadastro.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 6)?.Numero;

                data.EnderecoLocalPermanencia1.bairro = cad.Bairro;
                data.EnderecoLocalPermanencia1.cep = cad.CEP?.Replace("-", "");
                data.EnderecoLocalPermanencia1.codigoIbgeMunicipio = mun.CodIbge;
                data.EnderecoLocalPermanencia1.complemento = cad.Complemento;
                data.EnderecoLocalPermanencia1.nomeLogradouro = cad.Logradouro;
                data.EnderecoLocalPermanencia1.numero = cad.Numero;
                data.EnderecoLocalPermanencia1.numeroDneUf = mun.CodDNE?.ToString()?.Trim()?.PadLeft(2, '0');
                data.EnderecoLocalPermanencia1.pontoReferencia = cad.ENDREFERENCIA;
                data.EnderecoLocalPermanencia1.stForaArea = cad.ENDSEMAREA == 1;
                data.EnderecoLocalPermanencia1.stSemNumero = cad.SEMNUMERO == 1;
                data.EnderecoLocalPermanencia1.telefoneContato = cad.ASSMED_Cadastro.ASSMED_CadTelefones
                        .Where(x => x.TipoTel == "C").Select(x => $"{x.DDD}{x.NumTel}").FirstOrDefault();
                data.EnderecoLocalPermanencia1.telefoneResidencia = cad.ASSMED_Cadastro.ASSMED_CadTelefones
                        .Where(x => x.TipoTel == "R").Select(x => $"{x.DDD}{x.NumTel}").FirstOrDefault();
                data.EnderecoLocalPermanencia1.tipoLogradouroNumeroDne = Domain.TB_MS_TIPO_LOGRADOURO.FirstOrDefault(x => x.DS_TIPO_LOGRADOURO_ABREV == cad.TipoEnd)?.CO_TIPO_LOGRADOURO;
            }

            return data;
        }

        /// <summary>
        /// Endpoint para buscar pacientes atendidos pelo profissional informado
        /// </summary>
        /// <param name="token">Token de acesso</param>
        /// <param name="microarea">microárea</param>
        /// <returns>Coleção de Pacientes</returns>
        [HttpGet]
        [Route("paciente/{token:guid}", Name = "PacienteSupplyAction")]
        [ResponseType(typeof(GetCadastroIndividualViewModel[]))]
        public async Task<IHttpActionResult> GetPacientes([FromUri, Required] Guid token, [FromUri] string microarea = null)
        {
            Log.Info("-----");
            Log.Info($"GET api/dados/paciente/{token}");

            var acesso = await BuscarAcesso(token);

            if (acesso == null)
            {
                ModelState.AddModelError(nameof(token), "Token Inválido.");
                return BadRequest(ModelState);
            }

            var prof = acesso.profissional;

            var d = await (from pca in Domain.ProfCidadaoVincAgendaProd
                           join pcv in Domain.ProfCidadaoVinc
                                    on pca.IdVinc equals pcv.IdVinc
                           join cad in Domain.ASSMED_Cadastro
                                    on (decimal)pcv.IdCidadao equals cad.Codigo
                           let fic = cad.IdentificacaoUsuarioCidadao
                           where pca.AgendamentoMarcado == true
                              && pca.DataCarregado == null
                              && pcv.IdProfissional == acesso.cad.Codigo
                           select new { pca, cad, fic }).ToListAsync();

            CadastroIndividualViewModelCollection results = d.Select(x => Converge(x.cad, x.fic != null ? x.fic.CadastroIndividual.FirstOrDefault() : null, acesso)).ToList();

            if (microarea != null && Regex.IsMatch(microarea, "^([0-9][0-9])$"))
            {
                results = results.Where(r => r.identificacaoUsuarioCidadao?.microarea == null || r.identificacaoUsuarioCidadao.microarea == microarea).ToArray();
            }

            var rs = results.ToArray();

            d.ForEach(x =>
            {
                x.pca.DataCarregado = DateTime.Now;
                x.pca.FichaGerada = true;
            });

            await Domain.SaveChangesAsync();

            var serializer = new JavaScriptSerializer();
            Log.Info(serializer.Serialize(rs));

            return Ok(rs);
        }

        /// <summary>
        /// Endpoint para buscar os domicílios atendidos pelo profissional informado
        /// </summary>
        /// <param name="token">Token de acesso</param>
        /// <param name="microarea">Microárea</param>
        /// <returns>Coleção de domicilios</returns>
        [HttpGet]
        [Route("domicilio/{token:guid}", Name = "DomicilioSupplyAction")]
        [ResponseType(typeof(GetCadastroDomiciliarViewModel[]))]
        public async Task<IHttpActionResult> GetDomicilios([FromUri, Required] Guid token, [FromUri] string microarea = null)
        {
            try
            {
                Log.Info("----");
                Log.Info($"api/dados/domicilio/{token}");

                var acesso = await BuscarAcesso(token);

                if (acesso == null)
                {
                    ModelState.AddModelError(nameof(token), "Token Inválido.");
                    return BadRequest(ModelState);
                }

                var prof = acesso.profissional;

                var domicilios = await (from pcv in Domain.ProfCidadaoVinc
                                        join agd in Domain.ProfCidadaoVincAgendaProd on pcv.IdVinc equals agd.IdVinc
                                        join ass in Domain.ASSMED_Cadastro on (decimal?)pcv.IdCidadao equals ass.Codigo
                                        join end in Domain.ASSMED_Endereco on ass.Codigo equals end.Codigo
                                        let fic = end.EnderecoLocalPermanencia
                                        let ide = ass.IdentificacaoUsuarioCidadao
                                        let cdo = fic != null ? fic.CadastroDomiciliar.FirstOrDefault() : null
                                        let children = fic == null ? 0 : Domain.CadastroDomiciliar.Count(x => x.uuidFichaOriginadora == cdo.id && x.id != cdo.id)
                                        where children == 0
                                           && agd.AgendamentoMarcado == true
                                           && agd.DataCarregadoDomiciliar == null
                                           && pcv.IdProfissional == acesso.cad.Codigo
                                           && (ide == null || ide.cnsResponsavelFamiliar == null)
                                        select new { agd, cdo, end }).ToListAsync();

                CadastroDomiciliarViewModelCollection results = domicilios.Select(x => Converge(x.end, x.cdo, acesso)).ToArray();

                if (microarea != null && Regex.IsMatch(microarea, "^([0-9][0-9])$"))
                {
                    results = results.Where(r => r.enderecoLocalPermanencia?.microarea == null || r.enderecoLocalPermanencia?.microarea == microarea).ToArray();
                }

                domicilios.ForEach(x =>
                {
                    x.agd.DataCarregadoDomiciliar = DateTime.Now;
                    x.agd.FichaDomiciliarGerada = true;
                });

                await Domain.SaveChangesAsync();

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
