using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable 1591
namespace Softpark.Infrastructure.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<object> Cast(this IEnumerable source, Type type)
        {
            return source.Cast<object>().Select(x => Convert.ChangeType(x, type)).ToArray<object>();
        }
    }
}
