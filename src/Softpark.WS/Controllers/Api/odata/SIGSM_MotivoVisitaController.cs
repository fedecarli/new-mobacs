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

namespace Softpark.WS.Controllers.Api.odata
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Softpark.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<SIGSM_MotivoVisita>("SIGSM_MotivoVisita");
    builder.EntitySet<FichaVisitaDomiciliarChild>("FichaVisitaDomiciliarChild"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class SIGSM_MotivoVisitaController : ODataController
    {
        private DomainContainer db = new DomainContainer();

        // GET: odata/SIGSM_MotivoVisita
        [EnableQuery]
        public IQueryable<SIGSM_MotivoVisita> GetSIGSM_MotivoVisita()
        {
            return db.SIGSM_MotivoVisita;
        }

        // GET: odata/SIGSM_MotivoVisita(5)
        [EnableQuery]
        public SingleResult<SIGSM_MotivoVisita> GetSIGSM_MotivoVisita([FromODataUri] long key)
        {
            return SingleResult.Create(db.SIGSM_MotivoVisita.Where(sIGSM_MotivoVisita => sIGSM_MotivoVisita.codigo == key));
        }

        // GET: odata/SIGSM_MotivoVisita(5)/FichaVisitaDomiciliarChild
        [EnableQuery]
        public IQueryable<FichaVisitaDomiciliarChild> GetFichaVisitaDomiciliarChild([FromODataUri] long key)
        {
            return db.SIGSM_MotivoVisita.Where(m => m.codigo == key).SelectMany(m => m.FichaVisitaDomiciliarChild);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SIGSM_MotivoVisitaExists(long key)
        {
            return db.SIGSM_MotivoVisita.Count(e => e.codigo == key) > 0;
        }
    }
}
