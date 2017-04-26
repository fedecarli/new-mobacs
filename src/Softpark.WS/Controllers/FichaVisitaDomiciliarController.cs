using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Softpark.Models;

namespace Softpark.WS.Controllers
{
    [Authorize]
    public class FichaVisitaDomiciliarController : Controller
    {
        private DomainContainer db = new DomainContainer();

        // GET: FichaVisitaDomiciliar
        public async Task<ActionResult> Index()
        {
            var fichaVisitaDomiciliarChild = db.FichaVisitaDomiciliarChild.Include(f => f.FichaVisitaDomiciliarMaster);
            return View(await fichaVisitaDomiciliarChild.ToListAsync());
        }

        // GET: FichaVisitaDomiciliar/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FichaVisitaDomiciliarChild fichaVisitaDomiciliarChild = await db.FichaVisitaDomiciliarChild.FindAsync(id);
            if (fichaVisitaDomiciliarChild == null)
            {
                return HttpNotFound();
            }
            return View(fichaVisitaDomiciliarChild);
        }

        // GET: FichaVisitaDomiciliar/Create
        public ActionResult Create()
        {
            ViewBag.uuidFicha = new SelectList(db.FichaVisitaDomiciliarMaster, "uuidFicha", "uuidFicha");
            return View();
        }

        // POST: FichaVisitaDomiciliar/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "childId,uuidFicha,turno,numProntuario,cnsCidadao,dtNascimento,sexo,statusVisitaCompartilhadaOutroProfissional,desfecho,microarea,stForaArea,tipoDeImovel,pesoAcompanhamentoNutricional,alturaAcompanhamentoNutricional")] FichaVisitaDomiciliarChild fichaVisitaDomiciliarChild)
        {
            if (ModelState.IsValid)
            {
                db.FichaVisitaDomiciliarChild.Add(fichaVisitaDomiciliarChild);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.uuidFicha = new SelectList(db.FichaVisitaDomiciliarMaster, "uuidFicha", "uuidFicha", fichaVisitaDomiciliarChild.uuidFicha);
            return View(fichaVisitaDomiciliarChild);
        }

        // GET: FichaVisitaDomiciliar/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FichaVisitaDomiciliarChild fichaVisitaDomiciliarChild = await db.FichaVisitaDomiciliarChild.FindAsync(id);
            if (fichaVisitaDomiciliarChild == null)
            {
                return HttpNotFound();
            }
            ViewBag.uuidFicha = new SelectList(db.FichaVisitaDomiciliarMaster, "uuidFicha", "uuidFicha", fichaVisitaDomiciliarChild.uuidFicha);
            return View(fichaVisitaDomiciliarChild);
        }

        // POST: FichaVisitaDomiciliar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "childId,uuidFicha,turno,numProntuario,cnsCidadao,dtNascimento,sexo,statusVisitaCompartilhadaOutroProfissional,desfecho,microarea,stForaArea,tipoDeImovel,pesoAcompanhamentoNutricional,alturaAcompanhamentoNutricional")] FichaVisitaDomiciliarChild fichaVisitaDomiciliarChild)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fichaVisitaDomiciliarChild).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.uuidFicha = new SelectList(db.FichaVisitaDomiciliarMaster, "uuidFicha", "uuidFicha", fichaVisitaDomiciliarChild.uuidFicha);
            return View(fichaVisitaDomiciliarChild);
        }

        // GET: FichaVisitaDomiciliar/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FichaVisitaDomiciliarChild fichaVisitaDomiciliarChild = await db.FichaVisitaDomiciliarChild.FindAsync(id);
            if (fichaVisitaDomiciliarChild == null)
            {
                return HttpNotFound();
            }
            return View(fichaVisitaDomiciliarChild);
        }

        // POST: FichaVisitaDomiciliar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            FichaVisitaDomiciliarChild fichaVisitaDomiciliarChild = await db.FichaVisitaDomiciliarChild.FindAsync(id);
            db.FichaVisitaDomiciliarChild.Remove(fichaVisitaDomiciliarChild);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
