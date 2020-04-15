using System;
using System.Collections.Generic;

namespace DataCreationFramework.Data
{
    public class NullableGuidStrategy : DataCreationStrategy<Guid?>
    {
        private static Random _random = new Random();
        private readonly List<Guid?> _availableValues = new List<Guid?>();

        private enum GuidStrategyType
        {
            Empty,
            CreateNew,
            Value,
            ValueFromList
        }

        private GuidStrategyType strategyType = GuidStrategyType.Empty;
        private Guid? _value;

        public void CreateNew()
        {
            strategyType = GuidStrategyType.CreateNew;
        }

        public void Value(Guid? guid)
        {
            strategyType = GuidStrategyType.Value;
            _value = guid;
        }

        public void OneOfTheseValues(params Guid?[] values)
        {
            strategyType = GuidStrategyType.ValueFromList;
            _availableValues.AddRange(values);
        }


        public override Guid? GetValue()
        {
            if (strategyType == GuidStrategyType.Value)
            {
                return _value;
            }

            if (strategyType == GuidStrategyType.ValueFromList)
            {
                return _availableValues[_random.Next(0, _availableValues.Count)];
            }

            if (strategyType == GuidStrategyType.CreateNew)
            {
                return Guid.NewGuid();
            }

            return null;
        }
    }
}
