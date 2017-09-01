using System;
using System.Linq;

namespace Softpark.Infrastructure.Extensions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class WithStatement
    {
        public static void With<T>(ref T obj, Func<T> fn) where T : class, new()
        {
            if (obj == null) obj = new T();

            obj.WithMe(fn);
        }

        public static void WithMe<T>(this T obj, Func<T> fn) where T : class, new () =>
            obj.CopyValues(fn());

        public static void CopyValues<T>(this T target, T source) where T : class
        {
            Type t = typeof(T);

            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(source, null);

                if (value != null)
                    prop.SetValue(target, value, null);
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
