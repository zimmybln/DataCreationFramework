using System.Collections.Generic;

namespace DataCreationFramework.Extensions
{
    public static class IntegerExtensions
    {
        /// <summary>
        /// Checks whether an array of integers is descending.
        /// </summary>
        public static bool IsDescending(this IEnumerable<int> items)
        {
            int? formervalue = null;

            foreach (int i in items)
            {
                if (!formervalue.HasValue)
                {
                    formervalue = i;
                }
                else
                {
                    if (i >= formervalue)
                    {
                        return false;
                    }
                    formervalue = i;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks whether an array of integers is ascending.
        /// </summary>
        public static bool IsAscending(this IEnumerable<int> items)
        {
            int? formervalue = null;

            foreach (int i in items)
            {
                if (!formervalue.HasValue)
                {
                    formervalue = i;
                }
                else
                {
                    if (i <= formervalue)
                    {
                        return false;
                    }
                    formervalue = i;
                }
            }

            return true;
        }
    }
}
