using System;
using System.Collections.Generic;
using System.Linq;

namespace Skunked.Utility
{
    /// <summary>
    /// Functionally similar projections as LINQ library.
    /// </summary>
    public static class LinqExtended
    {
        /// <summary>
        /// Returns the Cartesian product of two sequences by enumerating all
        /// possible combinations of one item from each sequence, and applying
        /// a user-defined projection to the items in a given combination.
        /// </summary>
        /// <typeparam name="T1">
        /// The type of the elements of <paramref name="first"/>.</typeparam>
        /// <typeparam name="T2">
        /// The type of the elements of <paramref name="second"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the elements of the result sequence.</typeparam>
        /// <param name="first">The first sequence of elements.</param>
        /// <param name="second">The second sequence of elements.</param>
        /// <param name="resultSelector">A projection function that combines
        /// elements from all of the sequences.</param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// <para>
        /// The method returns items in the same order as a nested foreach
        /// loop, but all sequences except for <paramref name="first"/> are
        /// cached when iterated over. The cache is then re-used for any
        /// subsequent iterations.</para>
        /// <para>
        /// This method uses deferred execution and stream its results.</para>
        /// </remarks>
        public static IEnumerable<TResult> Cartesian<T1, T2, TResult>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            Func<T1, T2, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var bufferedSecond = second.ToList();

            return first.SelectMany(item1 => bufferedSecond.Select(item2 => resultSelector(item1, item2)));
        }

        /// <summary>
        /// Randomly rearrange the set of items.
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T">The type of source sequence elements.</typeparam>
        public static void Shuffle<T>(this IList<T> source)
        {
            source.Shuffle(RandomProvider.GetThreadRandom());
        }

        /// <summary>
        /// Randomly rearrange the set of items using the given random generator.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="random">A random generator used as part of the selection algorithm.</param>
        /// <typeparam name="T">The type of source sequence elements.</typeparam>
        public static void Shuffle<T>(this IList<T> source, Random random)
        {
            for (int index = source.Count - 1; index > 0; index--)
            {
                int position = random.Next(index + 1);
                T temp = source[index];
                source[index] = source[position];
                source[position] = temp;
            }
        }

        public static T NextOf<T>(this IList<T> list, T item)
        {
            if (list.Count == 0) throw new ArgumentOutOfRangeException(nameof(list));
            return list[(list.IndexOf(item) + 1) % list.Count];
        }

        /// <summary>
        /// Returns the minimal elements of the given sequence, based on
        /// the given projection.
        /// </summary>
        /// <remarks>
        /// This overload uses the default comparer for the projected type.
        /// This operator uses deferred execution. The results are evaluated
        /// and cached on first use to returned sequence.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the projected element.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector to use to pick the results to compare.</param>
        /// <returns>The sequence of minimal elements, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static TSource MinBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Returns the minimal elements of the given sequence, based on
        /// the given projection and the specified comparer for projected values.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution. The results are evaluated
        /// and cached on first use to returned sequence.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the projected element.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector to use to pick the results to compare.</param>
        /// <param name="comparer">Comparer to use to compare projected values.</param>
        /// <returns>The sequence of minimal elements, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/>
        /// or <paramref name="comparer"/> is null.</exception>
        public static TSource MinBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> selector,
            IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            using var sourceIterator = source.GetEnumerator();
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

        /// <summary>
        /// Returns the maximal elements of the given sequence, based on
        /// the given projection.
        /// </summary>
        /// <remarks>
        /// This overload uses the default comparer  for the projected type.
        /// This operator uses deferred execution. The results are evaluated
        /// and cached on first use to returned sequence.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the projected element.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector to use to pick the results to compare.</param>
        /// <returns>The sequence of maximal elements, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static TSource MaxBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Returns the maximal elements of the given sequence, based on
        /// the given projection and the specified comparer for projected values.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution. The results are evaluated
        /// and cached on first use to returned sequence.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the projected element.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector to use to pick the results to compare.</param>
        /// <param name="comparer">Comparer to use to compare projected values.</param>
        /// <returns>The sequence of maximal elements, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/>
        /// or <paramref name="comparer"/> is null.</exception>
        public static TSource MaxBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> selector,
            IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            using var sourceIterator = source.GetEnumerator();
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

        /// <summary>
        /// Returns every N-th element of a sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="step">Number of elements to bypass before returning the next element.</param>
        /// <returns>
        /// A sequence with every N-th element of the input sequence.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// int[] numbers = { 1, 2, 3, 4, 5 };
        /// var result = numbers.TakeEvery(2);
        /// ]]></code>
        /// The <c>result</c> variable, when iterated over, will yield 1, 3 and 5, in turn.
        /// </example>

        public static IEnumerable<TSource> TakeEvery<TSource>(this IEnumerable<TSource> source, int step)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (step <= 0) throw new ArgumentOutOfRangeException(nameof(step));
            return source.Where((e, i) => i % step == 0);
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

        /// <summary>
        /// Returns all distinct elements of the given source, where "distinctness"
        /// is determined via a projection and the default equality comparer for the projected type.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results, although
        /// a set of already-seen keys is retained. If a key is seen multiple times,
        /// only the first element with that key is returned.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the projected element.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Projection for determining "distinctness".</param>
        /// <returns>A sequence consisting of distinct elements from the source sequence,
        /// comparing them by the specified key projection.</returns>

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.DistinctBy(keySelector, null);
        }

        /// <summary>
        /// Returns all distinct elements of the given source, where "distinctness"
        /// is determined via a projection and the specified comparer for the projected type.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results, although
        /// a set of already-seen keys is retained. If a key is seen multiple times,
        /// only the first element with that key is returned.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the projected element.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Projection for determining "distinctness".</param>
        /// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
        /// If null, the default equality comparer for <c>TSource</c> is used.</param>
        /// <returns>A sequence consisting of distinct elements from the source sequence,
        /// comparing them by the specified key projection.</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            return _(); IEnumerable<TSource> _()
            {
                var knownKeys = new HashSet<TKey>(comparer);
                foreach (var element in source)
                {
                    if (knownKeys.Add(keySelector(element)))
                        yield return element;
                }
            }
        }
    }
}
