using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Softpark.Models;
using System.Linq;
using System.Web.Mvc.Ajax;
using DataTables.AspNet.WebApi2;

namespace Softpark.WS.Controllers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class MicroAreaCredenciadoController : BaseAjaxController
    {
        public MicroAreaCredenciadoController() : base(new DomainContainer()) { }

        // GET: MicroAreaCredenciado
        public async Task<ActionResult> Index()
        {
            var sIGSM_MicroArea_CredenciadoVinc = Domain.SIGSM_MicroArea_CredenciadoVinc.Include(s => s.AS_Credenciados).Include(s => s.AS_CredenciadosVinc).Include(s => s.ASSMED_Contratos).Include(s => s.SIGSM_MicroArea_Unidade);
            return View(await sIGSM_MicroArea_CredenciadoVinc.ToListAsync());
        }

        [HttpGet]
        public async Task<JsonResult> List()
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
                select new
                {
                    Unidade = se.DesSetor,
                    Nome = cad.Nome,
                    MicroArea = ma.Codigo + " - " + ma.Descricao,
                    btn = cred.id.ToString()
                };

            DataTableParameters dataTableParameters = new DataTableParameters(Request.QueryString);
            DataTableResult dataTableResult = new DataTableResult();
            int iTotalRecords = await Domain.SIGSM_MicroArea_CredenciadoVinc.CountAsync();
            int iTotalDisplayRecords = iTotalRecords;

            var query = await vincs.OrderBy(x => x.MicroArea)
                        .Skip(dataTableParameters.DisplayStart)
                        .Take(dataTableParameters.DisplayLength)
                        .ToListAsync();

            dataTableResult.aaData = query.Select(x => new
            {
                Unidade = x.Unidade,
                Nome = x.Nome,
                MicroArea = x.MicroArea,
                btn = "<a data-ajax=\"true\" data-ajax-method=\"GET\" data-ajax-mode=\"replace-with\" data-ajax-update=\"#page-wrapper\" href=\"" + Url.Action("Edit", new { id = x.btn }) + "\">Editar<a> | " +
                    "<a data-ajax=\"true\" data-ajax-method=\"GET\" data-ajax-mode=\"replace-with\" data-ajax-update=\"#page-wrapper\" href=\"" + Url.Action("Delete", new { id = x.btn }) + "\">Remover<a>"
            }).ToArray();
            dataTableResult.sEcho = dataTableParameters.Echo;
            dataTableResult.iTotalRecords = iTotalRecords;
            dataTableResult.iTotalDisplayRecords = iTotalDisplayRecords;

            return Json(dataTableResult, JsonRequestBehavior.AllowGet);
        }

        // GET: MicroAreaCredenciado/Details/5
        public async Task<ActionResult> Details(int? id)
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
            return View(sIGSM_MicroArea_CredenciadoVinc);
        }

        // GET: MicroAreaCredenciado/Create
        public ActionResult Create()
        {
            ViewBag.CodCred = new SelectList(Domain.AS_Credenciados, "CodCred", "CNES");
            ViewBag.ItemVinc = new SelectList(Domain.AS_CredenciadosVinc, "Id", "CNESLocal");
            ViewBag.NumContrato = new SelectList(Domain.ASSMED_Contratos, "NumContrato", "NomeContratante");
            ViewBag.idMicroAreaUnidade = new SelectList(Domain.SIGSM_MicroArea_Unidade, "id", "MicroArea");
            return View();
        }

        // POST: MicroAreaCredenciado/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,idMicroAreaUnidade,NumContrato,CodCred,ItemVinc")] SIGSM_MicroArea_CredenciadoVinc sIGSM_MicroArea_CredenciadoVinc)
        {
            if (ModelState.IsValid)
            {
                Domain.SIGSM_MicroArea_CredenciadoVinc.Add(sIGSM_MicroArea_CredenciadoVinc);
                await Domain.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CodCred = new SelectList(Domain.AS_Credenciados, "CodCred", "CNES", sIGSM_MicroArea_CredenciadoVinc.CodCred);
            ViewBag.ItemVinc = new SelectList(Domain.AS_CredenciadosVinc, "Id", "CNESLocal", sIGSM_MicroArea_CredenciadoVinc.ItemVinc);
            ViewBag.NumContrato = new SelectList(Domain.ASSMED_Contratos, "NumContrato", "NomeContratante", sIGSM_MicroArea_CredenciadoVinc.NumContrato);
            ViewBag.idMicroAreaUnidade = new SelectList(Domain.SIGSM_MicroArea_Unidade, "id", "MicroArea", sIGSM_MicroArea_CredenciadoVinc.idMicroAreaUnidade);
            return View(sIGSM_MicroArea_CredenciadoVinc);
        }

        // GET: MicroAreaCredenciado/Edit/5
        public async Task<ActionResult> Edit(int? id)
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

            ViewBag.CodCred = new SelectList(Domain.AS_Credenciados, "CodCred", "CNES", sIGSM_MicroArea_CredenciadoVinc.CodCred);
            ViewBag.ItemVinc = new SelectList(Domain.AS_CredenciadosVinc, "Id", "CNESLocal", sIGSM_MicroArea_CredenciadoVinc.ItemVinc);
            ViewBag.NumContrato = new SelectList(Domain.ASSMED_Contratos, "NumContrato", "NomeContratante", sIGSM_MicroArea_CredenciadoVinc.NumContrato);
            ViewBag.idMicroAreaUnidade = new SelectList(Domain.SIGSM_MicroArea_Unidade, "id", "MicroArea", sIGSM_MicroArea_CredenciadoVinc.idMicroAreaUnidade);
            return View(sIGSM_MicroArea_CredenciadoVinc);
        }

        // POST: MicroAreaCredenciado/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,idMicroAreaUnidade,NumContrato,CodCred,ItemVinc")] SIGSM_MicroArea_CredenciadoVinc sIGSM_MicroArea_CredenciadoVinc)
        {
            if (ModelState.IsValid)
            {
                Domain.Entry(sIGSM_MicroArea_CredenciadoVinc).State = EntityState.Modified;
                await Domain.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CodCred = new SelectList(Domain.AS_Credenciados, "CodCred", "CNES", sIGSM_MicroArea_CredenciadoVinc.CodCred);
            ViewBag.ItemVinc = new SelectList(Domain.AS_CredenciadosVinc, "Id", "CNESLocal", sIGSM_MicroArea_CredenciadoVinc.ItemVinc);
            ViewBag.NumContrato = new SelectList(Domain.ASSMED_Contratos, "NumContrato", "NomeContratante", sIGSM_MicroArea_CredenciadoVinc.NumContrato);
            ViewBag.idMicroAreaUnidade = new SelectList(Domain.SIGSM_MicroArea_Unidade, "id", "MicroArea", sIGSM_MicroArea_CredenciadoVinc.idMicroAreaUnidade);
            return View(sIGSM_MicroArea_CredenciadoVinc);
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
            return View(sIGSM_MicroArea_CredenciadoVinc);
        }

        // POST: MicroAreaCredenciado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SIGSM_MicroArea_CredenciadoVinc sIGSM_MicroArea_CredenciadoVinc = await Domain.SIGSM_MicroArea_CredenciadoVinc.FindAsync(id);
            Domain.SIGSM_MicroArea_CredenciadoVinc.Remove(sIGSM_MicroArea_CredenciadoVinc);
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
