using System;
using System.Collections.Generic;
using System.Linq;

namespace Skunked.Utility
{
    /// <summary>
    /// Set of enum helper functions.
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Returns all values of an enumeration.
        /// </summary>
        /// <typeparam name="T">The enum type to enumerate.</typeparam>
        /// <returns>The values of an enumeration.</returns>
        public static List<T> GetValues<T>() where T : struct
        {
            var type = typeof(T);
            return Enum.GetValues(type).Cast<T>().ToList();
        }
    }
}
