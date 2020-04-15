using System;

namespace DataCreationFramework.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsSameDay(this DateTime source, DateTime? target)
        {
            if (!target.HasValue)
                return false;

            return (source.Year == target.Value.Year &&
                    source.Month == target.Value.Month &&
                    source.Day == target.Value.Day);
        }

        public static bool IsSameDay(this DateTime? source, DateTime? target)
        {
            if (!source.HasValue || !target.HasValue)
                return false;

            return (source.Value.Year == target.Value.Year &&
                    source.Value.Month == target.Value.Month &&
                    source.Value.Day == target.Value.Day);
        }
    }
}
