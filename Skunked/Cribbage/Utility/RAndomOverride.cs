using System;

namespace Skunked.Utility
{
    public class RAndomOverride : Random
    {
        public override int Next()
        {
            return base.Next();
        }
    }
}
