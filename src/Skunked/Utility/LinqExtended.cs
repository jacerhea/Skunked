namespace Skunked.Utility;

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
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <returns>A sequence of elements.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    public static IEnumerable<(T1 Item1, T2 Item2)> Cartesian<T1, T2>(
        this IEnumerable<T1> first,
        IEnumerable<T2> second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);

        var bufferedSecond = second.ToList();

        return first.SelectMany(item1 => bufferedSecond.Select(item2 => (item1, item2)));
    }

    /// <summary>
    /// Randomly rearrange the set of items.
    /// </summary>
    /// <param name="source">IList to shuffle.</param>
    /// <typeparam name="T">The type of source sequence elements.</typeparam>
    public static void Shuffle<T>(this IList<T> source)
    {
        source.Shuffle(RandomProvider.GetThreadRandom());
    }

    /// <summary>
    /// Randomly rearrange the set of items using the given random generator.
    /// </summary>
    /// <param name="source">IList to shuffle.</param>
    /// <param name="random">A random generator used as part of the selection algorithm.</param>
    /// <typeparam name="T">The type of source sequence elements.</typeparam>
    public static void Shuffle<T>(this IList<T> source, Random random)
    {
        for (int index = source.Count - 1; index > 0; index--)
        {
            int position = random.Next(index + 1);
            (source[index], source[position]) = (source[position], source[index]);
        }
    }

    /// <summary>
    /// The list goes round and round and this returns the next item after the given item.
    /// </summary>
    /// <typeparam name="T">Type of item in the list.</typeparam>
    /// <param name="list">The source list.</param>
    /// <param name="item">The item to find next from.</param>
    /// <returns>The found item.</returns>
    public static T NextOf<T>(this IList<T> list, T item)
    {
        if (list.Count == 0) throw new ArgumentOutOfRangeException(nameof(list));
        return list[(list.IndexOf(item) + 1) % list.Count];
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
        ArgumentNullException.ThrowIfNull(source);
        if (step <= 0) throw new ArgumentOutOfRangeException(nameof(step));
        return source.Where((_, i) => i % step == 0);
    }


    /// <summary>
    /// Endless iterates source.  l
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns>Iterated Item</returns>
    /// <exception cref="ArgumentNullException">source must not be null.</exception>
    public static IEnumerable<TSource> Infinite<TSource>(this IEnumerable<TSource> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        while (true)
        {
            foreach (var item in source)
            {
                yield return item;
            }
        }
    }
}