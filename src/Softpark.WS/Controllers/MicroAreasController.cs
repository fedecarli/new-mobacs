using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Softpark.Models;
using DataTables.AspNet.WebApi2;
using System.Linq;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Softpark.WS.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class MicroAreasController : BaseAjaxController
    {
        private readonly List<ColumnDef> tblColumns = new List<ColumnDef> {
                new ColumnDef { DataProp = "Codigo", Title = "Código" },
                new ColumnDef { DataProp = "Descricao", Title = "Descrição" },
                new ColumnDef { DataProp = "btn", Title = "", Sortable = false }
            };

        /// <summary>
        /// 
        /// </summary>
        public MicroAreasController() : base(new DomainContainer()) { }

        /// <summary>
        /// GET: MicroAreas
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() => View(tblColumns);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> List([Bind(Include = "iDisplayStart,iDisplayLength,iSortingCols,iSortCol_0,sSortDir_0,sSearch,sEcho")] DataTableParameters request)
        {
            if (request == null) request = new DataTableParameters(Request.QueryString);

            var vincs = Domain.SIGSM_MicroAreas.AsQueryable();

            request.Total = await Domain.SIGSM_MicroAreas.CountAsync();

            Expression<Func<SIGSM_MicroAreas, object>> sort;
            if (request.iSortCol_0 == 1)
                sort = ((a) => a.Descricao);
            else
                sort = ((a) => a.Codigo);

            var col = tblColumns.Count < request.iSortCol_0 ? tblColumns[request.iSortCol_0].Name : "Codigo";

            if (col == "Descricao")
                sort = ((a) => a.Descricao);
            else
                sort = ((a) => a.Codigo);

            var comp = request.Compose(vincs, sort,
                x => x.Codigo.Contains(request.sSearch) || x.Descricao.Contains(request.sSearch), x => new
            {
                x.Codigo,
                x.Descricao,
                btn = "<a data-ajax=\"true\" data-ajax-method=\"GET\" data-ajax-mode=\"replace-with\" data-ajax-update=\"#page-wrapper\" href=\"" + Url.Action("Edit", new { id = x.Codigo }) + "\" class=\"btn btn-outline btn-xs btn-warning\" title=\"Editar\" data-ajax-begin=\"beginRequest\"><i class='fa fa-pencil'></i></a>&nbsp;" +
                    "<a data-ajax=\"true\" data-ajax-method=\"GET\" data-ajax-mode=\"replace-with\" data-ajax-update=\"#page-wrapper\" href=\"" + Url.Action("Delete", new { id = x.Codigo }) + "\" class=\"btn btn-outline btn-xs btn-danger\" title=\"Remover\" data-ajax-begin=\"beginRequest\"><i class='fa fa-times'></i></a>"
                });

            return Json(await comp.Result(), JsonRequestBehavior.AllowGet);
        }

        /// GET: MicroAreas/Create
        [Route("MicroAreas/Create")]
        public ActionResult Create()
        {
            ViewBag.Edit = false;
            return View();
        }

        /// POST: MicroAreas/Create
        /// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        /// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("MicroAreas/Create")]
        public async Task<ActionResult> Create([Bind(Include = "Codigo,Descricao")] SIGSM_MicroAreas sIGSM_MicroAreas)
        {
            ViewBag.Edit = false;
            if (ModelState.IsValid)
            {
                if (await Domain.SIGSM_MicroAreas.AnyAsync(x => x.Codigo == sIGSM_MicroAreas.Codigo))
                {
                    ModelState.AddModelError(nameof(SIGSM_MicroAreas.Codigo), "Já existe uma Micro Área com este código.");
                }
                else
                {
                    Domain.SIGSM_MicroAreas.Add(sIGSM_MicroAreas);
                    await Domain.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }

            return View(sIGSM_MicroAreas);
        }

        /// GET: MicroAreas/Edit/5
        [Route("MicroAreas/Edit/{id}")]
        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.Edit = true;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIGSM_MicroAreas sIGSM_MicroAreas = await Domain.SIGSM_MicroAreas.FindAsync(id);
            if (sIGSM_MicroAreas == null)
            {
                return HttpNotFound();
            }
            return View(sIGSM_MicroAreas);
        }

        /// POST: MicroAreas/Edit/5
        /// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        /// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("MicroAreas/Edit/{id}")]
        public async Task<ActionResult> Edit([Bind(Include = "Codigo,Descricao")] SIGSM_MicroAreas sIGSM_MicroAreas)
        {
            ViewBag.Edit = true;
            if (ModelState.IsValid)
            {
                Domain.Entry(sIGSM_MicroAreas).State = EntityState.Modified;
                await Domain.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sIGSM_MicroAreas);
        }

        /// GET: MicroAreas/Delete/5
        [Route("MicroAreas/Delete/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIGSM_MicroAreas sIGSM_MicroAreas = await Domain.SIGSM_MicroAreas.FindAsync(id);
            if (sIGSM_MicroAreas == null)
            {
                return HttpNotFound();
            }
            return View(sIGSM_MicroAreas);
        }

        /// POST: MicroAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("MicroAreas/Delete/{id}")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            SIGSM_MicroAreas sIGSM_MicroAreas = await Domain.SIGSM_MicroAreas.FindAsync(id);
            
            if (sIGSM_MicroAreas == null)
                return RedirectToAction("Index");

            var assocs = 0;

            if (0 == (assocs = sIGSM_MicroAreas.SIGSM_MicroArea_Unidade.Count()))
            {
                Domain.SIGSM_MicroAreas.Remove(sIGSM_MicroAreas);
                await Domain.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", $"Não é possível remover este registro, ele possui {assocs} associação(ões) de Unidades(s).");

            return View(sIGSM_MicroAreas);
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
}
