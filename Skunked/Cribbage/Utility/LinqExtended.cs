using System;
using System.Collections.Generic;
using System.Linq;

namespace Cribbage.Utility
{
    public static partial class LinqExtended
    {
        public static IEnumerable<TResult> Cartesian<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");

            var bufferedSecond = second.ToList();

            return first.SelectMany(item1 => bufferedSecond.Select(item2 => resultSelector(item1, item2)));
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var random = new Random();
            list.Shuffle(random);
        }

        private static void Shuffle<T>(this IList<T> list, Random random)
        {
            for (int index = list.Count - 1; index > 0; index--)
            {
                int position = random.Next(index + 1);
                T temp = list[index];
                list[index] = list[position];
                list[position] = temp;
            }
        }
    }
}
