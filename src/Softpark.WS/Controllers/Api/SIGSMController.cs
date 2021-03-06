﻿using System;
using System.Linq;
using System.Web.Http;
using Softpark.Models;
using System.Web.Http.Description;
using Softpark.WS.ViewModels.SIGSM;
using DataTables.AspNet.WebApi2;
using DataTables.AspNet.Core;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using static Softpark.Infrastructure.Extensions.WithStatement;
using Softpark.WS.ViewModels;
using Softpark.Infrastructure.Extensions;
using System.Collections.Generic;

namespace Softpark.WS.Controllers.Api
{
    /// <summary>
    /// Controller para chamadas via SIGSM
    /// </summary>
    [RoutePrefix("api/sigsm")]
    [System.Web.Mvc.OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
    [System.Web.Mvc.SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class SIGSMController : BaseApiController
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public SIGSMController() : base(new DomainContainer()) { }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        #region CadastroIndividual
        /// <summary>
        /// Buscar profissionais
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("listar/profissional/{nomeOuCns}/{cnes}")]
        [ResponseType(typeof(VW_Profissional[]))]
        public IHttpActionResult ListarProfissionais([FromUri] string nomeOuCns,
            [FromUri] string cnes = null)
        {
            if (!Autenticado())
            {
                throw new ValidationException("É preciso estar logado.");
            }

            var referer = Request.UrlReferrer();

            var ficha = GetTipoFichaFrom(referer);

            return ListarProfissionais(ficha, nomeOuCns, cnes);
        }

        private string GetTipoFichaFrom(string referer)
        {
            referer = referer.ToLower();
            var fichas = new Dictionary<string, string> {
                { "cadIndividual.asp", "CadastroIndividual" },
                { "domicilioCad.asp", "CadastroDomiciliar" },
                { "atendimentoIndividualCad.asp", "AtendimentoIndividual" },
                { "atendimentoOdontologicoCad.asp", "AtendimentoOdontologico" },
                { "procedimentoCad.asp", "Procedimento" },
                { "atividadeColetivaCad.asp", "AtividadeColetiva" },
                { "marcadoresConsumoAlimentarCad.asp", "Consumo" },
                { "visitaDomiciliar.asp", "VisitaDomiciliar" },
                { "atendimentoDomiciliarCad.asp", "AtendimentoDomiciliar" },
                { "avaliacaoElegibilidadeAdmissaoCad.asp", "AvaliacaoElegibilidade" }
            };

            return fichas.Where(x => referer.ToLower().Contains(x.Key.ToLower()))
                .Select(x => x.Value).FirstOrDefault();
        }

        /// <summary>
        /// Buscar profissionais
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("listar/profissional/{ficha}/{nomeOuCns}/{cnes}")]
        [ResponseType(typeof(VW_Profissional[]))]
        public IHttpActionResult ListarProfissionais([FromUri, Required] string ficha, [FromUri] string nomeOuCns,
            [FromUri] string cnes = null)
        {
            if (!Autenticado())
            {
                throw new ValidationException("É preciso estar logado.");
            }

            ficha = ficha == null || ficha.ToLower() == "todos" ? null : ficha;

            if (cnes != null && cnes.Trim() == "0")
                cnes = null;

            var data = Domain.VW_Profissionais(ficha, cnes, nomeOuCns, 20).ToArray();

            return Ok(data);
        }

        /// <summary>
        /// Pesquisar cadastros assmed
        /// </summary>
        /// <param name="q">Query</param>
        /// <param name="limit">Quantidade de registros</param>
        /// <returns></returns>
        [HttpGet]
        [Route("buscar/assmedCadastro")]
        [ResponseType(typeof(VW_Cadastros[]))]
        public IHttpActionResult BuscarCadastroIndividual(string q, int? limit = 15)
        {
            if (!Autenticado())
            {
                throw new ValidationException("É preciso estar logado.");
            }

            return Ok(Domain.VW_Cadastros(q, limit ?? 15).ToArray());
        }

        /// <summary>
        /// Buscar cadastros individuais
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("listar/CadastroIndividual")]
        [ResponseType(typeof(DataTablesJsonResult))]
        public IHttpActionResult ListarCadastroIndividual(IDataTablesRequest request)
        {
            if (!Autenticado())
            {
                throw new ValidationException("É preciso estar logado.");
            }

            if (request == null) request = new DataTablesRequest
            {
                Draw = 1,
                Length = 10
            };

            var search = request.Search.Value == null ||
                request.Search.Value.Trim().Length == 0 ?
                null : request.Search.Value.Trim();

            var http = System.Web.HttpContext.Current;
            var ordCol = Convert.ToInt32(http.Request.Params["order[0][column]"]);
            var ordDir = http.Request.Params["order[0][dir]"] == "asc" ? 0 : 1;

            // Response creation. To create your response you need to reference your request, to avoid
            // request/response tampering and to ensure response will be correctly created.
            var response = DataTablesResponse.Create(request, 100, 100, Domain.VW_CadastroIndividuais(search, request.Start, request.Length, ordCol, ordDir));

            // Easier way is to return a new 'DataTablesJsonResult', which will automatically convert your
            // response to a json-compatible content, so DataTables can read it when received.
            return new DataTablesJsonResult(response, Request);
        }

        /// <summary>
        /// Detalhar Cadastro Individual
        /// </summary>
        /// <param name="codigo">Código da pessoa em ASSMED_Cadastro</param>
        /// <returns></returns>
        [HttpGet]
        [Route("detalhar/CadastroIndividual/{codigo}")]
        [ResponseType(typeof(FormCadastroIndividual))]
        public async Task<IHttpActionResult> DetalharCadastroIndividual(decimal codigo)
        {
            if (!Autenticado())
            {
                throw new ValidationException("É preciso estar logado.");
            }

            var cad = Domain.ASSMED_Cadastro.SingleOrDefault(x => x.Codigo == codigo);

            if (cad == null)
            {
                throw new ValidationException("Cadastro não encontrado!");
            }

            var pf = await Domain.ASSMED_CadastroPF.FirstOrDefaultAsync(x => x.Codigo == codigo);

            var cns = cad.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 6)?.Numero;
            var rg = cad.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 1);
            var nis = cad.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 7)?.Numero;
            var mun = cad.ASSMED_PesFisica?.MUNICIPIONASC ?? pf?.MUNICIPIONASC;
            var nac = cad.ASSMED_PesFisica?.Nacionalidade ?? pf?.Nacionalidade;
            var naca = cad.ASSMED_PesFisica?.CodNacao ?? pf?.CodNacao ?? 10;

            var ulog = Convert.ToInt32(ASPSessionVar.Read("idUsuario") ?? "0");

            var cset = Convert.ToInt32(ASPSessionVar.Read("idSetor") ?? "0");

            var setor = Domain.AS_SetoresPar.SingleOrDefault(x => x.CodSetor == cset);

            var data = cad.IdentificacaoUsuarioCidadao?.CadastroIndividual.FirstOrDefault();

            var cont = Domain.ASSMED_Contratos.First();

            var cpf = (cad.ASSMED_PesFisica?.CPF ?? pf?.CPF)?.Trim()?.Replace("([^0-9])", "") ?? "";

            var iden = cad.IdentificacaoUsuarioCidadao;

            var header = data?.UnicaLotacaoTransport;
            var nh = header == null;

            var origem = header?.OrigemVisita;
            var nor = origem == null;

            var microarea = cad.ASSMED_Endereco.OrderBy(x => x.ItemEnd).Where(x => x.MicroArea != null).Select(x => x.MicroArea)
                .LastOrDefault() ?? cad.MicroArea;

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
                cnes = setor?.CNES,
                codigoIbgeMunicipio = cont.CodigoIbgeMunicipio,
                dataAtendimento = DateTime.Now,
                id = Guid.Empty,
                token = origem.token,
                OrigemVisita = origem,
                profissionalCNS = string.Empty,
                cboCodigo_2002 = string.Empty
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
                InformacoesSocioDemograficas1 = null,
                SaidaCidadaoCadastro1 = null,
                statusTermoRecusaCadastroIndividualAtencaoBasica = false,
                uuidFichaOriginadora = Guid.Empty,
                tpCdsOrigem = 3,
                UnicaLotacaoTransport = header
            }, nd ? new string[0] : new[] {
                nameof(CadastroIndividual.IdentificacaoUsuarioCidadao1),
                nameof(CadastroIndividual.CondicoesDeSaude1),
                nameof(CadastroIndividual.condicoesDeSaude),
                nameof(CadastroIndividual.DataRegistro),
                nameof(CadastroIndividual.emSituacaoDeRua),
                nameof(CadastroIndividual.EmSituacaoDeRua1),
                nameof(CadastroIndividual.fichaAtualizada),
                nameof(CadastroIndividual.headerTransport),
                nameof(CadastroIndividual.id),
                nameof(CadastroIndividual.identificacaoUsuarioCidadao),
                nameof(CadastroIndividual.IdentificacaoUsuarioCidadao1),
                nameof(CadastroIndividual.informacoesSocioDemograficas),
                nameof(CadastroIndividual.InformacoesSocioDemograficas1),
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
                Codigo = codigo,
                beneficiarioBolsaFamilia = false,
                cnsCidadao = cns,
                cnsResponsavelFamiliar = null,
                codigoIbgeMunicipioNascimento = Domain.Cidade.SingleOrDefault(x => x.CodCidade == mun)?.CodIbge,
                ComplementoRG = rg?.ComplementoRG,
                CPF = cpf.Length == 11 ? cpf : null,
                dataNascimentoCidadao = cad.ASSMED_PesFisica?.DtNasc ?? pf?.DtNasc ?? DateTime.MinValue,
                desconheceNomeMae = cad.ASSMED_PesFisica?.NomeMae == null && pf?.NomeMae == null,
                desconheceNomePai = cad.ASSMED_PesFisica?.NomePai == null && pf?.NomePai == null,
                nomeCidadao = cad.Nome,
                nomeMaeCidadao = cad.ASSMED_PesFisica?.NomeMae ?? pf?.NomeMae,
                nomePaiCidadao = cad.ASSMED_PesFisica?.NomePai ?? pf?.NomePai,
                nomeSocial = cad.NomeSocial,
                dtEntradaBrasil = cad.ASSMED_PesFisica?.ESTRANGEIRADATA ?? pf?.ESTRANGEIRADATA,
                dtNaturalizacao = cad.ASSMED_PesFisica?.NATURALIZADADATA ?? pf?.NATURALIZADADATA,
                emailCidadao = cad.ASSMED_CadEmails.OrderByDescending(x => x.DtSistema).FirstOrDefault()?.EMail,
                EstadoCivil = cad.ASSMED_PesFisica?.EstCivil ?? pf?.EstCivil ?? "I",
                etnia = cad.ASSMED_PesFisica?.CodEtnia > 0 ? cad.ASSMED_PesFisica.CodEtnia :
                pf?.CodEtnia > 0 ? pf.CodEtnia : 0,
                id = Guid.Empty,
                microarea = microarea,
                nacionalidadeCidadao = nac ?? 1,
                paisNascimento = Domain.Nacionalidade.FirstOrDefault(x => x.CodNacao == nac)?.codigo,
                numeroNisPisPasep = cad.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 7)?.Numero,
                num_contrato = 22,
                portariaNaturalizacao = cad.ASSMED_PesFisica?.NATURALIZACAOPORTARIA ?? pf?.NATURALIZACAOPORTARIA,
                racaCorCidadao = (int)(cad.ASSMED_PesFisica?.CodCor > 0 ? cad.ASSMED_PesFisica?.CodCor :
                pf?.CodCor > 0 ? pf.CodCor : 6),
                telefoneCelular = cad.ASSMED_CadTelefones.Where(x => x.TipoTel == "C").Select(x => $"{x.DDD}{x.NumTel}".Trim()).FirstOrDefault(),
                RG = rg?.Numero,
                sexoCidadao = cad.ASSMED_PesFisica?.Sexo == "M" ? 0 : cad.ASSMED_PesFisica?.Sexo == "F" ? 1 :
                pf?.Sexo == "M" ? 0 : pf?.Sexo == "F" ? 1 : 4,
                statusEhResponsavel = true,
                stForaArea = microarea == null
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

            data.IdentificacaoUsuarioCidadao1.Codigo = codigo;
            data.IdentificacaoUsuarioCidadao1.cnsCidadao = cns;
            data.IdentificacaoUsuarioCidadao1.codigoIbgeMunicipioNascimento = Domain.Cidade
                .SingleOrDefault(x => x.CodCidade == mun)?.CodIbge;
            data.IdentificacaoUsuarioCidadao1.ComplementoRG = rg?.ComplementoRG;
            data.IdentificacaoUsuarioCidadao1.CPF = cad.ASSMED_PesFisica?.CPF ?? data.IdentificacaoUsuarioCidadao1.CPF;
            data.IdentificacaoUsuarioCidadao1.dataNascimentoCidadao = cad.ASSMED_PesFisica?.DtNasc ?? pf?.DtNasc ?? DateTime.MaxValue;
            data.IdentificacaoUsuarioCidadao1.desconheceNomeMae = cad.ASSMED_PesFisica?.MaeDesconhecida == 1 || pf?.MaeDesconhecida == 1;
            data.IdentificacaoUsuarioCidadao1.desconheceNomePai = cad.ASSMED_PesFisica?.PaiDesconhecido == 1 || pf?.PaiDesconhecido == 1;
            data.IdentificacaoUsuarioCidadao1.nomeCidadao = cad.Nome;
            data.IdentificacaoUsuarioCidadao1.nomeMaeCidadao = cad.ASSMED_PesFisica?.NomeMae ?? pf?.NomeMae;
            data.IdentificacaoUsuarioCidadao1.nomePaiCidadao = cad.ASSMED_PesFisica?.NomePai ?? pf?.NomePai;
            data.IdentificacaoUsuarioCidadao1.nomeSocial = cad.NomeSocial;
            data.IdentificacaoUsuarioCidadao1.dtEntradaBrasil = cad.ASSMED_PesFisica?.ESTRANGEIRADATA ?? pf?.ESTRANGEIRADATA;
            data.IdentificacaoUsuarioCidadao1.dtNaturalizacao = cad.ASSMED_PesFisica?.NATURALIZADADATA ?? pf?.NATURALIZADADATA;
            data.IdentificacaoUsuarioCidadao1.emailCidadao = cad.ASSMED_CadEmails.OrderByDescending(x => x.DtSistema).FirstOrDefault()?.EMail;
            data.IdentificacaoUsuarioCidadao1.EstadoCivil = cad.ASSMED_PesFisica?.EstCivil ?? pf?.EstCivil ?? "I";
            data.IdentificacaoUsuarioCidadao1.etnia = cad.ASSMED_PesFisica?.CodEtnia ?? pf?.CodEtnia;
            data.IdentificacaoUsuarioCidadao1.nacionalidadeCidadao = nac ?? 1;
            data.IdentificacaoUsuarioCidadao1.paisNascimento = Domain.Nacionalidade.FirstOrDefault(x => x.CodNacao == naca)?.codigo;
            data.IdentificacaoUsuarioCidadao1.numeroNisPisPasep = nis;
            data.IdentificacaoUsuarioCidadao1.portariaNaturalizacao = cad.ASSMED_PesFisica?.NATURALIZACAOPORTARIA ?? pf?.NATURALIZACAOPORTARIA;
            data.IdentificacaoUsuarioCidadao1.racaCorCidadao = cad.ASSMED_PesFisica?.CodCor ?? pf?.CodCor ?? 0;
            data.IdentificacaoUsuarioCidadao1.telefoneCelular = cad.ASSMED_CadTelefones.Where(x => x.TipoTel == "C").Select(x => $"{x.DDD}{x.NumTel}".Trim()).FirstOrDefault();
            data.IdentificacaoUsuarioCidadao1.RG = rg?.Numero;
            data.IdentificacaoUsuarioCidadao1.sexoCidadao = cad.ASSMED_PesFisica?.Sexo == "M" ? 0 : cad.ASSMED_PesFisica?.Sexo == "F" ? 1 :
                pf?.Sexo == "M" ? 0 : pf?.Sexo == "F" ? 1 : 4;

            return Ok((await FormCadastroIndividual.Apply(data, Domain)).ToDetail());
        }

        /// <summary>
        /// Salvar Cadastro Individual
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("salvar/CadastroIndividual")]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> SalvarCadastroIndividual([FromBody] FormCadastroIndividual form)
        {
            if (!Autenticado())
            {
                throw new ValidationException("É preciso estar logado.");
            }

            var id = await form.LimparESalvarDados(Domain, Url);

            return Ok(id);
        }
        #endregion

        #region CadastroDomiciliar
        /// <summary>
        /// Buscar cadastros domiciliares
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("listar/CadastroDomiciliar")]
        [ResponseType(typeof(DataTablesJsonResult))]
        public IHttpActionResult ListarCadastroDomiciliar(IDataTablesRequest request)
        {
            if (!Autenticado())
            {
                throw new ValidationException("É preciso estar logado.");
            }

            if (request == null) request = new DataTablesRequest
            {
                Draw = 1,
                Length = 10
            };

            var http = System.Web.HttpContext.Current;
            var ordCol = Convert.ToInt32(http.Request.Params["order[0][column]"]);
            var ordDir = http.Request.Params["order[0][dir]"] == "asc" ? 0 : 1;

            // Paging filtered data.
            // Paging is rather manual due to in-memmory (IEnumerable) data.
            var dataPage = Domain.VW_CadastroDomiciliares(request.Search.Value, request.Start, request.Length, ordCol, ordDir).ToArray();

            // Response creation. To create your response you need to reference your request, to avoid
            // request/response tampering and to ensure response will be correctly created.
            var response = DataTablesResponse.Create(request, 100, 100, dataPage);

            // Easier way is to return a new 'DataTablesJsonResult', which will automatically convert your
            // response to a json-compatible content, so DataTables can read it when received.
            return new DataTablesJsonResult(response, Request);
        }

        /// <summary>
        /// Detalhar Cadastro Domiciliar
        /// </summary>
        /// <param name="codigo">Código do domicilio em ASSMED_Endereco</param>
        /// <returns></returns>
        [HttpGet]
        [Route("detalhar/CadastroDomiciliar/{codigo}")]
        [ResponseType(typeof(FormCadastroDomiciliar))]
        public async Task<IHttpActionResult> DetalharCadastroDomiciliar(string codigo)
        {
            if (!Autenticado())
            {
                throw new ValidationException("É preciso estar logado.");
            }

            var ulog = Convert.ToInt32(ASPSessionVar.Read("idUsuario") ?? "0");

            var cset = Convert.ToInt32(ASPSessionVar.Read("idSetor") ?? "0");

            var setor = Domain.AS_SetoresPar.SingleOrDefault(x => x.CodSetor == cset);

            ASSMED_Endereco cad = null;

            if (Guid.TryParse(codigo, out Guid uuid))
                cad = await Domain.CadastroDomiciliar.Where(x => x.id == uuid).SelectMany(x => x.EnderecoLocalPermanencia1.ASSMED_Endereco).FirstOrDefaultAsync();
            else if (decimal.TryParse(codigo, out decimal id))
                cad = await Domain.ASSMED_Endereco.OrderByDescending(x => x.ItemEnd).FirstOrDefaultAsync(x => x.Codigo == id);

            if (cad == null)
            {
                throw new ValidationException("Não foi possível encontrar a ficha selecionada.");
            }

            var data = cad.EnderecoLocalPermanencia?.CadastroDomiciliar.FirstOrDefault();

            var header = data?.UnicaLotacaoTransport;
            var nh = header == null;

            var origem = header?.OrigemVisita;
            var nor = origem == null;

            var microarea = cad.MicroArea ?? cad.ASSMED_Cadastro.MicroArea ?? cad.EnderecoLocalPermanencia?.microarea;

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

            var cont = Domain.ASSMED_Contratos.First();

            With(ref header, () => new UnicaLotacaoTransport
            {
                cnes = setor?.CNES,
                codigoIbgeMunicipio = cont.CodigoIbgeMunicipio,
                dataAtendimento = DateTime.Now,
                id = Guid.Empty,
                token = origem.token,
                OrigemVisita = origem,
                profissionalCNS = string.Empty,
                cboCodigo_2002 = string.Empty
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

            var cns = cad.ASSMED_Cadastro.ASSMED_CadastroDocPessoal.Where(x => x.Numero != null && x.CodTpDocP == 6)
                            .LastOrDefault()?.Numero.Trim();

            if (nd)
                data = new CadastroDomiciliar
                {
                    DataRegistro = DateTime.Now,
                    fichaAtualizada = false,
                    id = Guid.Empty,
                    UnicaLotacaoTransport = header,
                    EnderecoLocalPermanencia1 = new EnderecoLocalPermanencia { },
                    FamiliaRow = new List<FamiliaRow>()
                    {
                        new FamiliaRow
                        {
                            dataNascimentoResponsavel = cad.ASSMED_Cadastro.ASSMED_PesFisica?.DtNasc??
                            cad.ASSMED_Cadastro.ASSMED_CadastroPF?.DtNasc,
                            numeroCnsResponsavel = cns,
                            id = Guid.Empty,
                            numeroMembrosFamilia = Domain.IdentificacaoUsuarioCidadao
                                .Where(x => x.cnsResponsavelFamiliar == cns && !x.statusEhResponsavel)
                                .Select(x => x.cnsCidadao).Distinct().Count() + 1
                        }
                    }
                };

            var end = data.EnderecoLocalPermanencia1;

            end.microarea = microarea;
            end.nomeLogradouro = cad.Logradouro;
            end.numero = cad.Numero;

            var cid = Domain.Cidade.Single(x => x.CodIbge != null && x.CodIbge.Trim() == cont.CodigoIbgeMunicipio);

            end.numeroDneUf = Domain.UF.SingleOrDefault(x => x.UF1 == cad.UF || x.UF1 == cid.UF)?.DNE?.Trim();

            var telc = cad.ASSMED_Cadastro.ASSMED_CadTelefones.OrderByDescending(x => x.IDTelefone)
                .LastOrDefault(x => x.TipoTel == "C" && x.NumTel != null);

            var telr = cad.ASSMED_Cadastro.ASSMED_CadTelefones.OrderByDescending(x => x.IDTelefone)
                .LastOrDefault(x => x.TipoTel == "R" && x.NumTel != null);

            end.telefoneContato = telc == null ? null : telc.DDD.ToString() + telc.NumTel;
            end.telefoneResidencia = telr == null ? null : telr.DDD.ToString() + telr.NumTel;

            var tpl = cad.CodTpLogra?.ToString().PadLeft(3, '0');

            var log = Domain.TB_MS_TIPO_LOGRADOURO.FirstOrDefault(x => x.DS_TIPO_LOGRADOURO_ABREV == cad.TipoEnd || x.CO_TIPO_LOGRADOURO == tpl)?.CO_TIPO_LOGRADOURO;

            end.tipoLogradouroNumeroDne = log;

            end.bairro = cad.Bairro;
            end.cep = cad.CEP;

            cid = cad.CodCidade != null ? Domain.Cidade.Single(x => x.CodIbge != null && x.CodCidade == cad.CodCidade) : cid;

            end.codigoIbgeMunicipio = cid?.CodIbge?.Trim();

            return Ok(await (await FormCadastroDomiciliar.ToVM(data, Domain)).ToDetail(Domain));
        }

        /// <summary>
        /// Salvar Cadastro Domniciliar
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("salvar/CadastroDomiciliar")]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> SalvarCadastroDomiciliar([FromBody] FormCadastroDomiciliar form)
        {
            if (!Autenticado())
            {
                throw new ValidationException("É preciso estar logado.");
            }

            var id = await form.LimparESalvarDados(Domain, Url);

            return Ok(id);
        }
        #endregion

        #region VisitaDomiciliar
        /// <summary>
        /// Buscar visitas domiciliares (master)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("listar/VisitaDomiciliar")]
        [ResponseType(typeof(DataTablesJsonResult))]
        public IHttpActionResult ListarVisitaDomiciliar(IDataTablesRequest request)
        {
            if (!Autenticado())
            {
                throw new ValidationException("É preciso estar logado.");
            }

            if (request == null) request = new DataTablesRequest
            {
                Draw = 1,
                Length = 10
            };

            var http = System.Web.HttpContext.Current;
            var ordCol = Convert.ToInt32(http.Request.Params["order[0][column]"] ?? "0");
            var ordDir = http.Request.Params["order[0][dir]"] == "desc" ? 1 : 0;

            string cnes = ASPSessionVar.Read("CNESSetor");

            // Response creation. To create your response you need to reference your request, to avoid
            // request/response tampering and to ensure response will be correctly created.
            var response = DataTablesResponse.Create(request, 100, 100,
                Domain.VW_FichasMasters(request.Search.Value, request.Length < 10 ? 10 : request.Length, request.Start, ordCol, ordDir, cnes));

            // Easier way is to return a new 'DataTablesJsonResult', which will automatically convert your
            // response to a json-compatible content, so DataTables can read it when received.
            return new DataTablesJsonResult(response, Request);
        }

        /// <summary>
        /// Detalhar Visita Domiciliar Master
        /// </summary>
        /// <param name="uuidFicha">Id da Ficha Master</param>
        /// <returns></returns>
        [HttpGet]
        [Route("detalhar/VisitaDomiciliar/{uuidFicha}")]
        [ResponseType(typeof(DetalheFichaVisitaDomiciliarMasterVW))]
        public async Task<IHttpActionResult> DetalharVisitaDomiciliar(string uuidFicha)
        {
            if (!Autenticado())
            {
                throw new ValidationException("É preciso estar logado.");
            }

            var ficha = await Domain.FichaVisitaDomiciliarMaster.FindAsync(uuidFicha);

            if (ficha == null)
                throw new ValidationException("A ficha selecionada não foi encontrada.");

            DetalheFichaVisitaDomiciliarMasterVW vm = ficha;

            return Ok(vm.ToDetail());
        }

        /// <summary>
        /// Detalhar Visita Domiciliar Child
        /// </summary>
        /// <param name="uuidFicha">Id da Ficha Master</param>
        /// <param name="childId">Id da Ficha Child</param>
        /// <returns></returns>
        [HttpGet]
        [Route("detalhar/VisitaDomiciliar/{uuidFicha}/child/{childId:guid}")]
        [ResponseType(typeof(FichaVisitaDomiciliarChildCadastroViewModel))]
        public async Task<IHttpActionResult> DetalharVisitaDomiciliarChild(string uuidFicha, Guid childId)
        {
            if (!Autenticado())
            {
                throw new ValidationException("É preciso estar logado.");
            }

            var ficha = await Domain.FichaVisitaDomiciliarMaster.FindAsync(uuidFicha);

            if (ficha == null)
                throw new ValidationException("A ficha selecionada não foi encontrada.");

            var child = ficha.FichaVisitaDomiciliarChild.SingleOrDefault(x => x.childId == childId);

            if (child == null)
                throw new ValidationException("A ficha selecionada não foi encontrada.");

            FichaVisitaDomiciliarChildCadastroViewModel vm = child;

            return Ok(vm.ToDetail());
        }

        /// <summary>
        /// Criar/Atualizar Ficha de Visita Domiciliar Master
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("salvar/VisitaDomiciliar")]
        [ResponseType(typeof(DetalheFichaVisitaDomiciliarMasterVW))]
        public async Task<IHttpActionResult> SalvarVisitaDomiciliarMaster([FromBody] DetalheFichaVisitaDomiciliarMasterVW vm)
        {
            if (!Autenticado())
            {
                throw new ValidationException("É preciso estar logado.");
            }

            var master = await vm.LimparESalvarDados(Domain, Url);

            DetalheFichaVisitaDomiciliarMasterVW detalhe = master;

            return Ok(detalhe.ToDetail());
        }

        /// <summary>
        /// Criar/Atualizar Ficha de Visita Domiciliar Child
        /// </summary>
        /// <param name="uuidFicha"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("salvar/VisitaDomiciliar/{uuidFicha}/child")]
        [ResponseType(typeof(Guid))]
        public async Task<IHttpActionResult> SalvarVisitaDomiciliarChild([FromUri] string uuidFicha, [FromBody] FichaVisitaDomiciliarChildCadastroViewModel vm)
        {
            if (!Autenticado())
            {
                throw new ValidationException("É preciso estar logado.");
            }

            var ficha = await Domain.FichaVisitaDomiciliarMaster.FindAsync(uuidFicha);

            if (ficha == null)
                throw new ValidationException("Ficha master não encontrada.");

            if (ficha.UnicaLotacaoTransport.OrigemVisita.finalizado)
                throw new ValidationException("Não é possível alterar os dados de uma ficha finalizada.");

            FichaVisitaDomiciliarChildCadastroViewModel child = await vm.LimparESalvarDados(Domain, Url, ficha);

            return Ok(child.ToDetail());
        }

        /// <summary>
        /// Remover Ficha de Visita Domiciliar Child
        /// </summary>
        /// <param name="uuidFicha"></param>
        /// <param name="childId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("remover/VisitaDomiciliar/{uuidFicha}/child/{childId:guid}")]
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> RemoverVisitaDomiciliarChild([FromUri] string uuidFicha, [FromUri] Guid childId)
        {
            if (!Autenticado())
            {
                throw new ValidationException("É preciso estar logado.");
            }

            var ficha = await Domain.FichaVisitaDomiciliarMaster.FindAsync(uuidFicha);

            if (ficha == null) return Ok();

            if (ficha.UnicaLotacaoTransport.OrigemVisita.finalizado)
                throw new ValidationException("Não é possível alterar os dados de uma ficha finalizada.");

            var child = ficha.FichaVisitaDomiciliarChild.SingleOrDefault(x => x.childId == childId);

            if (child == null) return Ok();

            child.SIGSM_MotivoVisita.Clear();

            await Domain.SaveChangesAsync();

            Domain.FichaVisitaDomiciliarChild.Remove(child);

            await Domain.SaveChangesAsync();

            Guid.TryParse(ficha.uuidFicha.Substring(8), out Guid id);

            var proc = await Domain.SIGSM_Transmissao_Processos.FindAsync(id);

            if (proc != null)
            {
                proc.SIGSM_Transmissao_Processos_Log.Clear();

                Domain.SIGSM_Transmissao_Processos.Remove(proc);
            }

            await Domain.SaveChangesAsync();

            return Ok();
        }
        #endregion
    }
}
