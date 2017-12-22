using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Softpark.Models;

namespace Softpark.WS.Controllers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class PreparoMobileController : BaseAjaxController
    {
        public PreparoMobileController() : base(new DomainContainer()) { }

        // GET: PreparoMobile
        public async Task<ActionResult> Index()
        {
            var sIGSM_Check_Cadastros = Domain.SIGSM_Check_Cadastros.Include(s => s.AS_Credenciados).Include(s => s.ASSMED_Cadastro).Include(s => s.ASSMED_Contratos);
            return View(await sIGSM_Check_Cadastros.ToListAsync());
        }

        // GET: PreparoMobile/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIGSM_Check_Cadastros sIGSM_Check_Cadastros = await Domain.SIGSM_Check_Cadastros.FindAsync(id);
            if (sIGSM_Check_Cadastros == null)
            {
                return HttpNotFound();
            }
            return View(sIGSM_Check_Cadastros);
        }

        // GET: PreparoMobile/Create
        public ActionResult Create()
        {
            ViewBag.NumContrato = new SelectList(Domain.AS_Credenciados, "NumContrato", "CNES");
            ViewBag.NumContrato = new SelectList(Domain.ASSMED_Cadastro, "NumContrato", "Tipo");
            ViewBag.NumContrato = new SelectList(Domain.ASSMED_Contratos, "NumContrato", "NomeContratante");
            return View();
        }

        // POST: PreparoMobile/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,NumContrato,Codigo,CodCred,BaixarEndereco,Data,DataDownload")] SIGSM_Check_Cadastros sIGSM_Check_Cadastros)
        {
            if (ModelState.IsValid)
            {
                sIGSM_Check_Cadastros.id = Guid.NewGuid();
                Domain.SIGSM_Check_Cadastros.Add(sIGSM_Check_Cadastros);
                await Domain.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.NumContrato = new SelectList(Domain.AS_Credenciados, "NumContrato", "CNES", sIGSM_Check_Cadastros.NumContrato);
            ViewBag.NumContrato = new SelectList(Domain.ASSMED_Cadastro, "NumContrato", "Tipo", sIGSM_Check_Cadastros.NumContrato);
            ViewBag.NumContrato = new SelectList(Domain.ASSMED_Contratos, "NumContrato", "NomeContratante", sIGSM_Check_Cadastros.NumContrato);
            return View(sIGSM_Check_Cadastros);
        }

        // GET: PreparoMobile/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIGSM_Check_Cadastros sIGSM_Check_Cadastros = await Domain.SIGSM_Check_Cadastros.FindAsync(id);
            if (sIGSM_Check_Cadastros == null)
            {
                return HttpNotFound();
            }
            ViewBag.NumContrato = new SelectList(Domain.AS_Credenciados, "NumContrato", "CNES", sIGSM_Check_Cadastros.NumContrato);
            ViewBag.NumContrato = new SelectList(Domain.ASSMED_Cadastro, "NumContrato", "Tipo", sIGSM_Check_Cadastros.NumContrato);
            ViewBag.NumContrato = new SelectList(Domain.ASSMED_Contratos, "NumContrato", "NomeContratante", sIGSM_Check_Cadastros.NumContrato);
            return View(sIGSM_Check_Cadastros);
        }

        // POST: PreparoMobile/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,NumContrato,Codigo,CodCred,BaixarEndereco,Data,DataDownload")] SIGSM_Check_Cadastros sIGSM_Check_Cadastros)
        {
            if (ModelState.IsValid)
            {
                Domain.Entry(sIGSM_Check_Cadastros).State = EntityState.Modified;
                await Domain.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.NumContrato = new SelectList(Domain.AS_Credenciados, "NumContrato", "CNES", sIGSM_Check_Cadastros.NumContrato);
            ViewBag.NumContrato = new SelectList(Domain.ASSMED_Cadastro, "NumContrato", "Tipo", sIGSM_Check_Cadastros.NumContrato);
            ViewBag.NumContrato = new SelectList(Domain.ASSMED_Contratos, "NumContrato", "NomeContratante", sIGSM_Check_Cadastros.NumContrato);
            return View(sIGSM_Check_Cadastros);
        }

        // GET: PreparoMobile/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIGSM_Check_Cadastros sIGSM_Check_Cadastros = await Domain.SIGSM_Check_Cadastros.FindAsync(id);
            if (sIGSM_Check_Cadastros == null)
            {
                return HttpNotFound();
            }
            return View(sIGSM_Check_Cadastros);
        }

        // POST: PreparoMobile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            SIGSM_Check_Cadastros sIGSM_Check_Cadastros = await Domain.SIGSM_Check_Cadastros.FindAsync(id);
            Domain.SIGSM_Check_Cadastros.Remove(sIGSM_Check_Cadastros);
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
