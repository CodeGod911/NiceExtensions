using NiceExtensions.Enumerable.Models;

namespace NiceExtensions.Enumerable
{
    public static partial class Extensions
    {
        /// <summary>
        /// Creates and IEnumerable that caches values while Enumerating.<br/>
        /// If an IEnumerable is used twice or more and a ToList would be a disadvantage and multiple enumeration is counterproductive.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToCachedEnumerable<T>(this IEnumerable<T> items)
        {
            IEnumerator<T> enumerator = items.GetEnumerator();
            EnumerableCache<T> cache = new(false, new());
            SemaphoreSlim semaphore = new(1, 1);
            return ToCachedEnumerableHelper(enumerator, cache, semaphore);
        }

        private static IEnumerable<T> ToCachedEnumerableHelper<T>(IEnumerator<T> enumerator, EnumerableCache<T> cache, SemaphoreSlim semaphore)
        {
            for (int i = 0; true; i++)
            {
                if (i >= cache.Values.Count)
                    semaphore.Wait();

                if (i < cache.Values.Count)
                {
                    yield return cache.Values[i];
                    continue;
                }

                T t = default!;
                bool set = false;
                try
                {
                    if (!cache.Complete && enumerator.MoveNext())
                    {
                        t = enumerator.Current;
                        cache.Values.Add(t);
                        set = true;
                    }
                    else
                    {
                        cache.Complete = true;
                        enumerator.Dispose();
                        enumerator = null!;
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
        /// Creates and IEnumerable that caches values while Enumerating.<br/>
        /// If an IEnumerable is used twice or more and a ToList would be a disadvantage and multiple enumeration is counterproductive.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IAsyncEnumerable<T> ToCachedAsyncEnumerable<T>(this IEnumerable<T> items)
        {
            IEnumerator<T> enumerator = items.GetEnumerator();
            EnumerableCache<T> cache = new(false, new());
            SemaphoreSlim semaphore = new(1, 1);
            return ToCachedAsyncEnumerableHelper(enumerator, cache, semaphore);
        }

        private static async IAsyncEnumerable<T> ToCachedAsyncEnumerableHelper<T>(IEnumerator<T> enumerator, EnumerableCache<T> cache, SemaphoreSlim semaphore)
        {
            for (int i = 0; true; i++)
            {
                if (i >= cache.Values.Count)
                    await semaphore.WaitAsync();

                if (i < cache.Values.Count)
                {
                    yield return cache.Values[i];
                    continue;
                }

                T t = default!;
                bool set = false;
                try
                {
                    if (!cache.Complete && enumerator.MoveNext())
                    {
                        t = enumerator.Current;
                        cache.Values.Add(t);
                        set = true;
                    }
                    else
                    {
                        cache.Complete = true;
                        enumerator.Dispose();
                        enumerator = null!;
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
        /// Creates and IAsyncEnumerable that caches values while Enumerating.<br/>
        /// If an IAsyncEnumerable is used twice or more and a ToList would be a disadvantage and multiple enumeration is counterproductive.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IAsyncEnumerable<T> ToCachedAsyncEnumerable<T>(this IAsyncEnumerable<T> items)
        {
            IAsyncEnumerator<T> enumerator = items.GetAsyncEnumerator();
            EnumerableCache<T> cache = new(false, new());
            SemaphoreSlim semaphore = new(1, 1);
            return ToCachedAsyncEnumerableHelper(enumerator, cache, semaphore);
        }

        private static async IAsyncEnumerable<T> ToCachedAsyncEnumerableHelper<T>(IAsyncEnumerator<T> enumerator, EnumerableCache<T> cache, SemaphoreSlim semaphore)
        {
            for (int i = 0; true; i++)
            {
                if (i >= cache.Values.Count)
                    await semaphore.WaitAsync();

                if (i < cache.Values.Count)
                {
                    yield return cache.Values[i];
                    continue;
                }


                T t = default!;
                bool set = false;
                try
                {
                    if (await enumerator.MoveNextAsync())
                    {
                        t = enumerator.Current;
                        cache.Values.Add(t);
                        set = true;
                    }
                    else
                    {
                        cache.Complete = true;
                        await enumerator.DisposeAsync();
                        enumerator = null!;
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
}
