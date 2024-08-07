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
            Queue<T> queue = new();
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

            while (queue.Count != 0 || !finished)
            {
                if (queue.Count == 0)
                    Task.Delay(1, ct).Wait(ct);
                else
                    yield return queue.Dequeue();
            }
        }
    }
}
