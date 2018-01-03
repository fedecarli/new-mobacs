using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Softpark.Models;
using System.Linq;
using DataTables.AspNet.WebApi2;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using Softpark.WS.Validators;

namespace Softpark.WS.Controllers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class MicroAreaUnidadeController : BaseAjaxController
    {
        public MicroAreaUnidadeController() : base(new DomainContainer()) { }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public class TableList
        {
            public int id { get; set; }

            [ColumnDef(0, Sorting = SortingDirections.Ascending)]
            public string Setor { get; set; }

            [ColumnDef(1, Title = "Micro Área")]
            public string MicroArea { get; set; }

            [ColumnDef(2, Title = "", Sortable = false)]
            public string btn { get; set; }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        private readonly List<ColumnDef> tblColumns = ColumnDef.From<TableList>();

        /// GET: MicroAreaUnidade
        public ActionResult Index() => View(tblColumns);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> List([Bind(Include = "iDisplayStart,iDisplayLength,iSortingCols,iSortCol_0,sSortDir_0,sSearch,sEcho")] DataTableParameters request)
        {
            if (request == null) request = new DataTableParameters(Request.QueryString);

            var vincs = (from sp in Domain.AS_SetoresPar
                         join se in Domain.Setores
                         on sp.CodSetor equals se.CodSetor
                         join mu in Domain.SIGSM_MicroArea_Unidade
                         on se.CodSetor equals mu.CodSetor
                         join ma in Domain.SIGSM_MicroAreas
                         on mu.MicroArea equals ma.Codigo
                         where sp.CNES != null && se.DesSetor != null
                         select new TableList
                         {
                             id = mu.id,
                             MicroArea = ma.Codigo + " - " + ma.Descricao,
                             Setor = sp.CNES + " - " + se.DesSetor
                         }).Distinct();
            
            Expression<Func<TableList, object>> sort;

            var col = tblColumns.Count > request.iSortCol_0 ? tblColumns[request.iSortCol_0].Name : "MicroArea";

            if (col == "Setor")
                sort = ((a) => a.Setor);
            else
                sort = ((a) => a.MicroArea);

            var comp = request.Compose(vincs, sort,
                x => x.Setor.Contains(request.sSearch) || x.MicroArea.Contains(request.sSearch), x => new
                {
                    x.MicroArea,
                    x.Setor,
                    btn = "<a data-ajax=\"true\" data-ajax-method=\"GET\" data-ajax-mode=\"replace-with\" data-ajax-update=\"#page-wrapper\" href=\"" + Url.Action("Edit", new { x.id }) + "\" class=\"btn btn-outline btn-xs btn-warning\" title=\"Editar\" data-ajax-begin=\"beginRequest\"><i class='fa fa-pencil'></i></a>&nbsp;" +
                        "<a data-ajax=\"true\" data-ajax-method=\"GET\" data-ajax-mode=\"replace-with\" data-ajax-update=\"#page-wrapper\" href=\"" + Url.Action("Delete", new { x.id }) + "\" class=\"btn btn-outline btn-xs btn-danger\" title=\"Remover\" data-ajax-begin=\"beginRequest\"><i class='fa fa-times'></i></a>"
                });

            return Json(await comp.Result(), JsonRequestBehavior.AllowGet);
        }

        /// GET: MicroAreaUnidade/Create
        public ActionResult Create()
        {
            ViewBag.CodSetor = new SelectList(
                from x in Domain.AS_SetoresPar
                join s in Domain.Setores
                on x.CodSetor equals s.CodSetor
                where x.CNES != null && x.CNES.Trim().Length == 7 && s.DesSetor != null
                select new { s.CodSetor, DesSetor = x.CNES.Trim() + " - " + s.DesSetor.Trim() }, "CodSetor", "DesSetor");

            ViewBag.MicroArea = new SelectList(Domain.SIGSM_MicroAreas.Select(x => new
            {
                x.Codigo,
                Descricao = x.Codigo + " - " + x.Descricao
            }), "Codigo", "Descricao");
            return View();
        }

        /// POST: MicroAreaUnidade/Create
        /// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        /// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateActionParameters]
        public async Task<ActionResult> Create([Bind(Include = "MicroArea,NumContrato,CodSetor"), MAUValidation(ErrorMessage = "A Micro Área e a Unidade informadas já estão associadas.")] SIGSM_MicroArea_Unidade sIGSM_MicroArea_Unidade)
        {
            if (ModelState.IsValid)
            {
                if (!Domain.SIGSM_MicroArea_Unidade.Any(x => x.MicroArea == sIGSM_MicroArea_Unidade.MicroArea &&
                    x.CodSetor == sIGSM_MicroArea_Unidade.CodSetor))
                {
                    Domain.SIGSM_MicroArea_Unidade.Add(sIGSM_MicroArea_Unidade);
                    await Domain.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "A Micro Área e a Unidade informadas já estão associadas.");
            }

            ViewBag.CodSetor = new SelectList(
                from x in Domain.AS_SetoresPar
                join s in Domain.Setores
                on x.CodSetor equals s.CodSetor
                where x.CNES != null && x.CNES.Trim().Length == 7 && s.DesSetor != null
                select new { s.CodSetor, DesSetor = x.CNES.Trim() + " - " + s.DesSetor.Trim() }, "CodSetor", "DesSetor", sIGSM_MicroArea_Unidade.CodSetor);
            ViewBag.MicroArea = new SelectList(Domain.SIGSM_MicroAreas.Select(x => new
            {
                x.Codigo,
                Descricao = x.Codigo + " - " + x.Descricao
            }), "Codigo", "Descricao", sIGSM_MicroArea_Unidade.MicroArea);
            return View(sIGSM_MicroArea_Unidade);
        }

        /// GET: MicroAreaUnidade/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIGSM_MicroArea_Unidade sIGSM_MicroArea_Unidade = await Domain.SIGSM_MicroArea_Unidade.FindAsync(id);
            if (sIGSM_MicroArea_Unidade == null)
            {
                return HttpNotFound();
            }
            ViewBag.CodSetor = new SelectList(
                from x in Domain.AS_SetoresPar
                join s in Domain.Setores
                on x.CodSetor equals s.CodSetor
                where x.CNES != null && x.CNES.Trim().Length == 7 && s.DesSetor != null
                select new { s.CodSetor, DesSetor = x.CNES.Trim() + " - " + s.DesSetor.Trim() }, "CodSetor", "DesSetor", sIGSM_MicroArea_Unidade.CodSetor);
            ViewBag.MicroArea = new SelectList(Domain.SIGSM_MicroAreas.Select(x => new
            {
                x.Codigo,
                Descricao = x.Codigo + " - " + x.Descricao
            }), "Codigo", "Descricao", sIGSM_MicroArea_Unidade.MicroArea);
            return View(sIGSM_MicroArea_Unidade);
        }

        /// POST: MicroAreaUnidade/Edit/5
        /// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        /// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateActionParameters]
        public async Task<ActionResult> Edit([Bind(Include = "id,MicroArea,NumContrato,CodSetor"), MAUValidation(true, ErrorMessage = "A Micro Área e a Unidade informadas já estão associadas.")] SIGSM_MicroArea_Unidade sIGSM_MicroArea_Unidade)
        {
            if (ModelState.IsValid)
            {
                if (!Domain.SIGSM_MicroArea_Unidade.Any(x => x.MicroArea == sIGSM_MicroArea_Unidade.MicroArea &&
                 x.CodSetor == sIGSM_MicroArea_Unidade.CodSetor && x.id != sIGSM_MicroArea_Unidade.id))
                {
                    Domain.Entry(sIGSM_MicroArea_Unidade).State = EntityState.Modified;
                    await Domain.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("MicroArea", "A Micro Área e a Unidade informadas já estão associadas.");
            }
            ViewBag.CodSetor = new SelectList(
                from x in Domain.AS_SetoresPar
                join s in Domain.Setores
                on x.CodSetor equals s.CodSetor
                where x.CNES != null && x.CNES.Trim().Length == 7 && s.DesSetor != null
                select new { s.CodSetor, DesSetor = x.CNES.Trim() + " - " + s.DesSetor.Trim() }, "CodSetor", "DesSetor", sIGSM_MicroArea_Unidade.CodSetor);
            ViewBag.MicroArea = new SelectList(Domain.SIGSM_MicroAreas.Select(x => new
            {
                x.Codigo,
                Descricao = x.Codigo + " - " + x.Descricao
            }), "Codigo", "Descricao", sIGSM_MicroArea_Unidade.MicroArea);
            return View(sIGSM_MicroArea_Unidade);
        }

        /// GET: MicroAreaUnidade/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIGSM_MicroArea_Unidade sIGSM_MicroArea_Unidade = await Domain.SIGSM_MicroArea_Unidade.FindAsync(id);
            if (sIGSM_MicroArea_Unidade == null)
            {
                return HttpNotFound();
            }

            var unidade = (from a in Domain.AS_SetoresPar
                           where a.CodSetor == sIGSM_MicroArea_Unidade.CodSetor
                           select a)
                           .Include(a => a.Setores)
                          .FirstOrDefault();

            ViewBag.Unidade = unidade.CNES + " - " + unidade.Setores.DesSetor;

            return View(sIGSM_MicroArea_Unidade);
        }

        /// POST: MicroAreaUnidade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SIGSM_MicroArea_Unidade sIGSM_MicroArea_Unidade = await Domain.SIGSM_MicroArea_Unidade.FindAsync(id);

            if (sIGSM_MicroArea_Unidade == null)
                return RedirectToAction("Index");

            var assocs = 0;

            if (0 == (assocs = sIGSM_MicroArea_Unidade.SIGSM_MicroArea_CredenciadoVinc.Count()))
            {
                Domain.SIGSM_MicroArea_Unidade.Remove(sIGSM_MicroArea_Unidade);
                await Domain.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", $"Não é possível remover este registro, ele possui {assocs} associação(ões) de Credenciado(s).");

            return View(sIGSM_MicroArea_Unidade);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
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
