using System.Collections.Generic;

#pragma warning disable 1591
namespace Softpark.Infrastructure.Extensions
{
    public static class ISetExtension
    {
        /// <summary>
        /// Extensão para AddRange em lista distinta
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="range"></param>
        public static void AddRange<T>(this ISet<T> collection, IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                collection.Add(item);
            }
        }
    }
}
