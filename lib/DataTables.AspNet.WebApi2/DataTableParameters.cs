using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataTables.AspNet.WebApi2
{
    // This class might be temporary, I'm looking for a better solution 
    //     to get the parameters sent to the server by dataTables.
    public class DataTableParameters
    {
        public int iDisplayStart { get; set; }
        public int iDisplayLength { get; set; }
        public int iSortingCols { get; set; }
        public int iSortCol_0 { get; set; }
        public string sSortDir_0 { get; set; }
        public string sSearch { get; set; }
        public int sEcho { get; set; }
        public int Total { get; set; }

        List<bool> bSearchable;
        // Private Constructor with no parameter to force developer to use
        //   this constructor instead: DataTableParameters(NameValueCollection nameValueCollection)
        public DataTableParameters() { }

        public DataTableParameters(System.Collections.Specialized.NameValueCollection queryString)
        {
            if (queryString["iDisplayStart"] == null)
                if (int.TryParse(queryString["iDisplayStart"], out int out1))
                    iDisplayStart = out1;
            if (queryString["iDisplayLength"] == null)
                if (int.TryParse(queryString["iDisplayLength"], out int out2))
                    iDisplayLength = out2;
            if (queryString["iColumns"] == null)
                if (int.TryParse(queryString["iColumns"], out int out3))
                    iDisplayLength = out3;
            if (queryString["sSearch"] == null)
                sSearch = queryString["sSearch"];
            if (queryString["iSortingCols"] == null)
                if (int.TryParse(queryString["iSortingCols"], out int out4))
                    iSortingCols = out4;
            if ((queryString["sEcho"] == null)
                || !int.TryParse(queryString["sEcho"], out int out5))
                throw new Exception("sEcho parameter is not defined well.");
            else sEcho = out5;
            bSearchable = DataTableParametersHelper<bool>.ConvertToList("bSearchable_", queryString);
        }

        public DataTableComposer<T> Compose<T>(IQueryable<T> queryable,
            Expression<Func<T, object>> sort,
            Expression<Func<T, bool>> search,
            Expression<Func<T, object>> select) =>
            new DataTableComposer<T>(this, queryable, sort, search, select);
    }
}
