﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;
using Softpark.Models;
using Softpark.WS.ViewModels;
using Softpark.WS.Validators;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Softpark.WS.Controllers.Api
{
    /// <summary>
    /// Common datasets controller
    /// </summary>
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
        [EnableQuery]
        public IHttpActionResult GetEntities([FromUri, Required] string modelo)
        {
            List<BasicViewModel> model = new List<BasicViewModel>();

            switch (modelo.ToLowerInvariant())
            {
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
                        //.Where(x => x.ativo == 1)
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
                    model = Domain.Paises
                        .Select(x => new BasicViewModel
                        {
                            Modelo = "Pais",
                            Codigo = x.codigo.ToString(),
                            Descricao = x.nome,
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
                        //.Where(x => x.ativo == 1)
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
                        //.Where(x => x.ativo == 1)
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
                        //.Where(x => x.ativo == 1)
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
                        //.Where(x => x.ativo == 1)
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
                        //.Where(x => x.ativo == 1)
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
                        //.Where(x => x.ativo == 1)
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
                        //.Where(x => x.ativo == 1)
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
                        //.Where(x => x.ativo == 1)
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
                    int n = 1;
                    Func<UF, BasicViewModel>
                        f = (x) => new BasicViewModel
                        {
                            Modelo = "UF",
                            Codigo = (n++).ToString().PadLeft(2, '0'),
                            Descricao = x.DesUF,
                            Observacao = x.UF1
                        };

                    model = Domain.UF.ToList().OrderBy(x => x.DesUF)
                        //.Where(x => x.ativo == 1)
                        .Select(f).ToList();
                    break;
                case "tipodelogradouro":
                    model = Domain.TB_MS_TIPO_LOGRADOURO
                        //.Where(x => x.ativo == 1)
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
                        //.Where(x => x.ativo == 1)
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
                        //.Where(x => x.ativo == 1)
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
                        //.Where(x => x.ativo == 1)
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
                        } }.ToList();
                    break;
                default:
                    throw new ArgumentException("O modelo solicitado é inválido.", nameof(modelo));
            }

            return Ok(model.ToArray());
        }

        /// <summary>
        /// Endpoint para download de dados dos profissionais
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/dados/profissional", Name = "ProfessionalSupplyAction")]
        [ResponseType(typeof(ProfissionalViewModel[]))]
        [EnableQuery]
        public IHttpActionResult GetProfissionais()
        {
            var profs = Domain.VW_Profissional.ToList();

            var ps = new Dictionary<string, ProfissionalViewModel>();

            Func<VW_Profissional, ProfissionalViewModel> __ = (prof) =>
            {
                var _ = new ProfissionalViewModel();
                ps.Add(prof.CNS?.Trim(), _);
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
        /// <returns></returns>
        [HttpGet]
        [Route("api/dados/modelos", Name = "ModelSupplyAction")]
        [ResponseType(typeof(BasicViewModel[]))]
        [EnableQuery]
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
        /// <param name="cns">CNS do Profissional</param>
        /// <param name="token">Token de acesso</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/dados/paciente/{cns}/{token:guid}", Name = "PacienteSupplyAction")]
        [ResponseType(typeof(CadastroIndividualViewModel[]))]
        public async Task<IHttpActionResult> GetPacientes([FromUri, Required(AllowEmptyStrings = false), CnsValidation] string cns, [FromUri, Required] Guid token)
        {
            var headerToken = Domain.UnicaLotacaoTransport.FindAsync(token);

            if(await Domain.UnicaLotacaoTransport.AllAsync(u => u.token != token))
                return BadRequest("Token Inválido.");

            var headers = Domain.UnicaLotacaoTransport.Where(u => u.profissionalCNS == cns)
                .SelectMany(x => x.CadastroIndividual).ToArray();

            CadastroIndividualViewModelCollection results = headers;

            return Ok(results.ToArray());
        }

        /// <summary>
        /// Buscar domicílios atendidos pelo profissional informado
        /// </summary>
        /// <param name="cns">CNS do Profissional</param>
        /// <param name="token">Token de acesso</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/dados/domicilio/{cns}/{token:guid}", Name = "DomicilioSupplyAction")]
        [ResponseType(typeof(CadastroDomiciliarViewModel[]))]
        public async Task<IHttpActionResult> GetDomicilios([FromUri, Required(AllowEmptyStrings = false), CnsValidation] string cns, [FromUri, Required] Guid token)
        {
            var headerToken = Domain.UnicaLotacaoTransport.FindAsync(token);

            if (await Domain.UnicaLotacaoTransport.AllAsync(u => u.token != token))
                return BadRequest("Token Inválido.");

            var headers = Domain.UnicaLotacaoTransport.Where(u => u.profissionalCNS == cns)
                .SelectMany(x => x.CadastroDomiciliar).ToArray();

            CadastroDomiciliarViewModelCollection results = headers;

            return Ok(results.ToArray());
        }

        /// <summary>
        /// Buscar visitas realizadas pelo profissional informado
        /// </summary>
        /// <param name="cns">CNS do Profissional</param>
        /// <param name="token">Token de acesso</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/dados/visita/{cns}/{token:guid}", Name = "VisitaSupplyAction")]
        [ResponseType(typeof(FichaVisitaDomiciliarChildCadastroViewModel[]))]
        public async Task<IHttpActionResult> GetVisitas([FromUri, Required(AllowEmptyStrings = false), CnsValidation] string cns, [FromUri, Required] Guid token)
        {
            var headerToken = Domain.UnicaLotacaoTransport.FindAsync(token);

            if (await Domain.UnicaLotacaoTransport.AllAsync(u => u.token != token))
                return BadRequest("Token Inválido.");

            var headers = Domain.UnicaLotacaoTransport.Where(u => u.profissionalCNS == cns)
                .SelectMany(x => x.FichaVisitaDomiciliarMaster.SelectMany(f => f.FichaVisitaDomiciliarChild)).ToArray();

            FichaVisitaDomiciliarChildCadastroViewModelCollection results = headers;

            return Ok(results.ToArray());
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
        public List<BasicViewModel> CBOs { get; set; } = new List<BasicViewModel>();

        /// <summary>
        /// CNES
        /// </summary>
        public List<BasicViewModel> CNESs { get; set; } = new List<BasicViewModel>();

        /// <summary>
        /// INE
        /// </summary>
        public List<BasicViewModel> INEs { get; set; } = new List<BasicViewModel>();

        /// <summary>
        /// CNS
        /// </summary>
        public string CNS { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        public string Nome { get; set; }

        internal static BasicViewModel GetINE(VW_Profissional arg)
        {
            return new BasicViewModel { Codigo = arg.INE?.Trim(), Descricao = arg.Equipe?.Trim(), Modelo = "INE" };
        }

        internal static BasicViewModel GetCNES(VW_Profissional arg)
        {
            return new BasicViewModel { Codigo = arg.CNES?.Trim(), Descricao = arg.Unidade?.Trim(), Modelo = "CNES" };
        }

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
