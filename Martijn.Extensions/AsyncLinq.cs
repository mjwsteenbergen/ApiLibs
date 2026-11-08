
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martijn.Extensions.AsyncLinq
{
    public static class AsyncLinqExtensions
    {
        public async static Task<IEnumerable<T>> WhenAll<T>(this IEnumerable<Task<T>> enumerable)
        {
            return (await Task.WhenAll(enumerable));
        }

        public static Task WhenAll(this IEnumerable<Task> enumerable)
        {
            return (Task.WhenAll(enumerable));
        }

        public async static IAsyncEnumerable<T> ToIAsyncEnumberable<T>(this IEnumerable<Task<T>> enumerable)
        {
            foreach (var item in enumerable)
            {
                var res = await item;
                yield return res;
            }
        }

        public static async Task Foreach<T>(this IAsyncEnumerable<T> IEnumerable, Action<T> func)
        {
            await foreach (var item in IEnumerable)
            {
                func(item);
            }
        }

        public static async IAsyncEnumerable<V> Select<T,V>(this IAsyncEnumerable<T> enumerable, Func<T,V> func) {
            await foreach (var item in enumerable)
            {
                yield return func(item);
            }
        }

        public static async Task<List<T>> ToList<T>(this IAsyncEnumerable<T> enumerable)
        {
            List<T> res = new List<T>();
            await foreach (var item in enumerable)
            {
                res.Add(item);
            }
            return res;
        }

        public static async Task<T> First<T>(this IAsyncEnumerable<T> enumerable, Func<T, bool> func)
        {
            await foreach (var item in enumerable)
            {
                if (func(item))
                {
                    return item;
                }
            }

            throw new InvalidOperationException("Sequence contains no matching element");
        }

        public static Task<T> First<T>(this IAsyncEnumerable<T> enumerable) => enumerable.First(i => true);

        public static async IAsyncEnumerable<Y> SelectMany<T, Y>(this IAsyncEnumerable<T> enumerable, Func<T, IEnumerable<Y>> func)
        {
            await foreach (var item in enumerable)
            {
                foreach (var actualItem in func(item))
                {
                    yield return actualItem;
                }
            }
        }

        public static async IAsyncEnumerable<T> TakeWhile<T>(this IAsyncEnumerable<T> enumerable, Func<T, bool> func)
        {
            await foreach (var item in enumerable)
            {
                if (!func(item))
                {
                    break;
                }
                yield return item;
            }
        }
    }
}