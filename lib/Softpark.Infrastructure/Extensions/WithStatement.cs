using System;
using System.Linq;

namespace Softpark.Infrastructure.Extensions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class WithStatement
    {
        public static void With<T>(ref T obj, Func<T> fn, string[] without = null) where T : class, new()
        {
            if (obj == null) obj = new T();

            obj.WithMe(fn, without);
        }

        public static void WithMe<T>(this T obj, Func<T> fn, string[] without = null) where T : class, new () =>
            obj.CopyValues(fn(), without);

        public static void CopyValues<T>(this T target, T source, string[] without = null) where T : class, new ()
        {
            Type t = typeof(T);
            var empty = new T();

            without = without ?? new string[0];

            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite && !without.Contains(prop.Name));

            foreach (var prop in properties)
            {
                var value = prop.GetValue(source, null);
                var defaultValue = prop.GetValue(empty, null);
                var originValue = prop.GetValue(target, null);

                if (value != target)
                    prop.SetValue(target, value, null);
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
