using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable 1591
namespace Softpark.Infrastructure.Extensions
{
    public static class IEnumerableExtension
    {
        /// <summary>
        /// Extensão para conversão de tipo de enumerador
        /// </summary>
        /// <param name="source"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<object> Cast(this IEnumerable source, Type type)
        {
            return source.Cast<object>().Select(x => Convert.ChangeType(x, type)).ToArray<object>();
        }

        public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> where)
        {
            int index = -1;
            int pos = -1;

            source.ToList().ForEach(x =>
            {
                index++;
                if (where(x) && pos < 0)
                    pos = index;
            });

            return pos;
        }

        public static int LastIndexOf<T>(this IEnumerable<T> source, Func<T, bool> where)
        {
            int index = -1;
            int pos = -1;

            source.ToList().ForEach(x =>
            {
                index++;
                if (where(x))
                    pos = index;
            });

            return pos;
        }
    }
}
