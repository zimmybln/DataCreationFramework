using System;

namespace DataCreationFramework.Data
{
    public class NullableBooleanStrategy : DataCreationStrategy<bool?>
    {
        private Random _random = new Random();
        private bool? _value = false;
        private BooleanStrategyType _strategyType = BooleanStrategyType.Default;

        private enum BooleanStrategyType
        {
            Default,
            Value,
            Toggle,
            Random
        }

        public void Toggle(bool? startsWith = false)
        {
            _strategyType = BooleanStrategyType.Toggle;
            _value = startsWith;
        }

        public void Value(bool? value)
        {
            _strategyType = BooleanStrategyType.Value;
            _value = value;
        }

        public void Random()
        {
            _strategyType = BooleanStrategyType.Random;
        }

        public override bool? GetValue()
        {
            if (_strategyType == BooleanStrategyType.Value)
            {
                return _value;
            }
            if (_strategyType == BooleanStrategyType.Toggle)
            {
                return !_value;
            }
            else if (_strategyType == BooleanStrategyType.Random)
            {
                return (_random.Next(int.MinValue, int.MaxValue) % 2) == 0;
            }

            return null;
        }
    }
}