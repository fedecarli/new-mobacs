using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Softpark.Models;

namespace Softpark.WS.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class MicroAreasController : BaseAjaxController
    {
        /// <summary>
        /// 
        /// </summary>
        public MicroAreasController() : base(new DomainContainer()) { }

        /// <summary>
        /// GET: MicroAreas
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            return View(await Domain.SIGSM_MicroAreas.ToListAsync());
        }

        /// GET: MicroAreas/Create
        [Route("MicroAreas/Create")]
        public ActionResult Create()
        {
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
            if (ModelState.IsValid)
            {
                Domain.SIGSM_MicroAreas.Add(sIGSM_MicroAreas);
                await Domain.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sIGSM_MicroAreas);
        }

        /// GET: MicroAreas/Edit/5
        [Route("MicroAreas/Edit/{id}")]
        public async Task<ActionResult> Edit(string id)
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

        /// POST: MicroAreas/Edit/5
        /// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        /// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("MicroAreas/Edit/{id}")]
        public async Task<ActionResult> Edit([Bind(Include = "Codigo,Descricao")] SIGSM_MicroAreas sIGSM_MicroAreas)
        {
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
            Domain.SIGSM_MicroAreas.Remove(sIGSM_MicroAreas);
            await Domain.SaveChangesAsync();
            return RedirectToAction("Index");
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
