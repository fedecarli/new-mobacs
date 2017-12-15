using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Softpark.Models;

namespace Softpark.WS.Controllers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class MicroAreaUnidadeController : BaseAjaxController
    {
        public MicroAreaUnidadeController() : base(new DomainContainer()) { }

        // GET: MicroAreaUnidade
        public async Task<ActionResult> Index()
        {
            var sIGSM_MicroArea_Unidade = Domain.SIGSM_MicroArea_Unidade.Include(s => s.ASSMED_Contratos).Include(s => s.Setores).Include(s => s.SIGSM_MicroAreas);
            return View(await sIGSM_MicroArea_Unidade.ToListAsync());
        }

        // GET: MicroAreaUnidade/Details/5
        public async Task<ActionResult> Details(int? id)
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
            return View(sIGSM_MicroArea_Unidade);
        }

        // GET: MicroAreaUnidade/Create
        public ActionResult Create()
        {
            ViewBag.NumContrato = new SelectList(Domain.ASSMED_Contratos, "NumContrato", "NomeContratante");
            ViewBag.CodSetor = new SelectList(Domain.Setores, "CodSetor", "DesSetor");
            ViewBag.MicroArea = new SelectList(Domain.SIGSM_MicroAreas, "Codigo", "Descricao");
            return View();
        }

        // POST: MicroAreaUnidade/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,MicroArea,NumContrato,CodSetor")] SIGSM_MicroArea_Unidade sIGSM_MicroArea_Unidade)
        {
            if (ModelState.IsValid)
            {
                Domain.SIGSM_MicroArea_Unidade.Add(sIGSM_MicroArea_Unidade);
                await Domain.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.NumContrato = new SelectList(Domain.ASSMED_Contratos, "NumContrato", "NomeContratante", sIGSM_MicroArea_Unidade.NumContrato);
            ViewBag.CodSetor = new SelectList(Domain.Setores, "CodSetor", "DesSetor", sIGSM_MicroArea_Unidade.CodSetor);
            ViewBag.MicroArea = new SelectList(Domain.SIGSM_MicroAreas, "Codigo", "Descricao", sIGSM_MicroArea_Unidade.MicroArea);
            return View(sIGSM_MicroArea_Unidade);
        }

        // GET: MicroAreaUnidade/Edit/5
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
            ViewBag.NumContrato = new SelectList(Domain.ASSMED_Contratos, "NumContrato", "NomeContratante", sIGSM_MicroArea_Unidade.NumContrato);
            ViewBag.CodSetor = new SelectList(Domain.Setores, "CodSetor", "DesSetor", sIGSM_MicroArea_Unidade.CodSetor);
            ViewBag.MicroArea = new SelectList(Domain.SIGSM_MicroAreas, "Codigo", "Descricao", sIGSM_MicroArea_Unidade.MicroArea);
            return View(sIGSM_MicroArea_Unidade);
        }

        // POST: MicroAreaUnidade/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,MicroArea,NumContrato,CodSetor")] SIGSM_MicroArea_Unidade sIGSM_MicroArea_Unidade)
        {
            if (ModelState.IsValid)
            {
                Domain.Entry(sIGSM_MicroArea_Unidade).State = EntityState.Modified;
                await Domain.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.NumContrato = new SelectList(Domain.ASSMED_Contratos, "NumContrato", "NomeContratante", sIGSM_MicroArea_Unidade.NumContrato);
            ViewBag.CodSetor = new SelectList(Domain.Setores, "CodSetor", "DesSetor", sIGSM_MicroArea_Unidade.CodSetor);
            ViewBag.MicroArea = new SelectList(Domain.SIGSM_MicroAreas, "Codigo", "Descricao", sIGSM_MicroArea_Unidade.MicroArea);
            return View(sIGSM_MicroArea_Unidade);
        }

        // GET: MicroAreaUnidade/Delete/5
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
            return View(sIGSM_MicroArea_Unidade);
        }

        // POST: MicroAreaUnidade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SIGSM_MicroArea_Unidade sIGSM_MicroArea_Unidade = await Domain.SIGSM_MicroArea_Unidade.FindAsync(id);
            Domain.SIGSM_MicroArea_Unidade.Remove(sIGSM_MicroArea_Unidade);
            await Domain.SaveChangesAsync();
            return RedirectToAction("Index");
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
