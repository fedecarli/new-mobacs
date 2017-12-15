using System;

namespace DataTables.AspNet.WebApi2
{
    public class DataTableResult
    {
        /*private string _sEcho;
        public string sEcho
        {
            get
            {
                return _sEcho;
            }
            set
            {
                int result;
                if (int.TryParse(value, out result))
                    _sEcho = value;
                else
                    throw new Exception(string.Format("sEcho is expected to be a number, but it's: \"{0}\".", value));
            }
        }*/

        //An unaltered copy of sEcho sent from the client side. This parameter will change with each draw (it is basically a draw count)
        // - so it is important that this is implemented. 
        // Note that it strongly recommended for security reasons that you 'cast' this parameter to
        // an integer in order to prevent Cross Site Scripting (XSS) attacks.
        // TODO: ^
        public string sEcho { get; set; }
        //Total records, before filtering (i.e. the total number of records in the database)
        public int iTotalRecords { get; set; }
        //Total records, after filtering 
        // (i.e. the total number of records after filtering has been applied 
        // - not just the number of records being returned in this result set)
        public int iTotalDisplayRecords { get; set; }

        //public string sColumns { get; set; }
        //public IEnumerable<object> aaData { get; set; }
        public Array aaData { get; set; }

        //(int totalRecords,
        //                                           int totalDisplayRecords,
        //                                           string echo,
        //        //string columns,
        //                                           IEnumerable<T> data,

        //        string contentType,
        //System.Text.Encoding contentEncoding,
        //JsonRequestBehavior behavior

        //        )
    }
}
