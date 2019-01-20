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
        private static int _seed = Environment.TickCount;

        private static ThreadLocal<Random> _randomWrapper;

        static RandomProvider()
        {
            ResetInstance();
        }

        public static void ResetInstance()
        {
            _randomWrapper = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _seed)));
        }

        /// <summary>
        /// For Testing purposes only
        /// </summary>
        public static ThreadLocal<Random> RandomInstance 
        {
            set => _randomWrapper = value;
        }

        public static Random GetThreadRandom()
        {
            return _randomWrapper.Value;
        }
    }
}