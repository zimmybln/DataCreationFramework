using System;
using System.Collections.Generic;

namespace DataCreationFramework.Data
{
    public class DoubleStrategy : DataCreationStrategy<double>, IEdgeValues<double>
    {
        private static Random _random = new Random();

        private enum DoubleStrategyType
        {
            Random,
            Increment,
            Decrement,
            Unique,
            Value,
            OneOfTheseValues
        }

        private DoubleStrategyType strategyType = DoubleStrategyType.Random;
        private double _min = double.MinValue;
        private double _max = double.MaxValue;
        private double _value;
        private List<double> _uniqueList;

        private double _incrementCurrentValue;
        private double _incrementCurrentValueDefault;
        private double _decrementCurrentValue;
        private double _decrementCurrentValueDefault;

        private List<double> _availableValues = new List<double>();

        public IEdgeValues<double> Max(double value)
        {
            _max = value;
            return this;
        }

        public IEdgeValues<double> Min(double value)
        {
            _min = value;
            return this;
        }

        public void Value(double value)
        {
            _value = value;
            strategyType = DoubleStrategyType.Value;
        }

        public IEdgeValues<double> Unique()
        {
            strategyType = DoubleStrategyType.Unique;
            _uniqueList = new List<double>();

            return this;
        }

        public void Increment(double startsWith = 1)
        {
            strategyType = DoubleStrategyType.Increment;
            _incrementCurrentValue = startsWith;
            _incrementCurrentValueDefault = startsWith;
        }

        public void Decrement(double startsWith = 1)
        {
            strategyType = DoubleStrategyType.Decrement;
            _decrementCurrentValue = startsWith;
            _decrementCurrentValueDefault = startsWith;
        }

        public void OneOfTheseValues(params double[] values)
        {
            strategyType = DoubleStrategyType.OneOfTheseValues;
            _availableValues.AddRange(values);
        }

        protected internal override void Reset()
        {
            _uniqueList?.Clear();
            _decrementCurrentValue = _decrementCurrentValueDefault;
            _incrementCurrentValue = _incrementCurrentValueDefault;
        }

        public override double GetValue()
        {
            if (strategyType == DoubleStrategyType.Value)
            {
                return _value;
            }
            else if (strategyType == DoubleStrategyType.Unique)
            {
                double min = _min;
                double max = _max;

                double value = _random.NextDouble();

                //while (_uniqueList.Contains(value))
                //{
                //    value = _random.Next(_min, _max);
                //}

                _uniqueList.Add(value);

                return value;

            }
            else if (strategyType == DoubleStrategyType.OneOfTheseValues)
            {
                return _availableValues[_random.Next(0, _availableValues.Count)];
            }
            else if (strategyType == DoubleStrategyType.Increment)
            {
                return _incrementCurrentValue++;
            }
            else if (strategyType == DoubleStrategyType.Decrement)
            {
                return _decrementCurrentValue--;
            }

            return 0; // _random.Next(_min, _max);
        }
    }
}
