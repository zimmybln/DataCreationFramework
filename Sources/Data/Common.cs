using System;
using System.Collections.Generic;

namespace DataCreationFramework.Data
{
    [Flags]
    public enum CreateTextOptions
    {
        None = 0x0,

        WithoutSpaces = 0x1,

        Randomized = 0x2
    }

    /// <summary>
    /// Various methods for create kinds of content.
    /// </summary>
    public static class Common
    {
        private static Random _random = new Random();

        /// <summary>
        /// Creates a unique text with the specific size.
        /// </summary>
        public static string CreateUniqueText(int size = 30)
        {
            return CreateUniqueText(string.Empty, size, CreateTextOptions.Randomized);
        }

        public static string CreateUniqueText(string prefix, int size = 0, CreateTextOptions options = CreateTextOptions.None)
        {
            string value = String.Empty;
            int lowerIndex = (int)'A';
            int higherIndex = (int)'Z';


            if ((options & CreateTextOptions.Randomized) == CreateTextOptions.Randomized)
            {
                value = prefix ?? String.Empty;

                for (int idx = 0; idx < size - value.Length; idx++)
                {
                    value += (Char)_random.Next(lowerIndex, higherIndex);
                }
            }
            else
            {
                value = $"{prefix} ({DateTime.Now}) {DateTime.Now.Ticks}";
            }

            if ((options & CreateTextOptions.WithoutSpaces) == CreateTextOptions.WithoutSpaces)
            {
                value = value.Replace(" ", string.Empty);
            }

            if (size > 0 && value.Length > size)
            {
                value = value.Substring(0, size);
            }
            else if (size > 0 && value.Length < size)
            {
                while (value.Length < size)
                {
                    value += (Char)_random.Next(lowerIndex, higherIndex);
                }
            }

            return value;
        }

        public static IEnumerable<T> CreateItems<T>(int amountOfItems, DataCreationStrategy<T> strategy = null) where T : new()
        {
            for (int i = 0; i < amountOfItems; i++)
            {
                yield return strategy != null ? strategy.GetValue() : new T();
            }
        }

        public static T CreateItem<T>(DataCreationStrategy<T> strategy)
        {
            return strategy.GetValue();
        }
    }
}
