using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DataCreationFramework.Data
{
    public class ListStrategy<T> : DataCreationStrategy<List<T>>, IEdgeValues<int>
    {
        private readonly DataCreationStrategy<T> _embeddedStrategy;
        private int _min = 0;
        private int _max = 0;
        private int _amount = 0;
        private Random _random = new Random();

        public ListStrategy(DataCreationStrategy<T> embeddedStrategy)
        {
            _embeddedStrategy = embeddedStrategy;
        }

        public void Amount(int number)
        {
            Contract.Assert(number > 0);

            _amount = number;
        }

        public override List<T> GetValue()
        {
            List<T> list = new List<T>();
            int amount = 100;

            if (_amount > 0)
            {
                amount = _amount;
            }
            else if (_min > 0 && _max > 0 && _max > _min)
            {
                amount = _random.Next(_min, _max);
            }

            _embeddedStrategy.Reset();

            for (int i = 0; i < amount; i++)
            {
                list.Add(_embeddedStrategy.GetValue());
            }

            return list;
        }

        public IEdgeValues<int> Max(int value)
        {
            _max = value;
            return this;
        }

        public IEdgeValues<int> Min(int value)
        {
            _min = value;
            return this;
        }
    }
}
