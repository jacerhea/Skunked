using System;
using System.Collections.Generic;
using System.Linq;

namespace Cribbage.Utility
{
    public class EnumHelper
    {
        public static List<T> GetValues<T>()
        {
            var type = typeof (T);
            if (!type.IsEnum)
            {
                throw new ArgumentException("Type '" + type.Name + "' is not an enum");
            }
            return Enum.GetValues(type).Cast<T>().ToList();
        }
    }
}
