using System;

namespace DataCreationFramework.Data
{
    public class BooleanStrategy : DataCreationStrategy<bool>
    {
        private Random _random = new Random();
        private bool _value = false;
        private BooleanStrategyType _strategyType = BooleanStrategyType.Default;

        private enum BooleanStrategyType
        {
            Default,
            Toggle,
            Random
        }

        public void Toggle(bool startsWith = false)
        {
            _strategyType = BooleanStrategyType.Toggle;
            _value = startsWith;
        }

        public void Value(bool value)
        {
            _value = value;
        }

        public void Random()
        {
            _strategyType = BooleanStrategyType.Random;
        }
               
        public override bool GetValue()
        {
            bool resultValue = _value;

            if (_strategyType == BooleanStrategyType.Toggle)
            {
                _value = !_value;
            }
            else if (_strategyType == BooleanStrategyType.Random)
            {
                resultValue = (_random.Next(int.MinValue, int.MaxValue) % 2) == 0;
            }

            return resultValue;
        }
    }
}
