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

namespace Softpark.WS.Controllers.Api
{
    /// <summary>
    /// Common datasets controller
    /// </summary>
    [System.Web.Mvc.OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
    [System.Web.Mvc.SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class DatabaseSupplyController : BaseApiController
    {
        /// <summary>
        /// Endpoint para download de dados básicos para carga de trabalho
        /// </summary>
        /// <param name="modelo">O nome da view model que deseja consultar.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/dados/{modelo}", Name = "BasicSupplyAction")]
        [ResponseType(typeof(BasicViewModel[]))]
        public async Task<IHttpActionResult> GetEntities([FromUri, Required] string modelo)
        {
            IEnumerable<BasicViewModel> models = new BasicViewModel[0];

            using (var Domain = Repository)
            {
                var model = Task.FromResult(models);

                switch (modelo.ToLowerInvariant())
                {
                    case "doencacardiaca":
                        model = Domain.GetModel(c => c.TP_Doenca_Cardiaca, r => r
                            .Where(x => x.ativo == 1).Select(x => new BasicViewModel
                            {
                                Modelo = "DoencaCardiaca",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "doencarespiratoria":
                        model = Domain.GetModel(c => c.TP_Doenca_Respiratoria, r => r
                            .Where(x => x.ativo == 1).Select(x => new BasicViewModel
                            {
                                Modelo = "DoencaRespiratoria",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "formadeescoamentodobanheiroousanitario":
                        model = Domain.GetModel(c => c.TP_Escoamento_Esgoto, r => r
                            .Where(x => x.ativo == 1).Select(x => new BasicViewModel
                            {
                                Modelo = "FormaDeEscoamentoDoBanheiroOuSanitario",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "problemarins":
                        model = Domain.GetModel(c => c.TP_Doenca_Renal, r => r
                            .Where(x => x.ativo == 1).Select(x => new BasicViewModel
                            {
                                Modelo = "ProblemaRins",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "consideracaopeso":
                        model = Domain.GetModel(c => c.TP_Consideracao_Peso, r => r
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "ConsideracaoPeso",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = x.observacao
                            }));
                        break;
                    case "acessohigiene":
                        model = Domain.GetModel(c => c.TP_Higiene_Pessoal, r => r
                            .Where(x => x.ativo == 1).Select(x => new BasicViewModel
                            {
                                Modelo = "AcessoHigiene",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "origemalimentacao":
                        model = Domain.GetModel(c => c.TP_Origem_Alimentacao, r => r
                            .Where(x => x.ativo == 1).Select(x => new BasicViewModel
                            {
                                Modelo = "OrigemAlimentacao",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "quantasvezesalimentacao":
                        model = Domain.GetModel(c => c.TP_Quantas_Vezes_Alimentacao, r => r
                            //.Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "QuantasVezesAlimentacao",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = x.observacao
                            }));
                        break;
                    case "temposituacaoderua":
                        model = Domain.GetModel(c => c.TP_Sit_Rua, r => r
                            .Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "TempoSituacaoDeRua",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "municipios":
                        model = Domain.GetModel(c => c.Cidade, r => r
                            .Where(x => x.CodIbge != null && x.UF != null)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "Municipios",
                                Codigo = x.CodIbge.ToString(),
                                Descricao = x.NomeCidade,
                                Observacao = x.UF
                            }));
                        break;
                    case "nacionalidade":
                        model = Domain.GetModel(c => c.TP_Nacionalidade, r => r
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "Nacionalidade",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = x.observacao
                            }));
                        break;
                    case "pais":
                        model = Domain.GetModel(c => c.Paises, r => r
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "Pais",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.nome,
                                Observacao = null
                            }));
                        break;
                    case "racacor":
                        model = Domain.GetModel(c => c.TP_Raca_Cor, r => r
                            .Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "RacaCor",
                                Codigo = x.id_tp_raca_cor.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "sexo":
                        model = Domain.GetModel(c => c.TP_Sexo, r => r
                            //.Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "Sexo",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = x.observacao
                            }));
                        break;
                    case "etnia":
                        model = Domain.GetModel(c => c.Etnia, r => r
                            //.Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "Etnia",
                                Codigo = x.CodEtnia.ToString(),
                                Descricao = x.DesEtnia,
                                Observacao = null
                            }));
                        break;
                    case "deficienciacidadao":
                        model = Domain.GetModel(c => c.TP_Deficiencia, r => r
                            .Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "DeficienciaCidadao",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "cursomaiselevado":
                        model = Domain.GetModel(c => c.TP_Curso, r => r
                            .Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "CursoMaisElevado",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "cbo":
                        model = Domain.GetModel(c => c.AS_ProfissoesTab, r => r
                            //.Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "CBO",
                                Codigo = x.CodProfTab.ToString(),
                                Descricao = x.DesProfTab,
                                Observacao = null
                            }));
                        break;
                    case "orientacaosexual":
                        model = Domain.GetModel(c => c.TP_Orientacao_Sexual, r => r
                            //.Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "OrientacaoSexual",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = x.observacoes
                            }));
                        break;
                    case "relacaoparentesco":
                        model = Domain.GetModel(c => c.TP_Relacao_Parentesco, r => r
                            //.Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "RelacaoParentesco",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = x.observacoes
                            }));
                        break;
                    case "situacaomercadotrabalho":
                        model = Domain.GetModel(c => c.TP_Sit_Mercado, r => r
                            .Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "SituacaoMercadoTrabalho",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "identidadegenerocidadao":
                        model = Domain.GetModel(c => c.TP_Identidade_Genero_Cidadao, r => r
                            //.Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "identidadeGeneroCidadao",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = x.observacao
                            }));
                        break;
                    case "responsavelcrianca":
                        model = Domain.GetModel(c => c.TP_Crianca, r => r
                            .Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "ResponsavelCrianca",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "motivosaida":
                        model = Domain.GetModel(c => c.TP_Motivo_Saida, r => r
                            //.Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "MotivoSaida",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = x.observacao
                            }));
                        break;
                    case "cnes":
                        model = Domain.GetModel(c => (from p in c.AS_SetoresPar
                                                      let s = p.Setores
                                                      where p.CNES != null && p.CNES.Trim().Length > 0
                                                      select new { p, s }).ToListAsync(), r => r
                                                      .Select(x => new BasicViewModel
                                                      {
                                                          Modelo = "CNES",
                                                          Codigo = x.p.CNES,
                                                          Descricao = x.s.DesSetor,
                                                          Observacao = x.s.DesSetorRes
                                                      }));
                        break;
                    case "ine":
                        model = Domain.GetModel(c => c.SetoresINEs, r => r
                            .Where(x => x.Numero != null)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "INE",
                                Codigo = x.Numero,
                                Descricao = x.Descricao,
                                Observacao = null
                            }));
                        break;
                    case "animalnodomicilio":
                        model = Domain.GetModel(c => c.TP_Animais, r => r
                            .Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "AnimalNoDomicilio",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "abastecimentodeagua":
                        model = Domain.GetModel(c => c.TP_Abastecimento_Agua, r => r
                            .Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "AbastecimentoDeAgua",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "condicaodeposseeusodaterra":
                        model = Domain.GetModel(c => c.TP_Cond_Posse_Uso_Terra, r => r
                            //.Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "CondicaoDePosseEUsoDaTerra",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = x.observacoes
                            }));
                        break;
                    case "destinodolixo":
                        model = Domain.GetModel(c => c.TP_Destino_Lixo, r => r
                            .Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "DestinoDoLixo",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "localizacaodamoradia":
                        model = Domain.GetModel(c => c.TP_Localizacao, r => r
                            .Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "LocalizacaoDaMoradia",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "materialpredominantenaconstrucao":
                        model = Domain.GetModel(c => c.TP_Construcao_Domicilio, r => r
                            .Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "MaterialPredominanteNaConstrucao",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "situacaodemoradia":
                        model = Domain.GetModel(c => c.TP_Situacao_Moradia, r => r
                            .Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "SituacaoDeMoradia",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "tipodeacessoaodomicilio":
                        model = Domain.GetModel(c => c.TP_Acesso_Domicilio, r => r
                            .Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "TipoDeAcessoAoDomicilio",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "tipodedomicilio":
                        model = Domain.GetModel(c => c.TP_Domicilio, r => r
                            .Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "TipoDeDomicilio",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "aguaconsumodomicilio":
                        model = Domain.GetModel(c => c.TP_Tratamento_Agua, r => r
                            .Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "AguaConsumoDomicilio",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "uf":
                        var n = 1;

                        model = Domain.GetModel(c => c.UF, r => r
                            .OrderBy(x => x.DesUF)
                            //.Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "UF",
                                Codigo = (n++).ToString().PadLeft(2, '0'),
                                Descricao = x.DesUF,
                                Observacao = x.UF1
                            }));
                        break;
                    case "tipodelogradouro":
                        model = Domain.GetModel(c => c.TB_MS_TIPO_LOGRADOURO, r => r
                            //.Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "TipoDeLogradouro",
                                Codigo = x.CO_TIPO_LOGRADOURO.ToString(),
                                Descricao = x.DS_TIPO_LOGRADOURO,
                                Observacao = x.DS_TIPO_LOGRADOURO_ABREV
                            }));
                        break;
                    case "rendafamiliar":
                        model = Domain.GetModel(c => c.TP_Renda_Familiar, r => r
                            //.Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "RendaFamiliar",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = x.observacoes
                            }));
                        break;
                    case "tipodeimovel":
                        model = Domain.GetModel(c => c.TP_Imovel, r => r
                            //.Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "tipoDeImovel",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.descricao,
                                Observacao = null
                            }));
                        break;
                    case "turno":
                        models = new[] { new BasicViewModel
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
                        model = Domain.GetModel(c => c.SIGSM_MotivoVisita, r => r
                            //.Where(x => x.ativo == 1)
                            .Select(x => new BasicViewModel
                            {
                                Modelo = "MotivoVisita",
                                Codigo = x.codigo.ToString(),
                                Descricao = x.nome,
                                Observacao = x.observacoes
                            }));
                        break;
                    case "desfecho":
                        models = new[] { new BasicViewModel
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

                if (models.Count() == 0)
                {
                    await Task.WhenAll(model);

                    models = await model;
                }
            }

            return Ok(models.ToArray());
        }

        /// <summary>
        /// Endpoint para download de dados dos profissionais
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/dados/profissional", Name = "ProfessionalSupplyAction")]
        [ResponseType(typeof(ProfissionalViewModel[]))]
        public async Task<IHttpActionResult> GetProfissionais()
        {
            var results = new ProfissionalViewModel[0];

            using (var Domain = Repository)
            {
                var profs = Domain.GetProfissionais();

                await Task.WhenAll(profs);

                results = await profs;
            }

            return Ok(results);
        }

        /// <summary>
        /// Endpoint para listar os modelos de dados consultáveis
        /// </summary>
        /// <returns></returns>
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
                new BasicViewModel { Modelo = "ProfissionalViewModel", Codigo = "Profissional", Descricao = "Profissional", Observacao = "/api/dados/profissional" }
            }.OrderBy(x => x.Codigo).ToArray());
        }
        
        /// <summary>
        /// Buscar pacientes atendidos pelo profissional informado
        /// </summary>
        /// <param name="token">Token de acesso</param>
        /// <param name="microarea"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/dados/paciente/{token:guid}", Name = "PacienteSupplyAction")]
        [ResponseType(typeof(GetCadastroIndividualViewModel[]))]
        public async Task<IHttpActionResult> GetPacientes([FromUri, Required] Guid token, [FromUri] string microarea = null)
        {
            try
            {
                Log.Info("-----");
                Log.Info($"GET api/dados/paciente/{token}");

                var results = new GetCadastroIndividualViewModel[0];

                using (var Domain = Repository)
                {

                    var res = Domain.GetPacientes(token, microarea);

                    await Task.WhenAll(res);

                    results = await res;
                }

                return Ok(results);
            }
            catch (Exception e)
            {
                Log.Fatal(e.Message, e);

                return InternalServerError(e);
            }
        }

        /// <summary>
        /// Buscar domicílios atendidos pelo profissional informado
        /// </summary>
        /// <param name="token">Token de acesso</param>
        /// <param name="microarea"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/dados/domicilio/{token:guid}", Name = "DomicilioSupplyAction")]
        [ResponseType(typeof(GetCadastroDomiciliarViewModel[]))]
        public async Task<IHttpActionResult> GetDomicilios([FromUri, Required] Guid token, [FromUri] string microarea = null)
        {
            try
            {
                Log.Info("-----");
                Log.Info($"GET api/dados/domicilio/{token}");

                var results = new GetCadastroDomiciliarViewModel[0];

                using (var Domain = Repository)
                {
                    var res = Domain.GetDomicilios(token, microarea);

                    await Task.WhenAll(res);

                    results = await res;
                }

                return Ok(results);
            }
            catch (Exception e)
            {
                Log.Fatal(e.Message, e);

                return InternalServerError(e);
            }
        }

        /// <summary>
        /// Buscar visitas realizadas pelo profissional informado
        /// </summary>
        /// <param name="token">Token de acesso</param>
        /// <param name="microarea"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/dados/visita/{token:guid}", Name = "VisitaSupplyAction")]
        [ResponseType(typeof(FichaVisitaDomiciliarChildCadastroViewModel[]))]
        public async Task<IHttpActionResult> GetVisitas([FromUri, Required] Guid token, [FromUri] string microarea = null)
        {
            try
            {
                Log.Info("-----");
                Log.Info($"GET api/dados/visita/{token}");

                var results = new FichaVisitaDomiciliarChildCadastroViewModel[0];

                using (var Domain = Repository)
                {
                    var res = Domain.GetVisitas(token, microarea);

                    await Task.WhenAll(res);

                    results = await res;
                }

                return Ok(results);
            }
            catch (Exception e)
            {
                Log.Fatal(e.Message, e);

                return InternalServerError(e);
            }
        }
    }

    /// <summary>
    /// Profissional
    /// </summary>
    public class ProfissionalViewModel
    {
        /// <summary>
        /// CBO
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public List<BasicViewModel> CBOs { get; set; } = new List<BasicViewModel>();

        /// <summary>
        /// CNES
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public List<BasicViewModel> CNESs { get; set; } = new List<BasicViewModel>();

        /// <summary>
        /// INE
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public List<BasicViewModel> INEs { get; set; } = new List<BasicViewModel>();

        /// <summary>
        /// CNS
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string CNS { get; set; }

        /// <summary>
        /// Nome
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
    /// Dados básicos no padrão e-SUS
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
