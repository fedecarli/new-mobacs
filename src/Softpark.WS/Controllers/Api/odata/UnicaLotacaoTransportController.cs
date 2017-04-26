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
    builder.EntitySet<UnicaLotacaoTransport>("UnicaLotacaoTransport");
    builder.EntitySet<CadastroDomiciliar>("CadastroDomiciliar"); 
    builder.EntitySet<CadastroIndividual>("CadastroIndividual"); 
    builder.EntitySet<FichaVisitaDomiciliarMaster>("FichaVisitaDomiciliarMaster"); 
    builder.EntitySet<OrigemVisita>("OrigemVisita"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    [Authorize]
    public class UnicaLotacaoTransportController : ODataController
    {
        private DomainContainer db = new DomainContainer();

        // GET: odata/UnicaLotacaoTransport
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public IQueryable<UnicaLotacaoTransport> GetUnicaLotacaoTransport()
        {
            return db.UnicaLotacaoTransport;
        }

        // GET: odata/UnicaLotacaoTransport(5)
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public SingleResult<UnicaLotacaoTransport> GetUnicaLotacaoTransport([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.UnicaLotacaoTransport.Where(unicaLotacaoTransport => unicaLotacaoTransport.id == key));
        }

        // PUT: odata/UnicaLotacaoTransport(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<UnicaLotacaoTransport> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UnicaLotacaoTransport unicaLotacaoTransport = await db.UnicaLotacaoTransport.FindAsync(key);
            if (unicaLotacaoTransport == null)
            {
                return NotFound();
            }

            patch.Put(unicaLotacaoTransport);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnicaLotacaoTransportExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(unicaLotacaoTransport);
        }

        // POST: odata/UnicaLotacaoTransport
        public async Task<IHttpActionResult> Post(UnicaLotacaoTransport unicaLotacaoTransport)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UnicaLotacaoTransport.Add(unicaLotacaoTransport);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UnicaLotacaoTransportExists(unicaLotacaoTransport.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(unicaLotacaoTransport);
        }

        // PATCH: odata/UnicaLotacaoTransport(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<UnicaLotacaoTransport> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UnicaLotacaoTransport unicaLotacaoTransport = await db.UnicaLotacaoTransport.FindAsync(key);
            if (unicaLotacaoTransport == null)
            {
                return NotFound();
            }

            patch.Patch(unicaLotacaoTransport);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnicaLotacaoTransportExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(unicaLotacaoTransport);
        }

        // DELETE: odata/UnicaLotacaoTransport(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            UnicaLotacaoTransport unicaLotacaoTransport = await db.UnicaLotacaoTransport.FindAsync(key);
            if (unicaLotacaoTransport == null)
            {
                return NotFound();
            }

            db.UnicaLotacaoTransport.Remove(unicaLotacaoTransport);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/UnicaLotacaoTransport(5)/CadastroDomiciliar
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public IQueryable<CadastroDomiciliar> GetCadastroDomiciliar([FromODataUri] Guid key)
        {
            return db.UnicaLotacaoTransport.Where(m => m.id == key).SelectMany(m => m.CadastroDomiciliar);
        }

        // GET: odata/UnicaLotacaoTransport(5)/CadastroIndividual
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public IQueryable<CadastroIndividual> GetCadastroIndividual([FromODataUri] Guid key)
        {
            return db.UnicaLotacaoTransport.Where(m => m.id == key).SelectMany(m => m.CadastroIndividual);
        }

        // GET: odata/UnicaLotacaoTransport(5)/FichaVisitaDomiciliarMaster
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public IQueryable<FichaVisitaDomiciliarMaster> GetFichaVisitaDomiciliarMaster([FromODataUri] Guid key)
        {
            return db.UnicaLotacaoTransport.Where(m => m.id == key).SelectMany(m => m.FichaVisitaDomiciliarMaster);
        }

        // GET: odata/UnicaLotacaoTransport(5)/OrigemVisita
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public SingleResult<OrigemVisita> GetOrigemVisita([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.UnicaLotacaoTransport.Where(m => m.id == key).Select(m => m.OrigemVisita));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UnicaLotacaoTransportExists(Guid key)
        {
            return db.UnicaLotacaoTransport.Count(e => e.id == key) > 0;
        }
    }
}
