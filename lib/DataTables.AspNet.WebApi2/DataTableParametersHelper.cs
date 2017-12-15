using System;
using System.Collections.Generic;

namespace DataTables.AspNet.WebApi2
{
    public class DataTableParametersHelper<T>
    {
        public static List<T> ConvertToList(string parameterPrefix, System.Collections.Specialized.NameValueCollection queryString)
        {
            List<T> list = new List<T>();
            int i = 0;
            T item;
            string parameterName = parameterPrefix + "{0}";

            while (queryString[string.Format(parameterName, i)] != null)
            {
                try
                {
                    item = (T)Convert.ChangeType(queryString[string.Format(parameterName, i)], typeof(T));
                    list.Add(item);
                }
                catch { }
                i++;
            }

            return list;
        }
    }
}
