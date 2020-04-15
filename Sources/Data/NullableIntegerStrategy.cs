using System;
using System.Collections.Generic;

namespace DataCreationFramework.Data
{
    public class NullableIntegerStrategy : DataCreationStrategy<int?>, IEdgeValues<int?>
    {
        private static Random _random = new Random();

        private enum IntegerStrategyType
        {
            Default,
            Random,
            Increment,
            Decrement,
            Unique,
            Value,
            OneOfTheseValues
        }

        private IntegerStrategyType strategyType = IntegerStrategyType.Default;
        private int _min = int.MinValue;
        private int _max = int.MaxValue;
        private int? _value;
        private List<int> _uniqueList;

        private int _incrementCurrentValue;
        private int _incrementCurrentValueDefault;
        private int _decrementCurrentValue;
        private int _decrementCurrentValueDefault;

        private List<int> _availableValues = new List<int>();

        public IEdgeValues<int?> Min(int? value)
        {
            _min = value.HasValue ? value.Value : int.MinValue;
            return this;
        }

        public IEdgeValues<int?> Max(int? value)
        {
            _max = value.HasValue ? value.Value : int.MaxValue;
            return this;
        }

        public void Value(int? value)
        {
            strategyType = IntegerStrategyType.Value;
            _value = value;
        }

        public IEdgeValues<int?> Unique()
        {
            strategyType = IntegerStrategyType.Unique;
            _uniqueList = new List<int>();

            return this;
        }

        public void Increment(int startsWith = 1)
        {
            strategyType = IntegerStrategyType.Increment;
            _incrementCurrentValue = startsWith;
            _incrementCurrentValueDefault = startsWith;
        }

        public void Decrement(int startsWith = 1)
        {
            strategyType = IntegerStrategyType.Decrement;
            _decrementCurrentValue = startsWith;
            _decrementCurrentValueDefault = startsWith;
        }

        public void OneOfTheseValues(params int[] values)
        {
            strategyType = IntegerStrategyType.OneOfTheseValues;
            _availableValues.AddRange(values);
        }

        protected internal override void Reset()
        {
            _uniqueList?.Clear();
            _decrementCurrentValue = _decrementCurrentValueDefault;
            _incrementCurrentValue = _incrementCurrentValueDefault;
        }

        public override int? GetValue()
        {
            if (strategyType == IntegerStrategyType.Value)
            {
                return _value;
            }
            else if (strategyType == IntegerStrategyType.Unique)
            {
                int min = _min;
                int max = _max;

                int value = _random.Next(_min, _max);

                while (_uniqueList.Contains(value))
                {
                    value = _random.Next(_min, _max);
                }

                _uniqueList.Add(value);

                return value;

            }
            else if (strategyType == IntegerStrategyType.OneOfTheseValues)
            {
                return _availableValues[_random.Next(0, _availableValues.Count)];
            }
            else if (strategyType == IntegerStrategyType.Increment)
            {
                return _incrementCurrentValue++;
            }
            else if (strategyType == IntegerStrategyType.Decrement)
            {
                return _decrementCurrentValue--;
            }
            else if (strategyType == IntegerStrategyType.Random)
            {
                return _random.Next(_min, _max);
            }

            return null;
        }
    }
}
