using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using Softpark.Models;
using System.Web.Http.OData.Query;
using System.Text.RegularExpressions;
using System;

namespace Softpark.WS.Controllers.Api.odata
{
    [Authorize, ODataRouting, RoutePrefix("api/odata/Visitas")]
    public class VisitasController : ODataController
    {
        private DomainContainer db = new DomainContainer();

        // GET: odata/Visitas
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public IQueryable<VW_VISITAS> GetVisitas()
        {
            return db.VW_VISITAS;
        }

        // GET: odata/Visitas/Masters
        /// <summary>
        /// odata/Visitas/Masters
        /// </summary>
        /// <returns></returns>
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        [HttpGet, Route("Masters")]
        public IQueryable<VW_VISITAS> Masters()
        {
            return db.VW_VISITAS.GroupBy(x => x.token).Select(x => x.FirstOrDefault()).AsQueryable();
        }

        // GET: odata/Visitas(5)
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public SingleResult<VW_VISITAS> GetVisitas([FromODataUri] long key)
        {
            return SingleResult.Create(db.VW_VISITAS.Where(visita => visita.childId == key));
        }
    }
}
