using System.Runtime.CompilerServices;

namespace NiceExtensions.Enumerable
{
    public static class Extensions
    {

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
            return items;
        }
        public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> values, Action<T> action)
        {
            await foreach (var value in values)
            {
                action(value);
            }
        }

        public static IEnumerable<T> Reverse<T>(this IEnumerable<T> items)
        {
            var arr = items.ToArray();
            for (var i = arr.Length-1; i >= 0; i--)
            {
                yield return arr[i];
            }
        }

        /// <summary>
        /// Creates an IEnumerable that caches already enumerated values and preserves the lazy load generator advantages lost with a .ToList() 
        /// </summary>
        public static IEnumerable<T> ToCachedEnumerable<T>(this IEnumerable<T> items)
        {
            IEnumerator<T> enumerator = items.GetEnumerator();
            List<T> cache = new List<T>();
            SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
            return ToCachedEnumerableHelper(enumerator, cache, semaphore);
        }

        private static IEnumerable<T> ToCachedEnumerableHelper<T>(IEnumerator<T> enumerator, List<T> cache, SemaphoreSlim semaphore)
        {
            for (int i = 0; true; i++)
            {
                semaphore.Wait();
                if (i < cache.Count)
                {
                    semaphore.Release();
                    yield return cache[i];
                    continue;
                }
                T t = default!;
                bool set = false;
                try
                {
                    if (enumerator.MoveNext())
                    {
                        t = enumerator.Current;
                        cache.Add(t);
                        set = true;
                    }
                }
                catch (Exception)
                {
                    break;
                    throw;
                }
                finally
                {
                    semaphore.Release();
                }
                if (set)
                    yield return t;
                else
                    yield break;
            }
        }

        /// <summary>
        /// Creates an IAsyncEnumerable that caches already enumerated values and preserves the lazy load generator advantages lost with a .ToList() 
        /// </summary>
        public static IAsyncEnumerable<T> ToCachedAsyncEnumerable<T>(this IAsyncEnumerable<T> items)
        {
            IAsyncEnumerator<T> enumerator = items.GetAsyncEnumerator();
            List<T> cache = new List<T>();
            SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
            return ToCachedAsyncEnumerableHelper(enumerator, cache, semaphore);
        }


        private static async IAsyncEnumerable<T> ToCachedAsyncEnumerableHelper<T>(IAsyncEnumerator<T> enumerator, List<T> cache, SemaphoreSlim semaphore)
        {
            for (int i = 0; true; i++)
            {
                await semaphore.WaitAsync();
                if (i < cache.Count)
                {
                    semaphore.Release();
                    yield return cache[i];
                    continue;
                }
                T t = default!;
                bool set = false;
                try
                {
                    if (await enumerator.MoveNextAsync())
                    {
                        t = enumerator.Current;
                        cache.Add(t);
                        set = true;
                    }
                }
                catch (Exception)
                {
                    break;
                    throw;
                }
                finally
                {
                    semaphore.Release();
                }
                if (set)
                    yield return t;
                else
                    yield break;
            }
        }
    }

    //public static IEnumerable<int> Sequence(int start,int end,int step = 1)
    //{
    //    for (var i = start; i <= end; i+)
    //}
}
