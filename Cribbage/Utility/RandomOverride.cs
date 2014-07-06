using System;

namespace Skunked.Utility
{
    public class RandomOverride : Random
    {
        public override int Next()
        {
            return base.Next();
        }
    }
}
