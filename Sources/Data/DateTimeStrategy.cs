using System;

namespace DataCreationFramework.Data
{
    public class DateTimeStrategy : DataCreationStrategy<DateTime>
    {
        private DateTime? value;

        public void Value(DateTime value)
        {
            this.value = value;
        }


        public override DateTime GetValue()
        {
            if (value.HasValue)
            {
                return value.Value;
            }

            return DateTime.Now;
        }
    }
}
