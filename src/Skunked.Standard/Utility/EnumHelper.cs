using System;
using System.Collections.Generic;
using System.Linq;

namespace Skunked.Utility
{
    public static class EnumHelper
    {
        public static List<T> GetValues<T>() where T : struct
        {
            var type = typeof (T);
            return Enum.GetValues(type).Cast<T>().ToList();
        }
    }
}
