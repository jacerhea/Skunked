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

        private static ThreadLocal<Random> randomWrapper;

        static RandomProvider()
        {
             randomWrapper = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));
        }

        /// <summary>
        /// For Testing purposes only
        /// </summary>
        public static ThreadLocal<Random> RandomInstance 
        {
            set { randomWrapper = value; }
        }

        public static Random GetThreadRandom()
        {
            return randomWrapper.Value;
        }
    }
}