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
        public int Total { get; set; } = -1;
        private bool _ordered = false;

        private DataTableComposer(Expression<Func<T, object>> select = null) =>
            Select = select ?? (a => a);

        private DataTableComposer(Expression<Func<T, bool>> search, Expression<Func<T, object>> select = null) :
            this(select) =>
            Search = search;

        private DataTableComposer(Expression<Func<T, object>> sort, Expression<Func<T, bool>> search,
            Expression<Func<T, object>> select = null) : this(search, select) =>
            Sort = sort;

        private DataTableComposer(IQueryable<T> queryable, Expression<Func<T, object>> sort,
            Expression<Func<T, bool>> search, Expression<Func<T, object>> select = null) : this(sort, search, select) =>
            Query = queryable;

        private DataTableComposer(IOrderedQueryable<T> queryable, Expression<Func<T, object>> sort,
            Expression<Func<T, bool>> search, Expression<Func<T, object>> select = null) : this(sort, search, select) =>
            _ordered = (Query = queryable) != null;

        public DataTableComposer(DataTableParameters parameters, IQueryable<T> queryable,
            Expression<Func<T, object>> sort, Expression<Func<T, bool>> search,
            Expression<Func<T, object>> select = null) : this(queryable, sort, search, select) =>
            Parameters = parameters;

        public DataTableComposer(DataTableParameters parameters, IOrderedQueryable<T> queryable,
            Expression<Func<T, object>> sort, Expression<Func<T, bool>> search,
            Expression<Func<T, object>> select = null) : this(queryable, sort, search, select) =>
            Parameters = parameters;

        public async Task<DataTableResult> Result()
        {
            var dataTableResult = new DataTableResult();

            int iTotalRecords = Parameters.Total;
            int iTotalDisplayRecords = iTotalRecords;

            var qry = Query;

            if (!string.IsNullOrEmpty(Parameters.sSearch?.Trim()))
                qry = qry.Where(Search);

            iTotalRecords = await qry.CountAsync();

            var ordered = _ordered ? Query as IOrderedQueryable<T> :
                Sort == null ? qry.OrderBy(x => x) : Parameters.sSortDir_0 == "asc" ?
                qry.OrderBy(Sort) :
                qry.OrderByDescending(Sort);

            qry = ordered
                        .Skip(Parameters.iDisplayStart)
                        .Take(Parameters.iDisplayLength);

            var query = await qry.ToArrayAsync();

            dataTableResult.aaData = query.AsQueryable().Select(Select).ToArray();
            dataTableResult.iTotalRecords = dataTableResult.aaData.Length;
            dataTableResult.iTotalDisplayRecords = iTotalRecords;
            dataTableResult.sEcho = Parameters.sEcho.ToString();

            return dataTableResult;
        }

        public async Task<DataTableResult> ResultCompositeOrder(Func<IOrderedQueryable<T>, IOrderedQueryable<T>> fo)
        {
            var dataTableResult = new DataTableResult();

            int iTotalRecords = Parameters.Total;
            int iTotalDisplayRecords = iTotalRecords;

            var qry = Query;

            if (!string.IsNullOrEmpty(Parameters.sSearch?.Trim()))
                qry = qry.Where(Search);

            iTotalRecords = await qry.CountAsync();

            var ordered = fo(_ordered ? Query as IOrderedQueryable<T> :
                Sort == null ? qry.OrderBy(x => x) :
                Parameters.sSortDir_0 == "asc" ?
                qry.OrderBy(Sort) :
                qry.OrderByDescending(Sort));

            qry = ordered
                        .Skip(Parameters.iDisplayStart)
                        .Take(Parameters.iDisplayLength);

            var query = await qry.ToArrayAsync();

            dataTableResult.aaData = query.AsQueryable().Select(Select).ToArray();
            dataTableResult.iTotalRecords = dataTableResult.aaData.Length;
            dataTableResult.iTotalDisplayRecords = iTotalRecords;
            dataTableResult.sEcho = Parameters.sEcho.ToString();

            return dataTableResult;
        }
    }
}
