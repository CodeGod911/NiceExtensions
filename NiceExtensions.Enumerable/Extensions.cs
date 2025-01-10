namespace NiceExtensions.Enumerable
{
    public static partial class Extensions
    {
        /// <summary>
        /// ForEach on IEnumerable.<br/>
        /// Use ForEachAsync for async expression that should be waited and not void.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
            return items;
        }

        public static async Task ForEachAsync<T>(this IEnumerable<T> values, Func<T, Task> func)
        {
            foreach (var value in values)
            {
                await Task.Run(new Func<Task>(() => func(value)));
            }
        }

        public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> values, Action<T> action)
        {
            await foreach (var value in values)
            {
                action(value);
            }
        }

        public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> values, Func<T, Task> func)
        {
            await foreach (var value in values)
            {
                await Task.Run(new Func<Task>(() => func(value)));
            }
        }

        public static IEnumerable<T> Reverse2<T>(this IEnumerable<T> items)
        {
            var arr = items.ToArray();
            for (var i = arr.Length - 1; i >= 0; i--)
            {
                yield return arr[i];
            }
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs) where TKey : notnull
        {
            return keyValuePairs.ToDictionary(k => k.Key, v => v.Value);
        }

        /// <summary>
        /// Gets the item by the max value of the expression
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="list"></param>
        /// <param name="func">Selector to compare items</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static T1 ByMax<T1, T2>(this IEnumerable<T1> list, Func<T1, T2> func)
            where T1 : notnull
            where T2 : IComparable
        {
            var enumer = list.GetEnumerator();
            if (!enumer.MoveNext())
                throw new ArgumentException("List must not be empty");

            T1 max = enumer.Current;
            while (enumer.MoveNext())
            {
                if (func.Invoke(enumer.Current).CompareTo(func(max)) > 0)
                    max = enumer.Current;
            }

            return max;
        }

        /// <summary>
        /// Gets the item by the min value of the expression
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="list"></param>
        /// <param name="func">Selector to compare items</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static T1 ByMin<T1, T2>(this IEnumerable<T1> list, Func<T1, T2> func)
            where T1 : notnull
            where T2 : IComparable
        {
            var enumer = list.GetEnumerator();
            if (!enumer.MoveNext())
                throw new ArgumentException("List must not be empty");

            T1 min = enumer.Current;
            while (enumer.MoveNext())
            {
                if (func(enumer.Current).CompareTo(func(min)) < 0)
                    min = enumer.Current;
            }

            return min;
        }



    }
}
