using DataCreationFramework.Exceptions;

namespace DataCreationFramework.Data
{
    public partial class DefinitionOfValid<T>
    where T : class
    {
        public class IntegerValidity : Validity
        {
            private int? _min = null;
            private int? _max = null;
            private bool? _isRequired;

            public IntegerValidity IsRequired()
            {
                _isRequired = true;
                return this;
            }

            public IntegerValidity Min(int value)
            {
                _min = value;
                return this;
            }

            public IntegerValidity Max(int value)
            {
                _max = value;
                return this;
            }

            public override void ApplyDataCreationStrategy(string propertyName, DataCreationStrategy<T> strategy, ViolationType violationType)
            {
                if (violationType == ViolationType.MaximumValue && _max.HasValue)
                {
                    IntegerStrategy integerStrategy = new IntegerStrategy();

                    integerStrategy.Increment(_max.Value + 1);
                    strategy.Add<int, IntegerStrategy>(propertyName, integerStrategy);
                }
                else if (violationType == ViolationType.MinimumValue && _min.HasValue)
                {
                    IntegerStrategy integerStrategy = new IntegerStrategy();

                    integerStrategy.Decrement(_min.Value - 1);

                    strategy.Add<int, IntegerStrategy>(propertyName, integerStrategy);
                }
                else if (violationType == ViolationType.OppositeValue)
                {
                    IntegerStrategy integerStrategy = new IntegerStrategy();

                    integerStrategy.Value(0);

                    strategy.Add<int, IntegerStrategy>(propertyName, integerStrategy);
                }
                else
                {
                    throw new UnsupportedViolationException();
                }
            }
        }
    }
}
