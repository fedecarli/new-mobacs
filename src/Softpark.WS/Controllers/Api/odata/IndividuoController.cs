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
    builder.EntitySet<VW_INDIVIDUAIS>("Individuo");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class IndividuoController : ODataController
    {
        private DomainContainer db = new DomainContainer();

        // GET: odata/Individuo
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public IQueryable<VW_INDIVIDUAIS> GetIndividuo()
        {
            return db.VW_INDIVIDUAIS.Where(x => !x.NOME.Contains("*"));
        }

        // GET: odata/Individuo(5)
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public SingleResult<VW_INDIVIDUAIS> GetVW_INDIVIDUAIS([FromODataUri] decimal key)
        {
            return SingleResult.Create(db.VW_INDIVIDUAIS.Where(vW_INDIVIDUAIS => vW_INDIVIDUAIS.PK == key));
        }

        // PUT: odata/Individuo(5)
        public async Task<IHttpActionResult> Put([FromODataUri] decimal key, Delta<VW_INDIVIDUAIS> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            VW_INDIVIDUAIS vW_INDIVIDUAIS = await db.VW_INDIVIDUAIS.FindAsync(key);
            if (vW_INDIVIDUAIS == null)
            {
                return NotFound();
            }

            patch.Put(vW_INDIVIDUAIS);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await VW_INDIVIDUAISExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(vW_INDIVIDUAIS);
        }

        // POST: odata/Individuo
        public async Task<IHttpActionResult> Post(VW_INDIVIDUAIS vW_INDIVIDUAIS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VW_INDIVIDUAIS.Add(vW_INDIVIDUAIS);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (await VW_INDIVIDUAISExists(vW_INDIVIDUAIS.PK))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(vW_INDIVIDUAIS);
        }

        // PATCH: odata/Individuo(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] decimal key, Delta<VW_INDIVIDUAIS> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            VW_INDIVIDUAIS vW_INDIVIDUAIS = await db.VW_INDIVIDUAIS.FindAsync(key);
            if (vW_INDIVIDUAIS == null)
            {
                return NotFound();
            }

            patch.Patch(vW_INDIVIDUAIS);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await VW_INDIVIDUAISExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(vW_INDIVIDUAIS);
        }

        // DELETE: odata/Individuo(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] decimal key)
        {
            VW_INDIVIDUAIS vW_INDIVIDUAIS = await db.VW_INDIVIDUAIS.FindAsync(key);
            if (vW_INDIVIDUAIS == null)
            {
                return NotFound();
            }

            db.VW_INDIVIDUAIS.Remove(vW_INDIVIDUAIS);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private async Task<bool> VW_INDIVIDUAISExists(decimal key)
        {
            return await db.VW_INDIVIDUAIS.AnyAsync(e => e.PK == key);
        }
    }
}
