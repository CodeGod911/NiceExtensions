using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace NiceExtensions.Enumerable
{
    public static partial class Extensions
    {

        /// <summary>
        /// Creates and IEnumerable that enumerates in the background even if no value is requested at the time. <br/>
        /// If streaming a value makes sense to start work without the full working set available but should have the next value ready if the work of and item is complete. <br/>
        /// ExampleUse: IO to IO streaming e.g for parallel reading and writing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="itemsToEnumerateInAdvance">How many items should be enumerated in advance.<br/> Set to -1 for continous loading. Be ware that unconsumed values are cached and use memory :)</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<T> ToEagerLoadingEnumerable<T>(this IEnumerable<T> items, int itemsToEnumerateInAdvance = 1, CancellationToken ct = default)
        {
            if (itemsToEnumerateInAdvance <= 0 && itemsToEnumerateInAdvance != -1) throw new ArgumentException("value was not -1 or > 0", nameof(itemsToEnumerateInAdvance));

            IEnumerator<T> enumerator = items.GetEnumerator();
            ConcurrentQueue<T> queue = new();
            bool finished = false;
            SemaphoreSlim semaphore = new(1, 1);

            var task = Task.Run(async () =>
            {
                while (enumerator.MoveNext() && !ct.IsCancellationRequested)
                {
                    queue.Enqueue(enumerator.Current);
                    while (itemsToEnumerateInAdvance != -1 && queue.Count >= itemsToEnumerateInAdvance && !ct.IsCancellationRequested)
                        await Task.Delay(1);
                }
                finished = true;
            }, ct);

            while (!queue.IsEmpty || !finished)
            {
                if (task.Exception != null)
                    throw task.Exception;

                if (queue.IsEmpty)
                    Task.Delay(1, ct).Wait(ct);
                else
                    if (queue.TryDequeue(out var res))
                    yield return res;
            }
        }


        /// <summary>
        /// Creates and IAsyncEnumerable that enumerates in the background even if no value is requested at the time. <br/>
        /// If streaming a value makes sense to start work without the full working set available but should have the next value ready if the work of and item is complete. <br/>
        /// ExampleUse: IO to IO streaming e.g for parallel reading and writing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="itemsToEnumerateInAdvance">How many items should be enumerated in advance.<br/> Set to -1 for continous loading. Be ware that unconsumed values are cached and use memory :)</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static async IAsyncEnumerable<T> ToEagerLoadingAsyncEnumerable<T>(this IEnumerable<T> items, int itemsToEnumerateInAdvance = 1, [EnumeratorCancellation] CancellationToken ct = default)
        {
            if (itemsToEnumerateInAdvance <= 0 && itemsToEnumerateInAdvance != -1) throw new ArgumentException("value was not -1 or > 0", nameof(itemsToEnumerateInAdvance));

            IEnumerator<T> enumerator = items.GetEnumerator();
            ConcurrentQueue<T> queue = new();
            bool finished = false;

            var task = Task.Run(async () =>
            {
                while (enumerator.MoveNext() && !ct.IsCancellationRequested)
                {
                    queue.Enqueue(enumerator.Current);
                    while (itemsToEnumerateInAdvance != -1 && queue.Count >= itemsToEnumerateInAdvance && !ct.IsCancellationRequested)
                        await Task.Delay(1);
                }
                finished = true;
            }, ct);

            while (!queue.IsEmpty || !finished)
            {
                if (task.Exception != null)
                    throw task.Exception;

                if (queue.IsEmpty)
                    await Task.Delay(1, ct);
                else
                    if (queue.TryDequeue(out var res))
                    yield return res;
            }
        }


        /// <summary>
        /// Creates and IAsyncEnumerable that enumerates in the background even if no value is requested at the time. <br/>
        /// If streaming a value makes sense to start work without the full working set available but should have the next value ready if the work of and item is complete. <br/>
        /// ExampleUse: IO to IO streaming e.g for parallel reading and writing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="itemsToEnumerateInAdvance">How many items should be enumerated in advance.<br/> Set to -1 for continous loading. Be ware that unconsumed values are cached and use memory :)</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static async IAsyncEnumerable<T> ToEagerLoadingAsyncEnumerable<T>(this IAsyncEnumerable<T> items, int itemsToEnumerateInAdvance = 1, [EnumeratorCancellation] CancellationToken ct = default)
        {
            if (itemsToEnumerateInAdvance <= 0 && itemsToEnumerateInAdvance != -1) throw new ArgumentException("value was not -1 or > 0", nameof(itemsToEnumerateInAdvance));

            IAsyncEnumerator<T> enumerator = items.GetAsyncEnumerator(ct);
            ConcurrentQueue<T> queue = new();
            bool finished = false;

            var task = Task.Run(async () =>
            {
                while (await enumerator.MoveNextAsync() && !ct.IsCancellationRequested)
                {
                    queue.Enqueue(enumerator.Current);
                    while (itemsToEnumerateInAdvance != -1 && queue.Count >= itemsToEnumerateInAdvance && !ct.IsCancellationRequested)
                        await Task.Delay(1);
                }
                finished = true;
            }, ct);

            while (!queue.IsEmpty || !finished)
            {
                if (task.Exception != null)
                    throw task.Exception;

                if (queue.IsEmpty)
                    await Task.Delay(1, ct);
                else
                    if (queue.TryDequeue(out var res))
                    yield return res;
            }
        }
    }
}
