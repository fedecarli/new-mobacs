using System.Collections.Generic;

#pragma warning disable 1591
namespace Softpark.Infrastructure.Extensions
{
    public static class ISetExtension
    {
        public static void AddRange<T>(this ISet<T> collection, IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                collection.Add(item);
            }
        }
    }
}
