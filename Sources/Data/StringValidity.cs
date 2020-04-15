using DataCreationFramework.Exceptions;

namespace DataCreationFramework.Data
{
    public partial class DefinitionOfValid<T>
    where T : class
    {
        public class StringValidity : Validity
        {
            private enum StringValidationMode
            {
                Min, Max, NotNull
            }

            private StringValidationMode _validationMode;
            private int? _maxLength = null;
            private int? _minLength = null;

            public StringValidity NotNull()
            {
                _validationMode = StringValidationMode.NotNull;
                return this;
            }


            public StringValidity MaxLength(int maxLength)
            {
                _maxLength = maxLength;
                return this;
            }

            public StringValidity MinLength(int minLength)
            {
                _minLength = minLength;
                return this;
            }

            public override void ApplyDataCreationStrategy(string propertyName, DataCreationStrategy<T> strategy, ViolationType violationType)
            {
                if (violationType == ViolationType.MaximumLength)
                {
                    StringStrategy stringStrategy = new StringStrategy();

                    stringStrategy.Length(_maxLength.Value + 1);

                    strategy.Add<string, StringStrategy>(propertyName, stringStrategy);
                }
                else if (violationType == ViolationType.MinimumLength && _minLength >= 1)
                {
                    StringStrategy stringStrategy = new StringStrategy();

                    stringStrategy.Length(_minLength.Value - 1);

                    strategy.Add<string, StringStrategy>(propertyName, stringStrategy);

                }
                else if (violationType == ViolationType.Presence && _validationMode == StringValidationMode.NotNull)
                {
                    StringStrategy stringStrategy = new StringStrategy();

                    stringStrategy.Value(null);

                    strategy.Add<string, StringStrategy>(propertyName, stringStrategy);
                }
                else
                {
                    throw new UnsupportedViolationException();
                }
            }
        }
    }
}
