using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Softpark.Models;
using System.Web.Http.Description;
using Softpark.WS.ViewModels.SIGSM;
using System.Web.Http.Results;
using System.Web;
using System.Web.Http.ModelBinding;
using DataTables.AspNet.WebApi2;
using DataTables.AspNet.Core;

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
        [ResponseType(typeof(JsonResult<IDataTablesResponse>))]
        public IHttpActionResult ListarCadastroIndividual([FromUri] DataTablesRequest request)
        {
            if (!Autenticado())
            {
                return BadRequest("É preciso estar logado.");
            }

            if (request.Length == 0)
            {
                request.Length = 10;
            }

            var data = Domain.ASSMED_Cadastro
                .Where(x => x.Nome != null && x.Nome.Trim().Length > 0 && !x.Nome.Contains("*"))
                .Select(x => new CadastroIndividualVM
                {
                    CnsCidadao = x.ASSMED_CadastroDocPessoal.Count == 0 ? null : x.ASSMED_CadastroDocPessoal.Where(y => y.CodTpDocP == 6).Select(y => y.Numero).FirstOrDefault(),
                    Codigo = x.Codigo,
                    CPF = x.ASSMED_PesFisica == null || x.ASSMED_PesFisica.CPF == null || x.ASSMED_PesFisica.CPF.Trim().Length == 0 ? null : x.ASSMED_PesFisica.CPF,
                    DataNascimento = x.ASSMED_PesFisica == null ? null : x.ASSMED_PesFisica.DtNasc,
                    IdFicha = x.IdFicha,
                    NomeCidadao = x.Nome,
                    NumContrato = x.NumContrato
                });

            var filteredData = (request.Search.Value == null || request.Search.Value.Length == 0) ? data :
                data.Where(_item =>
                (_item.NomeCidadao != null && _item.NomeCidadao.Contains(request.Search.Value)) ||
                (_item.CPF != null && _item.CPF.Contains(request.Search.Value)) ||
                (_item.CnsCidadao != null && _item.CnsCidadao.Contains(request.Search.Value)) ||
                (_item.Codigo.ToString().Contains(request.Search.Value)) ||
                (_item.IdFicha != null && _item.IdFicha.ToString().Contains(request.Search.Value)) ||
                (_item.NumContrato.ToString().Contains(request.Search.Value)) ||
                (_item.DataNascimento != null &&
                _item.DataNascimento.Value.ToString("dd/MM/yyyy").Contains(request.Search.Value)));

            // Paging filtered data.
            // Paging is rather manual due to in-memmory (IEnumerable) data.
            var dataPage = filteredData
                .OrderBy(x => x.NomeCidadao)
                .ThenBy(x => x.DataNascimento)
                .Skip(request.Start).Take(request.Length)
                .ToArray();

            // Response creation. To create your response you need to reference your request, to avoid
            // request/response tampering and to ensure response will be correctly created.
            var response = DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

            // Easier way is to return a new 'DataTablesJsonResult', which will automatically convert your
            // response to a json-compatible content, so DataTables can read it when received.
            return new DataTablesJsonResult(response, Request) { };
        }

        /// <summary>
        /// Detalhar Cadastro Individual
        /// </summary>
        /// <param name="codigo">Código da pessoa em ASSMED_Cadastro</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/sigsm/detalhar/CadastroIndividual/{codigo}")]
        [ResponseType(typeof(string))]
        public IHttpActionResult DetalharCadastroIndividual(int codigo)
        {
            if (!Autenticado())
            {
                return BadRequest("É preciso estar logado.");
            }

            return Ok(Versions.Version);
        }

        /// <summary>
        /// Salvar Cadastro Individual
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/sigsm/salvar/CadastroIndividual")]
        [ResponseType(typeof(string))]
        public IHttpActionResult SalvarCadastroIndividual()
        {
            if (!Autenticado())
            {
                return BadRequest("É preciso estar logado.");
            }

            return Ok(Versions.Version);
        }
        #endregion

        #region CadastroDomiciliar
        /// <summary>
        /// Buscar cadastros domiciliares
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/sigsm/listar/CadastroDomiciliar")]
        [ResponseType(typeof(string))]
        public IHttpActionResult ListarCadastroDomiciliar()
        {
            if (!Autenticado())
            {
                return BadRequest("É preciso estar logado.");
            }

            return Ok(Versions.Version);
        }

        /// <summary>
        /// Detalhar Cadastro Domiciliar
        /// </summary>
        /// <param name="codigo">Código do domicilio em ASSMED_Endereco</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/sigsm/detalhar/CadastroDomiciliar/{codigo}")]
        [ResponseType(typeof(string))]
        public IHttpActionResult DetalharCadastroDomiciliar(int codigo)
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
        [Route("api/sigsm/salvar/CadastroDomiciliar")]
        [ResponseType(typeof(string))]
        public IHttpActionResult SalvarCadastroDomiciliar()
        {
            if (!Autenticado())
            {
                return BadRequest("É preciso estar logado.");
            }

            return Ok(Versions.Version);
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
