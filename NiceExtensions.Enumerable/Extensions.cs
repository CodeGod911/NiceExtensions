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

        public static IEnumerable<T> Reverse<T>(this IEnumerable<T> items)
        {
            var arr = items.ToArray();
            for (var i = arr.Length-1; i >= 0; i--)
            {
                yield return arr[i];
            }
        }

        public static IEnumerable<int> Sequence(int start,int end,int step = 1)
        {
            for (var i = start; i <= end; i+)
        }
    }
}