using System;
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
using System.Text.RegularExpressions;
using Softpark.WS.ViewModels;
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
        protected SIGSMController() : base(new DomainContainer()) { }

        public SIGSMController(DomainContainer domain) : base(domain) { }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        #region CadastroIndividual
        /// <summary>
        /// Buscar cadastros individuais
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

            if (ficha == null || Domain.SIGSM_ServicoSerializador_Fichas.Find(ficha) == null)
                throw new ValidationException("Informe a ficha para buscar o profissional.");

            if (cnes != null && cnes.Trim() == "0")
                cnes = null;

            var data = Domain.VW_Profissionais(ficha, cnes, nomeOuCns, 20).ToArray();

            return Ok(data.Select(x =>
            {
                x.Profissao = x.Profissao.Trim();
                x.CBO = x.CBO.Trim();
                x.Equipe = x.INE == null ? string.Empty : x.INE.Trim() + " - " + x.Equipe;
                x.INE = x.INE == null ? string.Empty : (Domain.SetoresINEs.Where(y => y.Numero != null && y.Numero.Trim() == x.INE.Trim()).Select(y => y.CodINE.ToString())
                    .FirstOrDefault())?.ToString();
                x.Equipe = x.INE == null ? string.Empty : x.Equipe;
                x.INE = x.INE == null ? string.Empty : x.INE;
                return x;
            }).ToArray());
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

            var cns = cad.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 6)?.Numero;
            var rg = cad.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 1);
            var nis = cad.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 7)?.Numero;
            var mun = cad.ASSMED_PesFisica?.MUNICIPIONASC;
            var nac = cad.ASSMED_PesFisica?.Nacionalidade;
            var naca = cad.ASSMED_PesFisica?.CodNacao ?? 10;

            var ulog = Convert.ToInt32(ASPSessionVar.Read("idUsuario") ?? "0");

            var cset = Convert.ToInt32(ASPSessionVar.Read("idSetor") ?? "0");

            var setor = Domain.AS_SetoresPar.SingleOrDefault(x => x.CodSetor == cset);

            var data = (await Domain.IdentificacaoUsuarioCidadao.AnyAsync(x => x.Codigo == codigo)) ?
                Domain.IdentificacaoUsuarioCidadao.Where(x => x.Codigo == codigo)
                .SelectMany(x => x.CadastroIndividual).OrderByDescending(x => x.DataRegistro)
                .FirstOrDefault() :
                (Domain.ASSMED_Cadastro.Where(x => x.Codigo == codigo && x.IdFicha != null)
                .SelectMany(x => x.IdentificacaoUsuarioCidadao.CadastroIndividual)
                .FirstOrDefault());

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
                Codigo = codigo,
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

            data.IdentificacaoUsuarioCidadao1.Codigo = codigo;
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
        [Route("detalhar/CadastroDomiciliar/{codigo:guid}")]
        [ResponseType(typeof(FormCadastroDomiciliar))]
        public async Task<IHttpActionResult> DetalharCadastroDomiciliar(Guid codigo)
        {
            if (!Autenticado())
            {
                throw new ValidationException("É preciso estar logado.");
            }

            var ulog = Convert.ToInt32(ASPSessionVar.Read("idUsuario") ?? "0");

            var cset = Convert.ToInt32(ASPSessionVar.Read("idSetor") ?? "0");

            var setor = Domain.AS_SetoresPar.SingleOrDefault(x => x.CodSetor == cset);

            var data = await Domain.CadastroDomiciliar.SingleOrDefaultAsync(x => x.id == codigo);

            if (data == null)
            {
                throw new ValidationException("Não foi possível encontrar a ficha selecionada.");
            }

            var header = data.UnicaLotacaoTransport;

            var origem = header.OrigemVisita;

            var cad = Domain.ASSMED_Endereco.Where(x => x.IdFicha == codigo)
                .OrderByDescending(x => x.ItemEnd).FirstOrDefault();

            if (cad != null)
            {
                var cns = cad.ASSMED_Cadastro.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 6)?.Numero;

                var mun = cad.CodCidade ?? 0;

                var cont = Domain.ASSMED_Contratos.First();

                var m = Domain.Cidade.SingleOrDefault(x => x.CodCidade == mun);

                data.EnderecoLocalPermanencia1.bairro = cad.Bairro;
                data.EnderecoLocalPermanencia1.cep = cad.CEP?.Replace("-", "");
                data.EnderecoLocalPermanencia1.codigoIbgeMunicipio = m?.CodIbge;
                data.EnderecoLocalPermanencia1.complemento = cad.Complemento;
                data.EnderecoLocalPermanencia1.nomeLogradouro = cad.Logradouro;
                data.EnderecoLocalPermanencia1.numero = cad.Numero;
                data.EnderecoLocalPermanencia1.numeroDneUf = m?.CodDNE?.ToString()?.Trim()?.PadLeft(2, '0');
                data.EnderecoLocalPermanencia1.pontoReferencia = cad.ENDREFERENCIA;
                data.EnderecoLocalPermanencia1.stForaArea = cad.ENDSEMAREA == 1;
                data.EnderecoLocalPermanencia1.stSemNumero = cad.SEMNUMERO == 1;
                data.EnderecoLocalPermanencia1.telefoneContato = cad.ASSMED_Cadastro.ASSMED_CadTelefones
                        .Where(x => x.TipoTel == "C").Select(x => $"{x.DDD}{x.NumTel}").FirstOrDefault();
                data.EnderecoLocalPermanencia1.telefoneResidencia = cad.ASSMED_Cadastro.ASSMED_CadTelefones
                        .Where(x => x.TipoTel == "R").Select(x => $"{x.DDD}{x.NumTel}").FirstOrDefault();
                data.EnderecoLocalPermanencia1.tipoLogradouroNumeroDne = Domain.TB_MS_TIPO_LOGRADOURO.FirstOrDefault(x => x.DS_TIPO_LOGRADOURO_ABREV == cad.TipoEnd)?.CO_TIPO_LOGRADOURO;
            }

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
            ficha.FichaVisitaDomiciliarChild.Remove(child);
            Domain.FichaVisitaDomiciliarChild.Remove(child);

            Guid.TryParse(ficha.uuidFicha.Replace(ficha.UnicaLotacaoTransport.cnes + "-", ""), out Guid id);
            var proc = await Domain.SIGSM_Transmissao_Processos.FindAsync(id);

            proc.SIGSM_Transmissao_Processos_Log.Clear();

            Domain.SIGSM_Transmissao_Processos.Remove(proc);

            await Domain.SaveChangesAsync();

            return Ok();
        }
        #endregion
    }
}
