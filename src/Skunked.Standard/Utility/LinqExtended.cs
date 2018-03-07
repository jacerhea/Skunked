using System;
using System.Collections.Generic;
using System.Linq;

namespace Skunked.Utility
{
    public static class LinqExtended
    {
        public static IEnumerable<T> Append<T>(this IEnumerable<T> list, T item)
        {
            foreach (var selectListItem in list)
            {
                yield return selectListItem;
            }
            yield return item;
        }

        public static IEnumerable<TResult> Cartesian<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var bufferedSecond = second.ToList();

            return first.SelectMany(item1 => bufferedSecond.Select(item2 => resultSelector(item1, item2)));
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            list.Shuffle(RandomProvider.GetThreadRandom());
        }

        public static void Shuffle<T>(this IList<T> list, Random random)
        {
            for (int index = list.Count - 1; index > 0; index--)
            {
                int position = random.Next(index + 1);
                T temp = list[index];
                list[index] = list[position];
                list[position] = temp;
            }
        }

        public static T NextOf<T>(this IList<T> list, T item)
        {
            if (list.Count == 0) throw new ArgumentOutOfRangeException(nameof(list));
            return list[(list.IndexOf(item) + 1) % list.Count];
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, Comparer<TKey>.Default);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var min = sourceIterator.Current;
                var minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, Comparer<TKey>.Default);
        }


        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var max = sourceIterator.Current;
                var maxKey = selector(max);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, maxKey) > 0)
                    {
                        max = candidate;
                        maxKey = candidateProjected;
                    }
                }
                return max;
            }
        }

        public static IEnumerable<T> TakeEvery<T>(this IEnumerable<T> source, int nStep)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.Where((x, i) => i % nStep == 0);

        }


        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> enumerable, int count)
        {
            return enumerable.Reverse().Take(count);
        }

        public static IEnumerable<TSource> Infinite<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            while (true)
            {
                foreach (var item in source)
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.DistinctBy(keySelector, null);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            return DistinctByImpl(source, keySelector, comparer);
        }

        private static IEnumerable<TSource> DistinctByImpl<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            var knownKeys = new HashSet<TKey>(comparer);
            return source.Where(element => knownKeys.Add(keySelector(element)));
        }
    }
}
