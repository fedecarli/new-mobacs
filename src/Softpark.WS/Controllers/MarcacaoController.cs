using DataTables.AspNet.WebApi2;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using System.Data.Entity;
using Softpark.Models;
using Softpark.WS.ViewModels;

namespace Softpark.WS.Controllers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class MarcacaoController : BaseAjaxController
    {
        public MarcacaoController() : base(new Models.DomainContainer()) { }

        private List<ColumnDef> _columnsAssoc = ColumnDef.From<ListagemAssociacaoViewModel>();

        private List<ColumnDef> _columnsDown = ColumnDef.From<ListagemDownloadViewModel>();

        // GET: Marcacao
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Associacao()
        {
            return View(_columnsAssoc);
        }

        [HttpPost]
        public async Task<ActionResult> Associacao([Bind(Include = "associacoes")] AssociacoesList assocs)
        {
            if (!ModelState.IsValid)
                return View(_columnsAssoc);

            var associacoes = assocs.associacoes;

            var idUsuario = Convert.ToInt32(ASPSessionVar.Read("idUsuario") ?? "0");
            var idSetor = Convert.ToInt32(ASPSessionVar.Read("idSetor") ?? "0");

            var cred = (from u in Domain.AS_CredenciadosUsu
                        join v in Domain.AS_CredenciadosVinc
                        on u.CodCred equals v.CodCred
                        join s in Domain.SIGSM_MicroArea_CredenciadoVinc
                        on new { u.NumContrato, u.CodCred, v.ItemVinc } equals new { s.NumContrato, s.CodCred, s.ItemVinc }
                        where u.CodUsuD == idUsuario && v.CodSetor == idSetor
                        select s);

            var associados = cred.Include(x => x.SIGSM_MicroArea_CredenciadoCidadao).SelectMany(x => x.SIGSM_MicroArea_CredenciadoCidadao);

            var cr = associacoes.Where(x => !x.Relacionar).Select(x => x.Codigo).ToArray();

            var rems = associados.Where(x => cr.Contains(x.Codigo));

            Domain.SIGSM_MicroArea_CredenciadoCidadao.RemoveRange(rems);

            var ca = await associados.Select(x => x.Codigo).ToArrayAsync();

            var allc = associacoes.Where(x => x.Relacionar && !ca.Contains(x.Codigo)).Select(x => x.Codigo).ToArray();

            var codCred = cred.First().CodCred;
            var mas = cred.Select(x => x.SIGSM_MicroArea_Unidade.SIGSM_MicroAreas.Codigo).ToArray();

            var aassocs = from vv in Domain.VW_Vinculos
                          join cc in Domain.SIGSM_MicroArea_CredenciadoVinc
                          on vv.CodCred equals cc.CodCred
                          where (vv.CodCred == null || vv.CodCred == codCred) &&
                          (vv.CodMicroArea == null || mas.Contains(vv.CodMicroArea)) &&
                          allc.Contains(vv.Codigo)
                          select new
                          {
                              Codigo = vv.Codigo,
                              idMaCredVinc = cc.id,
                              NumContrato = 22,
                              RealizarDownload = false
                          };

            var aas = await aassocs.Distinct().ToArrayAsync();

            Domain.SIGSM_MicroArea_CredenciadoCidadao.AddRange(aas.Select(x => new SIGSM_MicroArea_CredenciadoCidadao
            {
                Codigo = x.Codigo,
                idMaCredVinc = x.idMaCredVinc,
                NumContrato = x.NumContrato,
                RealizarDownload = x.RealizarDownload
            }));

            await Domain.SaveChangesAsync();

            return View(_columnsAssoc);
        }

        [HttpGet]
        public async Task<JsonResult> ListAssoc([Bind(Include = "iDisplayStart,iDisplayLength,iSortingCols,iSortCol_0,sSortDir_0,sSearch,sEcho")] DataTableParameters request)
        {
            var idUsuario = Convert.ToInt32(ASPSessionVar.Read("idUsuario") ?? "0");
            var idSetor = Convert.ToInt32(ASPSessionVar.Read("idSetor") ?? "0");

            var cred = await (from u in Domain.AS_CredenciadosUsu
                              join v in Domain.AS_CredenciadosVinc
                              on u.CodCred equals v.CodCred
                              join s in Domain.SIGSM_MicroArea_CredenciadoVinc
                              on new { u.NumContrato, u.CodCred, v.ItemVinc } equals new { s.NumContrato, s.CodCred, s.ItemVinc }
                              where u.CodUsuD == idUsuario && v.CodSetor == idSetor
                              select s).ToListAsync();

            var itens = new DataTableResult
            {
                aaData = new object[0],
                iTotalDisplayRecords = 0,
                iTotalRecords = 0,
                sEcho = request.sEcho.ToString()
            };

            if (cred.Any())
            {
                var codCred = cred.First().CodCred;
                var mas = cred.Select(x => x.SIGSM_MicroArea_Unidade.SIGSM_MicroAreas.Codigo).ToArray();

                var assocs = from vv in Domain.VW_Vinculos
                             where (vv.CodCred == null || vv.CodCred == codCred) &&
                             (vv.CodMicroArea == null || mas.Contains(vv.CodMicroArea))
                             select vv;

                if (request == null) request = new DataTableParameters(Request.QueryString);

                var comp = request.ComposeQueryable(assocs.OrderBy(x => x.row),
                    vv => vv.MicroArea.Contains(request.sSearch) ||
                    ((vv.TpLog == null ? "" : (vv.TpLog + " - ")) +
                                 (vv.Logradouro == null ? "" : (vv.Logradouro + ", ")) +
                                 (vv.Numero == null ? "S/N" : vv.Numero) +
                                 (vv.Bairro == null ? " - " : (vv.Bairro + " - "))).Contains(request.sSearch) ||
                    ((vv.CNSCidadao == null ? "" : (vv.CNSCidadao + " - ")) + vv.NomeCidadao)
                    .Contains(request.sSearch), vv => new ListagemAssociacaoViewModel
                    {
                        Endereco = (vv.TpLog == null ? "" : (vv.TpLog + " - ")) +
                                 (vv.Logradouro == null ? "" : (vv.Logradouro + ", ")) +
                                 (vv.Numero == null ? "S/N" : vv.Numero) +
                                 (vv.Bairro == null ? " - " : (vv.Bairro + " - ")),
                        MicroArea = vv.MicroArea,
                        Municipe = (vv.CNSCidadao == null ? "" : (vv.CNSCidadao + " - ")) + vv.NomeCidadao,
                        check = "<input type=\"checkbox\" class=\"chk_associar\"" + ((vv.Vinculo == null ? "" : (" checked data-vindulo=\"" + vv.Vinculo.ToString() + "\"")) +
                                 " data-codigo=\"" + vv.Codigo + "\"") + "/>"
                    });

                itens = await comp.Result();
            }

            return Json(itens, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Download()
        {
            return View(_columnsDown);
        }

        [HttpGet]
        public async Task<JsonResult> ListDown([Bind(Include = "iDisplayStart,iDisplayLength,iSortingCols,iSortCol_0,sSortDir_0,sSearch,sEcho")] DataTableParameters request)
        {
            var idUsuario = Convert.ToInt32(ASPSessionVar.Read("idUsuario") ?? "0");
            var idSetor = Convert.ToInt32(ASPSessionVar.Read("idSetor") ?? "0");
            
            var cred = await (from u in Domain.AS_CredenciadosUsu
                              join v in Domain.AS_CredenciadosVinc
                              on u.CodCred equals v.CodCred
                              join s in Domain.SIGSM_MicroArea_CredenciadoVinc
                              on new { u.NumContrato, u.CodCred, v.ItemVinc } equals new { s.NumContrato, s.CodCred, s.ItemVinc }
                              where u.CodUsuD == idUsuario && v.CodSetor == idSetor
                              select s).Include(x => x.SIGSM_MicroArea_CredenciadoCidadao).SingleOrDefaultAsync();

            var itens = new DataTableResult
            {
                aaData = new object[0],
                iTotalDisplayRecords = 0,
                iTotalRecords = 0,
                sEcho = request.sEcho.ToString()
            };

            if (cred != null)
            {
                IOrderedQueryable<SIGSM_MicroArea_CredenciadoCidadao> assocs =
                            (from creds in Domain.SIGSM_MicroArea_CredenciadoCidadao
                             where creds.idMaCredVinc == cred.id
                             select creds)
                             .Include(x => x.SIGSM_MicroArea_CredenciadoVinc)
                             .Include(x => x.SIGSM_MicroArea_CredenciadoVinc.SIGSM_MicroArea_Unidade)
                             .Include(x => x.SIGSM_MicroArea_CredenciadoVinc.SIGSM_MicroArea_Unidade.SIGSM_MicroAreas)
                             .Include(x => x.ASSMED_Cadastro)
                             .Include(x => x.ASSMED_Cadastro.ASSMED_CadastroDocPessoal)
                             .Include(x => x.ASSMED_Cadastro.ASSMED_Endereco)
                             .Include(x => x.ASSMED_Cadastro.ASSMED_Endereco)
                             .OrderBy(x => (x.RealizarDownload && x.DownloadDomiciliar == null && x.DownloadIndividual == null ? 0 : 1))
                             .ThenBy(x => x.id);

                if (request == null) request = new DataTableParameters(Request.QueryString);

                var comp = request.ComposeQueryable(assocs,
                    x =>
                    x.SIGSM_MicroArea_CredenciadoVinc.SIGSM_MicroArea_Unidade.SIGSM_MicroAreas.Descricao.Contains(request.sSearch) ||
                    x.ASSMED_Cadastro.ASSMED_CadastroDocPessoal.Any(y => y.Numero != null && y.Numero.Trim().Contains(request.sSearch)) ||
                    x.ASSMED_Cadastro.ASSMED_Endereco.Any(vv => (
                                 (vv.CodTpLogra == null ? "" : (vv.TB_MS_TIPO_LOGRADOURO.DS_TIPO_LOGRADOURO_ABREV + " - ")) +
                                 (vv.Logradouro == null ? "" : (vv.Logradouro + ", ")) +
                                 (vv.Numero == null ? "S/N" : vv.Numero) +
                                 (vv.Bairro == null ? " - " : (vv.Bairro + " - "))).Contains(request.sSearch)) ||
                    x.ASSMED_Cadastro.Nome.Contains(request.sSearch),
                    x => new ListagemDownloadViewModel
                    {
                        Endereco = (x.ASSMED_Cadastro.ASSMED_Endereco.Count > 0 ? x.ASSMED_Cadastro.ASSMED_Endereco.Select(vv =>
                                 (vv.CodTpLogra == null ? "" : (vv.TB_MS_TIPO_LOGRADOURO.DS_TIPO_LOGRADOURO_ABREV + " - ")) +
                                 (vv.Logradouro == null ? "" : (vv.Logradouro + ", ")) +
                                 (vv.Numero == null ? "S/N" : vv.Numero) +
                                 (vv.Bairro == null ? " - " : (vv.Bairro + " - "))).Last() : ""),
                        MicroArea = x.SIGSM_MicroArea_CredenciadoVinc.SIGSM_MicroArea_Unidade.SIGSM_MicroAreas.Descricao,
                        Municipe = (x.ASSMED_Cadastro.ASSMED_CadastroDocPessoal.LastOrDefault(y => y.Numero != null && y.Numero.Trim().Length == 15 && y.CodTpDocP == 6) == null ? "" :
                        x.ASSMED_Cadastro.ASSMED_CadastroDocPessoal.Last(y => y.Numero != null && y.Numero.Trim().Length == 15 && y.CodTpDocP == 6).Numero.Trim())
                         + " - " + x.ASSMED_Cadastro.Nome.ToUpper(),
                        check = "<input type=\"checkbox\" class=\"chk_download\"" +
                        (" data-vinculo=\"" + x.id.ToString() + "\" data-codigo=\"" + x.Codigo.ToString() + "\"" +
                                 (x.RealizarDownload && x.DownloadDomiciliar == null && x.DownloadIndividual == null ? " checked" : "")) + "/>"
                    });

                itens = await comp.Result();
            }

            return Json(itens, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<ActionResult> Download([Bind(Include = "downloads")] DownloadsList downs)
        {
            if (!ModelState.IsValid)
                return View(_columnsAssoc);

            var associacoes = downs.downloads;

            var idUsuario = Convert.ToInt32(ASPSessionVar.Read("idUsuario") ?? "0");
            var idSetor = Convert.ToInt32(ASPSessionVar.Read("idSetor") ?? "0");

            var cred = (from u in Domain.AS_CredenciadosUsu
                        join v in Domain.AS_CredenciadosVinc
                        on u.CodCred equals v.CodCred
                        join s in Domain.SIGSM_MicroArea_CredenciadoVinc
                        on new { u.NumContrato, u.CodCred, v.ItemVinc } equals new { s.NumContrato, s.CodCred, s.ItemVinc }
                        join sv in Domain.SIGSM_MicroArea_CredenciadoCidadao
                        on s.id equals sv.idMaCredVinc
                        where u.CodUsuD == idUsuario && v.CodSetor == idSetor
                        select sv);

            var cr = associacoes.Where(x => !x.Baixar).Select(x => x.Vinculo).ToArray();
            var cb = associacoes.Where(x => x.Baixar).Select(x => x.Vinculo).ToArray();

            var rems = await cred.Where(x => cr.Contains(x.id)).ToListAsync();

            rems.ForEach(x =>
            {
                x.DownloadDomiciliar = null;
                x.DownloadIndividual = null;
                x.RealizarDownload = false;
            });

            var allc = await cred.Where(x => cb.Contains(x.id)).ToListAsync();

            allc.ForEach(x =>
            {
                x.DownloadDomiciliar = null;
                x.DownloadIndividual = null;
                x.RealizarDownload = true;
            });

            await Domain.SaveChangesAsync();

            return View(_columnsAssoc);
        }

    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}