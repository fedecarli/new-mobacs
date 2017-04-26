using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
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
    builder.EntitySet<FichaVisitaDomiciliarMaster>("FichaVisitaDomiciliarMaster");
    builder.EntitySet<FichaVisitaDomiciliarChild>("FichaVisitaDomiciliarChild"); 
    builder.EntitySet<UnicaLotacaoTransport>("UnicaLotacaoTransport"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    [Authorize]
    public class FichaVisitaDomiciliarMasterController : ODataController
    {
        private DomainContainer db = new DomainContainer();

        // GET: odata/FichaVisitaDomiciliarMaster
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public IQueryable<FichaVisitaDomiciliarMaster> GetFichaVisitaDomiciliarMaster()
        {
            return db.FichaVisitaDomiciliarMaster;
        }

        // GET: odata/FichaVisitaDomiciliarMaster(5)
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public SingleResult<FichaVisitaDomiciliarMaster> GetFichaVisitaDomiciliarMaster([FromODataUri] string key)
        {
            return SingleResult.Create(db.FichaVisitaDomiciliarMaster.Where(fichaVisitaDomiciliarMaster => fichaVisitaDomiciliarMaster.uuidFicha == key));
        }

        // PUT: odata/FichaVisitaDomiciliarMaster(5)
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Delta<FichaVisitaDomiciliarMaster> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FichaVisitaDomiciliarMaster fichaVisitaDomiciliarMaster = await db.FichaVisitaDomiciliarMaster.FindAsync(key);
            if (fichaVisitaDomiciliarMaster == null)
            {
                return NotFound();
            }

            patch.Put(fichaVisitaDomiciliarMaster);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FichaVisitaDomiciliarMasterExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(fichaVisitaDomiciliarMaster);
        }

        // POST: odata/FichaVisitaDomiciliarMaster
        public async Task<IHttpActionResult> Post(FichaVisitaDomiciliarMaster fichaVisitaDomiciliarMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FichaVisitaDomiciliarMaster.Add(fichaVisitaDomiciliarMaster);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FichaVisitaDomiciliarMasterExists(fichaVisitaDomiciliarMaster.uuidFicha))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(fichaVisitaDomiciliarMaster);
        }

        // PATCH: odata/FichaVisitaDomiciliarMaster(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<FichaVisitaDomiciliarMaster> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FichaVisitaDomiciliarMaster fichaVisitaDomiciliarMaster = await db.FichaVisitaDomiciliarMaster.FindAsync(key);
            if (fichaVisitaDomiciliarMaster == null)
            {
                return NotFound();
            }

            patch.Patch(fichaVisitaDomiciliarMaster);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FichaVisitaDomiciliarMasterExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(fichaVisitaDomiciliarMaster);
        }

        // DELETE: odata/FichaVisitaDomiciliarMaster(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            FichaVisitaDomiciliarMaster fichaVisitaDomiciliarMaster = await db.FichaVisitaDomiciliarMaster.FindAsync(key);
            if (fichaVisitaDomiciliarMaster == null)
            {
                return NotFound();
            }

            db.FichaVisitaDomiciliarMaster.Remove(fichaVisitaDomiciliarMaster);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/FichaVisitaDomiciliarMaster(5)/FichaVisitaDomiciliarChild
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public IQueryable<FichaVisitaDomiciliarChild> GetFichaVisitaDomiciliarChild([FromODataUri] string key)
        {
            return db.FichaVisitaDomiciliarMaster.Where(m => m.uuidFicha == key).SelectMany(m => m.FichaVisitaDomiciliarChild);
        }

        // GET: odata/FichaVisitaDomiciliarMaster(5)/UnicaLotacaoTransport
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public SingleResult<UnicaLotacaoTransport> GetUnicaLotacaoTransport([FromODataUri] string key)
        {
            return SingleResult.Create(db.FichaVisitaDomiciliarMaster.Where(m => m.uuidFicha == key).Select(m => m.UnicaLotacaoTransport));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FichaVisitaDomiciliarMasterExists(string key)
        {
            return db.FichaVisitaDomiciliarMaster.Count(e => e.uuidFicha == key) > 0;
        }
    }
}
