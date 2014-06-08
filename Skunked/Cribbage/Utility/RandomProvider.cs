using System;
using System.Threading;

namespace Skunked.Utility
{
    /// <summary>
    /// Thread Safe Implementation of a Random class provider by Jon Skeet
    /// http://csharpindepth.com/Articles/Chapter12/Random.aspx
    /// </summary>
    public static class RandomProvider
    {
        private static int seed = Environment.TickCount;

        private static readonly ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        public static Random GetThreadRandom()
        {
            return randomWrapper.Value;
        }
    }
}