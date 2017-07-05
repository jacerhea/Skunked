using System;
using System.Threading;
using Skunked.Utility;

namespace Skunked.Test.System
{
    public class IncrementalRandom : Random
    {
        private static int _seed = 0;

        public override int Next(int minValue, int maxValue)
        {
            var next = minValue + _seed;
            while (next > maxValue)
            {
                next = next % (maxValue - minValue);
            }

            Interlocked.Increment(ref _seed);
            if (!next.IsBetween(minValue, maxValue))
            {
                throw new Exception("Something went terribly wrong.");
            }
            return next;
        }

        public override int Next(int maxValue)
        {
            var next = _seed;
            while (next > maxValue)
            {
                next = next % (maxValue - 0);
            }
            Interlocked.Increment(ref _seed);
            if (!next.IsBetween(0, maxValue))
            {
                throw new Exception("Something went terribly wrong.");
            }
            return next;
        }
    }
}
