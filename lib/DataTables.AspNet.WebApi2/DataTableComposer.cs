using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Linq.Expressions;

namespace DataTables.AspNet.WebApi2
{
    public class DataTableComposer<T>
    {
        public DataTableParameters Parameters { get; }
        public IQueryable<T> Query { get; }
        public Expression<Func<T, object>> Sort { get; }
        public Expression<Func<T, bool>> Search { get; }
        public Expression<Func<T, object>> Select { get; }

        private DataTableComposer(Expression<Func<T, object>> select) =>
            Select = select;

        private DataTableComposer(Expression<Func<T, bool>> search, Expression<Func<T, object>> select) : this(select) =>
            Search = search;

        private DataTableComposer(Expression<Func<T, object>> sort, Expression<Func<T, bool>> search,
            Expression<Func<T, object>> select) : this(search, select) =>
            Sort = sort;

        private DataTableComposer(IQueryable<T> queryable, Expression<Func<T, object>> sort,
            Expression<Func<T, bool>> search, Expression<Func<T, object>> select) : this(sort, search, select) =>
            Query = queryable;

        public DataTableComposer(DataTableParameters parameters, IQueryable<T> queryable,
            Expression<Func<T, object>> sort, Expression<Func<T, bool>> search,
            Expression<Func<T, object>> select) : this(queryable, sort, search, select) =>
            Parameters = parameters;

        public async Task<DataTableResult> Result()
        {
            var dataTableResult = new DataTableResult();

            int iTotalRecords = Parameters.Total;
            int iTotalDisplayRecords = iTotalRecords;

            var qry = Query;

            if (!string.IsNullOrEmpty(Parameters.sSearch?.Trim()))
                qry = qry.Where(Search);

            qry = (await qry.ToListAsync()).AsQueryable();

            iTotalRecords = qry.Count();

            var ordered = Parameters.sSortDir_0 == "asc" ?
                qry.OrderBy(Sort) :
                qry.OrderByDescending(Sort);
            
            var query = ordered
                        .Skip(Parameters.iDisplayStart)
                        .Take(Parameters.iDisplayLength);

            dataTableResult.aaData = query.Select(Select).ToArray();
            dataTableResult.iTotalRecords = dataTableResult.aaData.Length;
            dataTableResult.iTotalDisplayRecords = iTotalRecords;
            dataTableResult.sEcho = Parameters.sEcho.ToString();

            return dataTableResult;
        }
    }
}
