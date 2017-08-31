﻿using System;
using System.Linq;
using System.Web.Http;
using Softpark.Models;
using System.Web.Http.Description;
using Softpark.WS.ViewModels.SIGSM;
using DataTables.AspNet.WebApi2;
using DataTables.AspNet.Core;
using Softpark.WS.ViewModels;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Data.Entity;

namespace Softpark.WS.Controllers.Api
{
    /// <summary>
    /// Controller para chamadas via SIGSM
    /// </summary>
    [System.Web.Mvc.OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
    [System.Web.Mvc.SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class SIGSMController : BaseApiController
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected SIGSMController() : base(new DomainContainer()) { }

        public SIGSMController(DomainContainer domain) : base(domain) { }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        #region CadastroIndividual
        /// <summary>
        /// Buscar cadastros individuais
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/sigsm/listar/CadastroIndividual")]
        [ResponseType(typeof(DataTablesJsonResult))]
        public async Task<IHttpActionResult> ListarCadastroIndividual(IDataTablesRequest request)
        {
            if (!Autenticado())
            {
                return BadRequest("É preciso estar logado.");
            }

            if (request == null) request = new DataTablesRequest
            {
                Draw = 1,
                Length = 10
            };

            var data = Domain.VW_ConsultaCadastrosIndividuais;

            DateTime? dtFilter = null;
            if (DateTime.TryParseExact(request.Search.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _dtFilter))
                dtFilter = _dtFilter;

            decimal? codigo = null;
            if (decimal.TryParse(request.Search.Value, out decimal _codigo))
                codigo = _codigo;

            string cns = null;
            if (!string.IsNullOrEmpty(request.Search.Value) && request.Search.Value.Length == 15 && Regex.IsMatch(request.Search.Value, "([0-9]+)"))
                cns = request.Search.Value;

            Guid? idFicha = null;

            if (Guid.TryParse(request.Search.Value, out Guid _idFicha))
                idFicha = _idFicha;

            var filteredData = data.AsQueryable();

            if (request.Search.Value != null && request.Search.Value.Length > 0)
            {
                if (cns != null && await data.AnyAsync(x => x.CnsCidadao == cns))
                    filteredData = data.Where(x => x.CnsCidadao == cns);
                else if (idFicha != null && await data.AnyAsync(x => x.IdFicha == _idFicha))
                    filteredData = data.Where(x => x.IdFicha == _idFicha);
                else if (codigo != null && await data.AnyAsync(x => x.Codigo == _codigo))
                    filteredData = data.Where(x => x.Codigo == _codigo);
                else if (dtFilter != null && await data.AnyAsync(x => x.DataNascimento == _dtFilter))
                    filteredData = data.Where(x => x.DataNascimento == _dtFilter);
                else
                    filteredData = data.Where(_item => (
                    (_item.NomeCidadao != null ? _item.NomeCidadao : "") +
                    (_item.MunicipioNascimento != null ? _item.MunicipioNascimento : "") +
                    (_item.NomeMae != null ? _item.NomeMae : "")).Contains(request.Search.Value));
            }

            // Paging filtered data.
            // Paging is rather manual due to in-memmory (IEnumerable) data.
            var dataPage = (await filteredData
                .OrderBy(x => x.NomeCidadao)
                .ThenBy(x => x.DataNascimento)
                .ThenBy(x => x.NomeMae)
                .ThenBy(x => x.CnsCidadao)
                .ThenBy(x => x.MunicipioNascimento)
                .ThenBy(x => x.Codigo)
                .Skip(request.Start).Take(request.Length)
                .ToArrayAsync())
                .Select(x => new object[] {
                    x.NomeCidadao,
                    x.DataNascimento?.ToString("dd/MM/yyyy"),
                    x.NomeMae,
                    x.CnsCidadao,
                    x.MunicipioNascimento,
                    x.Codigo
                })
                .ToArray();

            // Response creation. To create your response you need to reference your request, to avoid
            // request/response tampering and to ensure response will be correctly created.
            var response = DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

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
        [Route("api/sigsm/detalhar/CadastroIndividual/{codigo}")]
        [ResponseType(typeof(FormCadastroIndividual))]
        public IHttpActionResult DetalharCadastroIndividual(decimal codigo)
        {
            if (!Autenticado())
            {
                return BadRequest("É preciso estar logado.");
            }

            var cad = Domain.ASSMED_Cadastro.SingleOrDefault(x => x.Codigo == codigo);

            if (cad == null)
            {
                return BadRequest("Cadastro não encontrado!");
            }

            var cns = cad.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 6)?.Numero;
            var rg = cad.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 1);
            var mun = cad.ASSMED_PesFisica?.MUNICIPIONASC ?? 0;
            var nac = (cad.ASSMED_PesFisica?.Nacionalidade ?? cad.ASSMED_PesFisica?.CodNacao) ?? 0;

            var ulog = Convert.ToInt32(ASPSessionVar.Read("idUsuario", Url) ?? "0");

            var cset = Convert.ToInt32(ASPSessionVar.Read("idSetor", Url) ?? "0");

            var setor = Domain.AS_SetoresPar.SingleOrDefault(x => x.CodSetor == cset);

            var data = Domain.IdentificacaoUsuarioCidadao.Where(x => x.Codigo == codigo)
                .SelectMany(x => x.CadastroIndividual).OrderByDescending(x => x.idAuto)
                .FirstOrDefault();

            var cont = Domain.ASSMED_Contratos.First();

            if (data == null)
            {
                data = new CadastroIndividual
                {
                    IdentificacaoUsuarioCidadao1 = new IdentificacaoUsuarioCidadao
                    {
                        Codigo = codigo,
                        beneficiarioBolsaFamilia = false,
                        cnsCidadao = cns,
                        cnsResponsavelFamiliar = null,
                        codigoIbgeMunicipioNascimento = Domain.Cidade.SingleOrDefault(x => x.CodCidade == mun)?.CodIbge ?? cont.CodigoIbgeMunicipio,
                        ComplementoRG = rg?.ComplementoRG,
                        CPF = cad.ASSMED_PesFisica?.CPF,
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
                        etnia = cad.ASSMED_PesFisica?.CodEtnia,
                        id = Guid.Empty,
                        microarea = null,
                        nacionalidadeCidadao = nac == 10 || nac == 0 ? 1 : 2,
                        paisNascimento = Domain.Nacionalidade.FirstOrDefault(x => x.CodNacao == nac)?.codigo,
                        numeroNisPisPasep = cad.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 7)?.Numero,
                        num_contrato = 22,
                        portariaNaturalizacao = cad.ASSMED_PesFisica?.NATURALIZACAOPORTARIA,
                        racaCorCidadao = cad.ASSMED_PesFisica?.CodCor ?? 0,
                        telefoneCelular = cad.ASSMED_CadTelefones.Where(x => x.TipoTel == "C").Select(x => $"{x.DDD}{x.NumTel}".Trim()).FirstOrDefault(),
                        RG = rg?.Numero,
                        sexoCidadao = cad.ASSMED_PesFisica?.Sexo == "M" ? 0 : cad.ASSMED_PesFisica?.Sexo == "F" ? 1 : 4,
                        statusEhResponsavel = true,
                        stForaArea = true
                    },
                    CondicoesDeSaude1 = null,
                    DataRegistro = null,
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
                    UnicaLotacaoTransport = new UnicaLotacaoTransport
                    {
                        cnes = setor?.CNES,
                        codigoIbgeMunicipio = cont.CodigoIbgeMunicipio,
                        dataAtendimento = DateTime.Now,
                        id = Guid.Empty,
                        token = Guid.Empty,
                        OrigemVisita = new OrigemVisita
                        {
                            enviado = false,
                            enviarParaThrift = false,
                            finalizado = false,
                            id_tipo_origem = 2,
                            token = Guid.Empty
                        }
                    }
                };
            }

            if (data.IdentificacaoUsuarioCidadao1 == null)
            {
                data.IdentificacaoUsuarioCidadao1 = new IdentificacaoUsuarioCidadao
                {
                    Codigo = codigo,
                    beneficiarioBolsaFamilia = false,
                    cnsCidadao = cns,
                    cnsResponsavelFamiliar = null,
                    codigoIbgeMunicipioNascimento = Domain.Cidade.SingleOrDefault(x => x.CodCidade == mun)?.CodIbge ?? cont.CodigoIbgeMunicipio,
                    ComplementoRG = rg?.ComplementoRG,
                    CPF = cad.ASSMED_PesFisica?.CPF,
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
                    etnia = cad.ASSMED_PesFisica?.CodEtnia,
                    id = Guid.Empty,
                    microarea = null,
                    nacionalidadeCidadao = nac == 10 || nac == 0 ? 1 : 2,
                    paisNascimento = Domain.Nacionalidade.FirstOrDefault(x => x.CodNacao == nac)?.codigo,
                    numeroNisPisPasep = cad.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 7)?.Numero,
                    num_contrato = 22,
                    portariaNaturalizacao = cad.ASSMED_PesFisica?.NATURALIZACAOPORTARIA,
                    racaCorCidadao = cad.ASSMED_PesFisica?.CodCor ?? 0,
                    telefoneCelular = cad.ASSMED_CadTelefones.Where(x => x.TipoTel == "C").Select(x => $"{x.DDD}{x.NumTel}".Trim()).FirstOrDefault(),
                    RG = rg?.Numero,
                    sexoCidadao = cad.ASSMED_PesFisica?.Sexo == "M" ? 0 : cad.ASSMED_PesFisica?.Sexo == "F" ? 1 : 4,
                    statusEhResponsavel = true,
                    stForaArea = true
                };
            }

            data.IdentificacaoUsuarioCidadao1.Codigo = codigo;
            data.IdentificacaoUsuarioCidadao1.cnsCidadao = cns;
            data.IdentificacaoUsuarioCidadao1.codigoIbgeMunicipioNascimento = Domain.Cidade
                .SingleOrDefault(x => x.CodCidade == mun)?.CodIbge ?? cont.CodigoIbgeMunicipio;
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
            data.IdentificacaoUsuarioCidadao1.nacionalidadeCidadao = nac == 10 || nac == 0 ? 1 : 2;
            data.IdentificacaoUsuarioCidadao1.paisNascimento = Domain.Nacionalidade.FirstOrDefault(x => x.CodNacao == nac)?.codigo;
            data.IdentificacaoUsuarioCidadao1.numeroNisPisPasep = cad.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 7)?.Numero;
            data.IdentificacaoUsuarioCidadao1.portariaNaturalizacao = cad.ASSMED_PesFisica?.NATURALIZACAOPORTARIA;
            data.IdentificacaoUsuarioCidadao1.racaCorCidadao = cad.ASSMED_PesFisica?.CodCor ?? 0;
            data.IdentificacaoUsuarioCidadao1.telefoneCelular = cad.ASSMED_CadTelefones.Where(x => x.TipoTel == "C").Select(x => $"{x.DDD}{x.NumTel}".Trim()).FirstOrDefault();
            data.IdentificacaoUsuarioCidadao1.RG = rg?.Numero;
            data.IdentificacaoUsuarioCidadao1.sexoCidadao = cad.ASSMED_PesFisica?.Sexo == "M" ? 0 : cad.ASSMED_PesFisica?.Sexo == "F" ? 1 : 4;

            return Ok(new FormCadastroIndividual
            {
                CabecalhoTransporte = UnicaLotacaoTransportCadastroViewModel.ApplyModel(data.UnicaLotacaoTransport),
                CadastroIndividual = data
            });
        }

        /// <summary>
        /// Salvar Cadastro Individual
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/sigsm/salvar/CadastroIndividual")]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> SalvarCadastroIndividual([FromBody] FormCadastroIndividual form)
        {
            if (!Autenticado())
            {
                return BadRequest("É preciso estar logado.");
            }

            var id = await form.LimparESalvarDados(Domain, Url);

            Domain.PR_ProcessarFichasAPI(id);
            
            return Ok(true);
        }
        #endregion

        #region CadastroDomiciliar
        /// <summary>
        /// Buscar cadastros domiciliares
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/sigsm/listar/CadastroDomiciliar")]
        [ResponseType(typeof(DataTablesJsonResult))]
        public async Task<IHttpActionResult> ListarCadastroDomiciliar(IDataTablesRequest request)
        {
            if (!Autenticado())
            {
                return BadRequest("É preciso estar logado.");
            }

            if (request == null) request = new DataTablesRequest
            {
                Draw = 1,
                Length = 10
            };

            var data = Domain.VW_ConsultaCadastrosDomiciliares.AsQueryable();

            decimal? codigo = null;
            if (decimal.TryParse(request.Search.Value, out decimal _codigo))
                codigo = _codigo;

            var idFicha = (Guid?)null;
            if (Guid.TryParse(request.Search.Value, out Guid _idFicha))
                idFicha = _idFicha;

            var filteredData = data;
            if (request.Search.Value != null && request.Search.Value.Length > 0)
            {
                if (codigo != null && await data.AnyAsync(x => x.Codigo == _codigo))
                    filteredData = data.Where(x => x.Codigo == _codigo);
                else if (idFicha != null && await data.AnyAsync(x => x.IdFicha == _idFicha))
                    filteredData = data.Where(x => x.IdFicha == _idFicha);
                else
                    filteredData = data.Where(_item => (
                    (_item.Numero != null ? _item.Numero : "") +
                    (_item.Endereco != null ? _item.Endereco : "") +
                    (_item.Complemento != null ? _item.Complemento : "") +
                    (_item.Telefone != null ? _item.Telefone : "") +
                    (_item.Responsavel != null ? _item.Responsavel : "")).Contains(request.Search.Value));
            }

            // Paging filtered data.
            // Paging is rather manual due to in-memmory (IEnumerable) data.
            var dataPage = (await filteredData
                .OrderBy(x => x.Endereco)
                .ThenBy(x => x.Numero)
                .ThenBy(x => x.Complemento)
                .ThenBy(x => x.Telefone)
                .ThenBy(x => x.Responsavel)
                .ThenBy(x => x.Codigo)
                .Skip(request.Start).Take(request.Length)
                .ToArrayAsync())
                .Select(x => new object[] {
                    x.Endereco,
                    x.Numero,
                    x.Complemento,
                    x.Telefone,
                    x.Responsavel,
                    x.Codigo
                })
                .ToArray();

            // Response creation. To create your response you need to reference your request, to avoid
            // request/response tampering and to ensure response will be correctly created.
            var response = DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

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
        [Route("api/sigsm/detalhar/CadastroDomiciliar/{codigo}")]
        [ResponseType(typeof(FormCadastroDomiciliar))]
        public IHttpActionResult DetalharCadastroDomiciliar(decimal codigo)
        {
            if (!Autenticado())
            {
                return BadRequest("É preciso estar logado.");
            }

            var cad = Domain.ASSMED_Endereco.Where(x => x.Codigo == codigo)
                .OrderByDescending(x => x.ItemEnd).FirstOrDefault();

            if (cad == null)
            {
                return BadRequest("Cadastro não encontrado!");
            }

            var cns = cad.ASSMED_Cadastro.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 6)?.Numero;

            var mun = cad.CodCidade ?? 0;

            var cont = Domain.ASSMED_Contratos.First();

            var m = Domain.Cidade.SingleOrDefault(x => x.CodCidade == mun) ?? Domain.Cidade.FirstOrDefault(x => x.CodIbge == cont.CodigoIbgeMunicipio);

            var ulog = Convert.ToInt32(ASPSessionVar.Read("idUsuario", Url) ?? "0");

            var cset = Convert.ToInt32(ASPSessionVar.Read("idSetor", Url) ?? "0");

            var setor = Domain.AS_SetoresPar.SingleOrDefault(x => x.CodSetor == cset);

            var data = Domain.EnderecoLocalPermanencia.Where(x => x.Codigo == codigo)
                .SelectMany(x => x.CadastroDomiciliar).OrderByDescending(x => x.idAuto)
                .FirstOrDefault();

            if (data == null)
            {
                data = new CadastroDomiciliar
                {
                    EnderecoLocalPermanencia1 = new EnderecoLocalPermanencia
                    {
                        bairro = cad.Bairro,
                        cep = cad.CEP,
                        Codigo = cad.Codigo,
                        num_contrato = cad.NumContrato,
                        codigoIbgeMunicipio = m.CodIbge,
                        complemento = cad.Complemento,
                        item_end = cad.ItemEnd,
                        id = Guid.Empty,
                        microarea = null,
                        nomeLogradouro = cad.Logradouro,
                        numero = cad.Numero,
                        numeroDneUf = m.CodDNE?.ToString()?.Trim()?.PadLeft(2, '0'),
                        pontoReferencia = cad.ENDREFERENCIA,
                        stForaArea = cad.ENDSEMAREA == 1,
                        stSemNumero = cad.SEMNUMERO == 1,
                        telefoneContato = cad.ASSMED_Cadastro.ASSMED_CadTelefones
                            .Where(x => x.TipoTel == "C").Select(x => $"{x.DDD}{x.NumTel}").FirstOrDefault(),
                        telefoneResidencia = cad.ASSMED_Cadastro.ASSMED_CadTelefones
                            .Where(x => x.TipoTel == "R").Select(x => $"{x.DDD}{x.NumTel}").FirstOrDefault(),
                        tipoLogradouroNumeroDne = Domain.TB_MS_TIPO_LOGRADOURO.FirstOrDefault(x => x.DS_TIPO_LOGRADOURO_ABREV == cad.TipoEnd)?.CO_TIPO_LOGRADOURO
                    },
                    FamiliaRow = new FamiliaRow[] {
                        new FamiliaRow
                        {
                            dataNascimentoResponsavel = cad.ASSMED_Cadastro.ASSMED_PesFisica?.DtNasc,
                            id = Guid.Empty,
                            numeroCnsResponsavel = cns
                        }
                    },
                    DataRegistro = null,
                    fichaAtualizada = false,
                    id = Guid.Empty,
                    idAuto = 0,
                    Justificativa = null,
                    uuidFichaOriginadora = Guid.Empty,
                    tpCdsOrigem = 3,
                    UnicaLotacaoTransport = new UnicaLotacaoTransport
                    {
                        cnes = setor?.CNES,
                        codigoIbgeMunicipio = cont.CodigoIbgeMunicipio,
                        dataAtendimento = DateTime.Now,
                        id = Guid.Empty,
                        token = Guid.Empty,
                        OrigemVisita = new OrigemVisita
                        {
                            enviado = false,
                            enviarParaThrift = false,
                            finalizado = false,
                            id_tipo_origem = 2,
                            token = Guid.Empty
                        }
                    }
                };
            }

            if (data.EnderecoLocalPermanencia1 == null)
            {
                data.EnderecoLocalPermanencia1 = new EnderecoLocalPermanencia
                {
                    bairro = cad.Bairro,
                    cep = cad.CEP,
                    Codigo = cad.Codigo,
                    num_contrato = cad.NumContrato,
                    codigoIbgeMunicipio = m.CodIbge,
                    complemento = cad.Complemento,
                    item_end = cad.ItemEnd,
                    id = Guid.Empty,
                    microarea = null,
                    nomeLogradouro = cad.Logradouro,
                    numero = cad.Numero,
                    numeroDneUf = m.CodDNE?.ToString()?.Trim()?.PadLeft(2, '0'),
                    pontoReferencia = cad.ENDREFERENCIA,
                    stForaArea = cad.ENDSEMAREA == 1,
                    stSemNumero = cad.SEMNUMERO == 1,
                    telefoneContato = cad.ASSMED_Cadastro.ASSMED_CadTelefones
                            .Where(x => x.TipoTel == "C").Select(x => $"{x.DDD}{x.NumTel}").FirstOrDefault(),
                    telefoneResidencia = cad.ASSMED_Cadastro.ASSMED_CadTelefones
                            .Where(x => x.TipoTel == "R").Select(x => $"{x.DDD}{x.NumTel}").FirstOrDefault(),
                    tipoLogradouroNumeroDne = Domain.TB_MS_TIPO_LOGRADOURO.FirstOrDefault(x => x.DS_TIPO_LOGRADOURO_ABREV == cad.TipoEnd)?.CO_TIPO_LOGRADOURO
                };
            }

            data.EnderecoLocalPermanencia1.bairro = cad.Bairro;
            data.EnderecoLocalPermanencia1.cep = cad.CEP;
            data.EnderecoLocalPermanencia1.Codigo = cad.Codigo;
            data.EnderecoLocalPermanencia1.num_contrato = cad.NumContrato;
            data.EnderecoLocalPermanencia1.codigoIbgeMunicipio = m.CodIbge;
            data.EnderecoLocalPermanencia1.complemento = cad.Complemento;
            data.EnderecoLocalPermanencia1.item_end = cad.ItemEnd;
            data.EnderecoLocalPermanencia1.nomeLogradouro = cad.Logradouro;
            data.EnderecoLocalPermanencia1.numero = cad.Numero;
            data.EnderecoLocalPermanencia1.numeroDneUf = m.CodDNE?.ToString()?.Trim()?.PadLeft(2, '0');
            data.EnderecoLocalPermanencia1.pontoReferencia = cad.ENDREFERENCIA;
            data.EnderecoLocalPermanencia1.stForaArea = cad.ENDSEMAREA == 1;
            data.EnderecoLocalPermanencia1.stSemNumero = cad.SEMNUMERO == 1;
            data.EnderecoLocalPermanencia1.telefoneContato = cad.ASSMED_Cadastro.ASSMED_CadTelefones
                    .Where(x => x.TipoTel == "C").Select(x => $"{x.DDD}{x.NumTel}").FirstOrDefault();
            data.EnderecoLocalPermanencia1.telefoneResidencia = cad.ASSMED_Cadastro.ASSMED_CadTelefones
                    .Where(x => x.TipoTel == "R").Select(x => $"{x.DDD}{x.NumTel}").FirstOrDefault();
            data.EnderecoLocalPermanencia1.tipoLogradouroNumeroDne = Domain.TB_MS_TIPO_LOGRADOURO.FirstOrDefault(x => x.DS_TIPO_LOGRADOURO_ABREV == cad.TipoEnd)?.CO_TIPO_LOGRADOURO;

            return Ok(new FormCadastroDomiciliar
            {
                CabecalhoTransporte = UnicaLotacaoTransportCadastroViewModel.ApplyModel(data.UnicaLotacaoTransport),
                CadastroDomiciliar = data
            });
        }

        /// <summary>
        /// Salvar Cadastro Domniciliar
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/sigsm/salvar/CadastroDomiciliar")]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> SalvarCadastroDomiciliar([FromBody] FormCadastroDomiciliar form)
        {
            if (!Autenticado())
            {
                return BadRequest("É preciso estar logado.");
            }

            //var id = await form.LimparESalvarDados(Domain, Url);
            //
            //Domain.PR_ProcessarFichasAPI(id);

            return Ok(true);
        }
        #endregion

        #region VisitaDomiciliar
        /// <summary>
        /// Buscar visitas domiciliares
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/sigsm/listar/VisitaDomiciliar")]
        [ResponseType(typeof(string))]
        public IHttpActionResult ListarVisitaDomiciliar()
        {
            if (!Autenticado())
            {
                return BadRequest("É preciso estar logado.");
            }

            return Ok(Versions.Version);
        }

        /// <summary>
        /// Detalhar Visita Domiciliar
        /// </summary>
        /// <param name="codigo">Código do domicilio em ASSMED_Endereco</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/sigsm/detalhar/VisitaDomiciliar/{codigo}")]
        [ResponseType(typeof(string))]
        public IHttpActionResult DetalharVisitaDomiciliar(int codigo)
        {
            if (!Autenticado())
            {
                return BadRequest("É preciso estar logado.");
            }

            return Ok(Versions.Version);
        }

        /// <summary>
        /// Salvar Cadastro Domniciliar
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/sigsm/salvar/VisitaDomiciliar")]
        [ResponseType(typeof(string))]
        public IHttpActionResult SalvarVisitaDomiciliar()
        {
            if (!Autenticado())
            {
                return BadRequest("É preciso estar logado.");
            }

            return Ok(Versions.Version);
        }
        #endregion
    }
}
