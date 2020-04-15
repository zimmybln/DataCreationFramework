using System;
using System.Collections.Generic;

namespace DataCreationFramework.Data
{
    public class StringStrategy : DataCreationStrategy<string>
    {
        private static Random _random = new Random();

        private int _maxLength = 0;
        private string _prefix = null;
        private string _suffix = null;
        private List<string> _availableValues = new List<string>();
        private string _value = null;
        private bool _isValueUsed = false;

        public StringStrategy Length(int value)
        {
            _maxLength = value;
            return this;
        }

        public StringStrategy Prefix(string value)
        {
            _prefix = value;
            return this;
        }

        public StringStrategy Suffix(string value)
        {
            _suffix = value;
            return this;
        }

        public void OneOfTheseValues(params string[] values)
        {
            _availableValues.AddRange(values);
        }

        public void Value(string value)
        {
            _isValueUsed = true;
            _value = value;
        }

        public override string GetValue()
        {
            string value = null;

            if (_isValueUsed)
            {
                value = _value;
            }
            else if (_availableValues != null && _availableValues.Count > 0)
            {
                value = _availableValues[_random.Next(0, _availableValues.Count)];
            }
            else
            {
                value = Common.CreateUniqueText(_prefix, _maxLength > 0 ? _maxLength : 200, CreateTextOptions.Randomized);
            }

            if (!_isValueUsed && !String.IsNullOrEmpty(_suffix))
            {
                if (_maxLength > 0 && (value.Length + _suffix.Length > _maxLength))
                {
                    value = value.Substring(0, value.Length - _suffix.Length);
                }

                value += _suffix;
            }

            return value;
        }

    }
}
