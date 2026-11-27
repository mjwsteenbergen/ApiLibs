using System;
using System.Collections.Generic;
using System.Linq;

namespace Martijn.Extensions.Linq
{
    public static class LinqExtensions
    {
        public static string Combine(this IEnumerable<string> IEnumerable, string between)
        {
            return IEnumerable.Combine((i, j) => i + between + j);
        }

        public static T Combine<T>(this IEnumerable<T> IEnumerable, Func<T, T, T> func)
        {
            var count = 0;
            foreach (var item in IEnumerable)
            {
                count++;

                if (count > 2)
                {
                    break;
                }
            }

            if (count >= 2)
            {
                return IEnumerable.Aggregate(func);
            }
            else if (count == 1)
            {
                return IEnumerable.First();
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>Executes <c>func</c> for each item in Enumerable</summary>
        public static void Foreach<T>(this IEnumerable<T> IEnumerable, Action<T> func)
        {
            foreach (var item in IEnumerable)
            {
                func(item);
            }
        }

        public static IEnumerable<T> Repeat<T>(this T item, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                yield return item;
            }
        }

        /// <summary>Removes all items in Enumerable not found in other</summary>
        public static IEnumerable<T> Complement<T>(this IEnumerable<T> me, IEnumerable<T> other)
        {
            return me.Where(i => !other.Contains(i));
        }

        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Where(i => i != null);
        }

        public static IEnumerable<List<T>> Subset<T>(this IEnumerable<T> list, int amountOfItems)
        {
            List<List<T>> cache = new List<List<T>>();

            foreach (var item in list)
            {
                cache = cache.Concat(cache.Select(i => i.Concat(new List<T> { item }).ToList())).Append(new List<T> { item }).ToList();
                foreach (var res in cache.Where(i => i.Count == amountOfItems))
                {
                    yield return res;
                }
                cache = cache.Where(i => i.Count < amountOfItems).ToList();
            }
        }

        public static IEnumerable<List<T>> Split<T>(this IEnumerable<T> list, int size)
        {
            if (size < 0) throw new ArgumentException(nameof(size) + " should be bigger than zero");
            List<T> subset = new List<T>();
            foreach (var item in list)
            {
                subset.Add(item);
                if (subset.Count >= size)
                {
                    yield return subset;
                    subset = new List<T>();
                }
            }

            if (subset.Count > 0)
            {
                yield return subset;
            }
        }

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> IEnumerable)
        {
            return IEnumerable.Distinct((i,j) => i?.Equals(j) ?? j?.Equals(i) ?? true, (i) => i?.GetHashCode() ?? 0);
        }

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> IEnumerable, Func<T, T, bool> equals, Func<T, int> hash)
        {
            return IEnumerable.Distinct(new Comparerr<T>(equals, hash));
        }

        public class Comparerr<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> equals;
            private readonly Func<T, int> hash;

            public Comparerr(Func<T, T, bool> equals, Func<T, int> hash)
            {
                this.equals = equals;
                this.hash = hash;
            }

            public bool Equals(T x, T y)
            {
                return equals(x, y);
            }

            public int GetHashCode(T obj)
            {
                return hash(obj);
            }
        }
    }
}