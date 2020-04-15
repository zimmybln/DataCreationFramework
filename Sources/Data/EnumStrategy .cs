using System;

namespace DataCreationFramework.Data
{
    public class EnumStrategy<T> : DataCreationStrategy<T>
    {
        private T value;

        public void Value(T value)
        {
            this.value = value;
        }

        public override T GetValue()
        {
            return value;
        }
    }
}
