using System;

namespace DataCreationFramework.Data
{
    public class GuidStrategy : DataCreationStrategy<Guid>
    {
        private enum GuidStrategyType
        {
            Empty,
            CreateNew,
            Value
        }

        private GuidStrategyType strategyType = GuidStrategyType.Empty;
        private Guid _value;

        public void CreateNew()
        {
            strategyType = GuidStrategyType.CreateNew;
        }

        public void Value(Guid guid)
        {
            strategyType = GuidStrategyType.Value;
            _value = guid;
        }


        public override Guid GetValue()
        {
            if (strategyType == GuidStrategyType.Value)
            {
                return _value;
            }

            if (strategyType == GuidStrategyType.CreateNew)
            {
                return Guid.NewGuid();
            }

            return Guid.Empty;
        }
    }
}
