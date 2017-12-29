using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Softpark.Models;
using System.Linq;
using DataTables.AspNet.WebApi2;
using System;
using System.Linq.Expressions;
using Softpark.WS.ViewModels.SIGSM;
using System.Collections.Generic;

namespace Softpark.WS.Controllers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class MicroAreaCredenciadoController : BaseAjaxController
    {
        public MicroAreaCredenciadoController() : base(new DomainContainer()) { }

        private readonly List<ColumnDef> tblColumns = ColumnDef.From<ListagemMicroAreaCredenciadoViewModel>();

        // GET: MicroAreaCredenciado
        public ActionResult Index() => View(tblColumns);

        [HttpGet]
        public async Task<JsonResult> List([Bind(Include = "iDisplayStart,iDisplayLength,iSortingCols,iSortCol_0,sSortDir_0,sSearch,sEcho")] DataTableParameters request)
        {
            var vincs =
                from cred in Domain.SIGSM_MicroArea_CredenciadoVinc
                join creds in Domain.AS_Credenciados
                on cred.CodCred equals creds.CodCred
                join vinc in Domain.AS_CredenciadosVinc
                on new { cred.NumContrato, cred.CodCred, cred.ItemVinc } equals new { vinc.NumContrato, vinc.CodCred, vinc.ItemVinc }
                join cont in Domain.ASSMED_Contratos
                on cred.NumContrato equals cont.NumContrato
                join mau in Domain.SIGSM_MicroArea_Unidade
                on cred.idMicroAreaUnidade equals mau.id
                join ma in Domain.SIGSM_MicroAreas
                on mau.MicroArea equals ma.Codigo
                join cad in Domain.ASSMED_Cadastro
                on creds.Codigo equals cad.Codigo
                join se in Domain.Setores
                on vinc.CodSetor equals se.CodSetor
                join sp in Domain.AS_SetoresPar
                on se.CodSetor equals sp.CodSetor
                join doc in Domain.ASSMED_CadastroDocPessoal
                on cad.Codigo equals doc.Codigo
                where se.DesSetor != null && cad.Nome != null
                && sp.CNES != null && doc.CodTpDocP == 6
                && doc.Numero != null && doc.Numero.Trim().Length == 15
                && sp.CNES.Trim().Length == 7
                select new ListagemMicroAreaCredenciadoViewModel
                {
                    Unidade = sp.CNES.Trim() + " - " + se.DesSetor.Trim(),
                    Nome = doc.Numero.Trim() + " - " + cad.Nome,
                    MicroArea = ma.Codigo + " - " + ma.Descricao,
                    btn = cred.id.ToString()
                };

            if (request == null) request = new DataTableParameters(Request.QueryString);

            request.Total = await Domain.SIGSM_MicroArea_CredenciadoVinc.CountAsync();

            Expression<Func<ListagemMicroAreaCredenciadoViewModel, object>> sort;

            var col = tblColumns.Count > request.iSortCol_0 ? tblColumns[request.iSortCol_0].Name : "MicroArea";

            if (col == "Unidade")
                sort = ((a) => a.Unidade);
            else if (col == "Nome")
                sort = ((a) => a.Nome);
            else
                sort = ((a) => a.MicroArea);

            var comp = request.Compose(vincs, sort,
                x => x.Unidade.Contains(request.sSearch) ||
                x.Nome.Contains(request.sSearch) ||
                x.MicroArea.Contains(request.sSearch), x => new
                {
                    x.Unidade,
                    x.Nome,
                    x.MicroArea,
                    btn = "<a data-ajax=\"true\" data-ajax-method=\"GET\" data-ajax-mode=\"replace-with\" data-ajax-update=\"#page-wrapper\" href=\"" + Url.Action("Edit", new { id = x.btn }) + "\" class=\"btn btn-outline btn-xs btn-warning\" title=\"Editar\" data-ajax-begin=\"beginRequest\"><i class='fa fa-pencil'></i></a>&nbsp;" +
                        "<a data-ajax=\"true\" data-ajax-method=\"GET\" data-ajax-mode=\"replace-with\" data-ajax-update=\"#page-wrapper\" href=\"" + Url.Action("Delete", new { id = x.btn }) + "\" class=\"btn btn-outline btn-xs btn-danger\" title=\"Remover\" data-ajax-begin=\"beginRequest\"><i class='fa fa-times'></i></a>"
                });

            return Json(await comp.Result(), JsonRequestBehavior.AllowGet);
        }

        public class CredSelect
        {
            public string Id { get; set; }
            public string Nome { get; set; }
        }

        public class MicroAreaSelect
        {
            public int id { get; set; }
            public string MicroArea { get; set; }
        }

        private IQueryable<CredSelect> Credenciados =>
            (from acv in Domain.AS_CredenciadosVinc
             join ac in Domain.AS_Credenciados
             on acv.CodCred equals ac.CodCred
             join acu in Domain.AS_CredenciadosUsu
             on ac.CodCred equals acu.CodCred
             join usu in Domain.ASSMED_Usuario
             on acu.CodUsuD equals usu.CodUsu
             join cad in Domain.ASSMED_Cadastro
             on ac.Codigo equals cad.Codigo
             join doc in Domain.ASSMED_CadastroDocPessoal
             on cad.Codigo equals doc.Codigo
             join mu in Domain.SIGSM_MicroArea_Unidade
             on acv.CodSetor equals mu.CodSetor
             join ma in Domain.SIGSM_MicroAreas
             on mu.MicroArea equals ma.Codigo
             join se in Domain.Setores
             on mu.CodSetor equals se.CodSetor
             join sp in Domain.AS_SetoresPar
             on se.CodSetor equals sp.CodSetor
             where doc.CodTpDocP == 6 && doc.Numero != null && doc.Numero.Trim().Length == 15
             && usu.Ativo == 1 && sp.CNES != null && sp.CNES.Trim().Length == 7
             orderby cad.Nome
             select new CredSelect
             {
                 Id = acv.ItemVinc + ":" + ac.CodCred,
                 Nome = doc.Numero.Trim() + " - " + cad.Nome + " / "
                 + sp.CNES + " - " + se.DesSetor
             }).Distinct();

        private IEnumerable<MicroAreaSelect> MicroAreaAssociada =>
            (from mu in Domain.SIGSM_MicroArea_Unidade
             join ma in Domain.SIGSM_MicroAreas
             on mu.MicroArea equals ma.Codigo
             join se in Domain.Setores
             on mu.CodSetor equals se.CodSetor
             join sp in Domain.AS_SetoresPar
             on se.CodSetor equals sp.CodSetor
             select new
             {
                 mu.id,
                 ma.Codigo,
                 ma.Descricao,
                 sp.CNES,
                 se.DesSetor
             }).ToList()
            .Select(x => new MicroAreaSelect
            {
                id = x.id,
                MicroArea = x.Codigo + " - " + x.Descricao + " / " + x.CNES + " - " + x.DesSetor
            }).Distinct();

        // GET: MicroAreaCredenciado/Create
        public ActionResult Create()
        {
            ViewBag.ItemVinc = new SelectList(Credenciados, "Id", "Nome");

            ViewBag.idMicroAreaUnidade = new SelectList(MicroAreaAssociada.OrderBy(x => x.MicroArea), "id", "MicroArea");
            return View();
        }

        // POST: MicroAreaCredenciado/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ItemVinc,idMicroAreaUnidade")] CadastroMicroAreaCredenciadoViewModel form)
        {
            var sIGSM_MicroArea_CredenciadoVinc = new SIGSM_MicroArea_CredenciadoVinc { };

            var item = form.ItemVinc?.Split(':') ?? new string[0];

            if (ModelState.IsValid && item.Length > 0)
            {
                var itemVinc = Convert.ToInt32(item[0]);
                var codCred = Convert.ToInt32(item[1]);

                sIGSM_MicroArea_CredenciadoVinc = new SIGSM_MicroArea_CredenciadoVinc
                {
                    idMicroAreaUnidade = Convert.ToInt32(form.idMicroAreaUnidade),
                    NumContrato = 22,
                    CodCred = codCred,
                    ItemVinc = itemVinc
                };

                if (!await Domain.SIGSM_MicroArea_CredenciadoVinc.AnyAsync(x => x.CodCred == codCred && x.ItemVinc == itemVinc
                 && x.idMicroAreaUnidade == sIGSM_MicroArea_CredenciadoVinc.idMicroAreaUnidade))
                {
                    Domain.SIGSM_MicroArea_CredenciadoVinc.Add(sIGSM_MicroArea_CredenciadoVinc);
                    await Domain.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "O Credenciado e a Micro Área informados já estão associados.");
            }
            else if (item.Length <= 0)
            {
                ModelState.AddModelError(nameof(form.ItemVinc), "Selecione um Credenciado");
            }

            ViewBag.ItemVinc = new SelectList(Credenciados, "Id", "Nome", form.ItemVinc);

            ViewBag.idMicroAreaUnidade = new SelectList(MicroAreaAssociada.OrderBy(x => x.MicroArea), "id", "MicroArea", sIGSM_MicroArea_CredenciadoVinc.idMicroAreaUnidade);

            return View(sIGSM_MicroArea_CredenciadoVinc);
        }

        // GET: MicroAreaCredenciado/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIGSM_MicroArea_CredenciadoVinc mcv = await Domain.SIGSM_MicroArea_CredenciadoVinc.FindAsync(id);
            if (mcv == null)
            {
                return HttpNotFound();
            }

            ViewBag.ItemVinc = new SelectList(Credenciados, "Id", "Nome", mcv.ItemVinc + ":" + mcv.CodCred);

            ViewBag.idMicroAreaUnidade = new SelectList(MicroAreaAssociada.OrderBy(x => x.MicroArea), "id", "MicroArea", mcv.idMicroAreaUnidade);

            return View(new CadastroMicroAreaCredenciadoViewModel
            {
                id = mcv.id,
                ItemVinc = mcv.ItemVinc + ":" + mcv.CodCred,
                idMicroAreaUnidade = mcv.idMicroAreaUnidade
            });
        }

        // POST: MicroAreaCredenciado/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,ItemVinc,idMicroAreaUnidade")] CadastroMicroAreaCredenciadoViewModel form)
        {
            var item = form.ItemVinc?.Split(':') ?? new string[0];

            SIGSM_MicroArea_CredenciadoVinc mcv = await Domain.SIGSM_MicroArea_CredenciadoVinc.FindAsync(form.id);

            if (ModelState.IsValid && item.Length > 0 && mcv != null)
            {
                var itemVinc = Convert.ToInt32(item[0]);
                var codCred = Convert.ToInt32(item[1]);
                var micro = form.idMicroAreaUnidade;

                if (!await Domain.SIGSM_MicroArea_CredenciadoVinc.AnyAsync(x => x.CodCred == codCred && x.ItemVinc == itemVinc
                 && x.idMicroAreaUnidade == micro && x.id != form.id))
                {
                    mcv.idMicroAreaUnidade = micro;
                    mcv.NumContrato = 22;
                    mcv.CodCred = codCred;
                    mcv.ItemVinc = itemVinc;

                    Domain.Entry(mcv).State = EntityState.Modified;
                    await Domain.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "O Credenciado e a Micro Área informados já estão associados.");
            }

            ViewBag.ItemVinc = new SelectList(Credenciados, "Id", "Nome", form.ItemVinc);

            ViewBag.idMicroAreaUnidade = new SelectList(MicroAreaAssociada, "id", "MicroArea", form.idMicroAreaUnidade);

            return View(form);
        }

        // GET: MicroAreaCredenciado/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIGSM_MicroArea_CredenciadoVinc sIGSM_MicroArea_CredenciadoVinc = await Domain.SIGSM_MicroArea_CredenciadoVinc.FindAsync(id);
            if (sIGSM_MicroArea_CredenciadoVinc == null)
            {
                return HttpNotFound();
            }

            var pessoa = Domain.ASSMED_Cadastro
                .Include(x => x.ASSMED_CadastroDocPessoal)
                .SingleOrDefault(x => x.Codigo == sIGSM_MicroArea_CredenciadoVinc.AS_Credenciados.Codigo);

            ViewBag.Credenciado = (pessoa.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 6 && x.Numero != null && x.Numero.Trim().Length == 15)?
                .Numero.Trim() ?? "") + " " + pessoa.Nome;

            return View(sIGSM_MicroArea_CredenciadoVinc);
        }

        // POST: MicroAreaCredenciado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SIGSM_MicroArea_CredenciadoVinc sIGSM_MicroArea_CredenciadoVinc = await Domain.SIGSM_MicroArea_CredenciadoVinc.FindAsync(id);

            if (sIGSM_MicroArea_CredenciadoVinc == null)
                return RedirectToAction("Index");

            if (!sIGSM_MicroArea_CredenciadoVinc.SIGSM_MicroArea_CredenciadoCidadao.Any())
            {
                Domain.SIGSM_MicroArea_CredenciadoVinc.Remove(sIGSM_MicroArea_CredenciadoVinc);
                await Domain.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Não é possível remover este registro, ele já possui um histórico de Download de Fichas.");

            var pessoa = Domain.ASSMED_Cadastro
                .Include(x => x.ASSMED_CadastroDocPessoal)
                .SingleOrDefault(x => x.Codigo == sIGSM_MicroArea_CredenciadoVinc.AS_Credenciados.Codigo);

            ViewBag.Credenciado = (pessoa.ASSMED_CadastroDocPessoal.FirstOrDefault(x => x.CodTpDocP == 6 && x.Numero != null && x.Numero.Trim().Length == 15)?
                .Numero.Trim() ?? "") + " " + pessoa.Nome;

            return View(sIGSM_MicroArea_CredenciadoVinc);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Domain.Dispose();
            }
            base.Dispose(disposing);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
