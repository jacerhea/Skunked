using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
