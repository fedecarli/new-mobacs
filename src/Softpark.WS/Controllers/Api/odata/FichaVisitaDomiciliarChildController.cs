using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using Softpark.Models;
using System.Web.Http.OData.Query;

namespace Softpark.WS.Controllers.Api.odata
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Softpark.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<FichaVisitaDomiciliarChild>("FichaVisitaDomiciliarChilds");
    builder.EntitySet<FichaVisitaDomiciliarMaster>("FichaVisitaDomiciliarMaster"); 
    builder.EntitySet<SIGSM_MotivoVisita>("SIGSM_MotivoVisita"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class FichaVisitaDomiciliarChildController : ODataController
    {
        private DomainContainer db = new DomainContainer();

        // GET: odata/FichaVisitaDomiciliarChild
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public IQueryable<FichaVisitaDomiciliarChild> GetFichaVisitaDomiciliarChild()
        {
            return db.FichaVisitaDomiciliarChild;
        }

        // GET: odata/FichaVisitaDomiciliarChild(5)
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public SingleResult<FichaVisitaDomiciliarChild> GetFichaVisitaDomiciliarChild([FromODataUri] long key)
        {
            return SingleResult.Create(db.FichaVisitaDomiciliarChild.Where(fichaVisitaDomiciliarChild => fichaVisitaDomiciliarChild.childId == key));
        }

        // PUT: odata/FichaVisitaDomiciliarChild(5)
        public async Task<IHttpActionResult> Put([FromODataUri] long key, Delta<FichaVisitaDomiciliarChild> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FichaVisitaDomiciliarChild fichaVisitaDomiciliarChild = await db.FichaVisitaDomiciliarChild.FindAsync(key);
            if (fichaVisitaDomiciliarChild == null)
            {
                return NotFound();
            }

            patch.Put(fichaVisitaDomiciliarChild);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FichaVisitaDomiciliarChildExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(fichaVisitaDomiciliarChild);
        }

        // POST: odata/FichaVisitaDomiciliarChild
        public async Task<IHttpActionResult> Post(FichaVisitaDomiciliarChild fichaVisitaDomiciliarChild)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FichaVisitaDomiciliarChild.Add(fichaVisitaDomiciliarChild);
            await db.SaveChangesAsync();

            return Created(fichaVisitaDomiciliarChild);
        }

        // PATCH: odata/FichaVisitaDomiciliarChild(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] long key, Delta<FichaVisitaDomiciliarChild> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FichaVisitaDomiciliarChild fichaVisitaDomiciliarChild = await db.FichaVisitaDomiciliarChild.FindAsync(key);
            if (fichaVisitaDomiciliarChild == null)
            {
                return NotFound();
            }

            patch.Patch(fichaVisitaDomiciliarChild);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FichaVisitaDomiciliarChildExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(fichaVisitaDomiciliarChild);
        }

        // DELETE: odata/FichaVisitaDomiciliarChild(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            FichaVisitaDomiciliarChild fichaVisitaDomiciliarChild = await db.FichaVisitaDomiciliarChild.FindAsync(key);
            if (fichaVisitaDomiciliarChild == null)
            {
                return NotFound();
            }

            db.FichaVisitaDomiciliarChild.Remove(fichaVisitaDomiciliarChild);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/FichaVisitaDomiciliarChild(5)/FichaVisitaDomiciliarMaster
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public SingleResult<FichaVisitaDomiciliarMaster> GetFichaVisitaDomiciliarMaster([FromODataUri] long key)
        {
            return SingleResult.Create(db.FichaVisitaDomiciliarChild.Where(m => m.childId == key).Select(m => m.FichaVisitaDomiciliarMaster));
        }

        // GET: odata/FichaVisitaDomiciliarChild(5)/SIGSM_MotivoVisita
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public IQueryable<SIGSM_MotivoVisita> GetSIGSM_MotivoVisita([FromODataUri] long key)
        {
            return db.FichaVisitaDomiciliarChild.Where(m => m.childId == key).SelectMany(m => m.SIGSM_MotivoVisita);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FichaVisitaDomiciliarChildExists(long key)
        {
            return db.FichaVisitaDomiciliarChild.Count(e => e.childId == key) > 0;
        }
    }
}
