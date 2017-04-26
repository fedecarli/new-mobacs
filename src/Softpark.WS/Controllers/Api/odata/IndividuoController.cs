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
    [Authorize]
    public class IndividuoController : ODataController
    {
        private DomainContainer db = new DomainContainer();

        // GET: odata/Individuo
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf | AllowedFunctions.Length,
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
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf | AllowedFunctions.Length,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public SingleResult<VW_INDIVIDUAIS> GetVW_INDIVIDUAIS([FromODataUri] decimal key)
        {
            return SingleResult.Create(db.VW_INDIVIDUAIS.Where(vW_INDIVIDUAIS => vW_INDIVIDUAIS.PK == key));
        }
    }
}
