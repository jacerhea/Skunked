using System;
using System.Threading;

namespace Skunked.Utility
{
    /// <summary>
    /// Thread Safe Implementation of a Random class provider by Jon Skeet
    /// http://csharpindepth.com/Articles/Chapter12/Random.aspx .
    /// </summary>
    public static class RandomProvider
    {
        private static int _seed = Environment.TickCount;

        private static ThreadLocal<Random> _randomWrapper = null!;

        /// <summary>
        /// Initializes static members of the <see cref="RandomProvider"/> class.
        /// </summary>
        static RandomProvider()
        {
            ResetInstance();
        }

        /// <summary>
        /// Sets for Testing purposes only.
        /// </summary>
        public static ThreadLocal<Random> RandomInstance
        {
            set => _randomWrapper = value;
        }

        /// <summary>
        /// Restart instance of random generator.
        /// </summary>
        public static void ResetInstance()
        {
            _randomWrapper = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _seed)));
        }

        public static Random GetThreadRandom()
        {
            return _randomWrapper.Value;
        }
    }
}