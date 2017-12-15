using System;
using System.Collections.Generic;

namespace DataTables.AspNet.WebApi2
{
    // This class might be temporary, I'm looking for a better solution 
    //     to get the parameters sent to the server by dataTables.
    public class DataTableParameters
    {
        readonly int iDisplayStart;
        readonly int iDisplayLength;
        readonly int iColumns;
        readonly bool bRegex;
        readonly int iSortingCols;
        readonly int sEcho;

        List<bool> bSearchable;
        // Private Constructor with no parameter to force developer to use
        //   this constructor instead: DataTableParameters(NameValueCollection nameValueCollection)
        private DataTableParameters() { }
        
        public DataTableParameters(System.Collections.Specialized.NameValueCollection queryString)
        {
            if (queryString["iDisplayStart"] == null)
                int.TryParse(queryString["iDisplayStart"], out iDisplayStart);
            if (queryString["iDisplayLength"] == null)
                int.TryParse(queryString["iDisplayLength"], out iDisplayLength);
            if (queryString["iColumns"] == null)
                int.TryParse(queryString["iColumns"], out iDisplayLength);
            if (queryString["sSearch"] == null)
                Search = queryString["sSearch"];
            if (queryString["bRegex"] == null)
                bool.TryParse(queryString["bRegex"], out bRegex);
            if (queryString["iSortingCols"] == null)
                int.TryParse(queryString["iSortingCols"], out iSortingCols);
            if ((queryString["sEcho"] == null)
                || !int.TryParse(queryString["sEcho"], out sEcho))
                throw new Exception("sEcho parameter is not defined well.");
            bSearchable = DataTableParametersHelper<bool>.ConvertToList("bSearchable_", queryString);

        }
        
        public int DisplayStart { get { return iDisplayLength; } }
        public int DisplayLength { get { return iDisplayLength; } }
        public int Columns { get { return iColumns; } }
        public string Search { get; private set; }
        //True if the global filter should be treated as a regular expression for advanced filtering, false if not.
        public bool Regex { get { return bRegex; } }
        //Number of columns to sort on
        public int SortingCols { get { return iSortingCols; } }
        public string Echo { get { return sEcho.ToString(); } }
    }
}
