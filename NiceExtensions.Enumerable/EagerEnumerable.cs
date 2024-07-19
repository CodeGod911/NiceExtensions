using NiceExtensions.Enumerable.Models;

namespace NiceExtensions.Enumerable
{
	public static partial class Extensions
	{
		/// <summary>
		/// Creates and IEnumerable that caches values while Enumerating and enumerates in the background even if no value is requested at the Time. <br/>
		/// If streaming a value makes sense to start work without the full working set available but should have the next value ready if the work of and item is complete.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <param name="ct">CancellationToken witch cancels the Enumeration in the background (throws)</param>
		/// <returns></returns>
		public static IEnumerable<T> ToCachedEagerLoadingEnumerable<T>(this IEnumerable<T> items, CancellationToken ct = default)
		{
			IEnumerator<T> enumerator = items.GetEnumerator();
			EnumerableCache<T> cache = new(false, new());
			SemaphoreSlim semaphore = new(1, 1);
			var task = Task.Run(() =>
			{
				while (enumerator.MoveNext())
				{
					ct.ThrowIfCancellationRequested();
					cache.Values.Add(enumerator.Current);
				}

				cache.Complete = true;
				enumerator.Dispose();
				enumerator = null!;
			}, ct);
			return ToCachedEagerLoadingEnumerableHelper(cache, semaphore, task, ct);
		}



		private static IEnumerable<T> ToCachedEagerLoadingEnumerableHelper<T>(EnumerableCache<T> cache, SemaphoreSlim semaphore, Task task, CancellationToken ct)
		{
			for (int i = 0; true; i++)
			{
				ct.ThrowIfCancellationRequested();
				if (cache.Values.Count > i)
					yield return cache.Values[i];
				else if (cache.Complete)
					yield break;
				else
				{
					while (cache.Values.Count <= i)
					{
						ct.ThrowIfCancellationRequested();
						if (task.Exception != null)
							throw task.Exception.InnerException ?? task.Exception;
						else Task.Delay(1, ct).Wait(ct);
					}

					yield return cache.Values[i];
				}
			}
		}
	}
}
