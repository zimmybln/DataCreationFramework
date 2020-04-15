using System;
using System.Collections.Generic;

namespace DataCreationFramework.Extensions
{
    public static class GenericExtensions
    {
        /// <summary>
        /// Checks if the value equals one of the values in items.
        /// </summary>
        public static bool IsOneOfTheseValues<T>(this T value, IEnumerable<T> items)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            foreach (T item in items)
            {
                if (value.Equals(item))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
