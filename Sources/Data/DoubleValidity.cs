using DataCreationFramework.Exceptions;

namespace DataCreationFramework.Data
{
    public partial class DefinitionOfValid<T>
    where T : class
    {
        public class DoubleValidity : Validity
        {
            private double? _min = null;
            private double? _max = null;

            public DoubleValidity Min(double value)
            {
                _min = value;
                return this;
            }

            public DoubleValidity Max(double value)
            {
                _max = value;
                return this;
            }

            public override void ApplyDataCreationStrategy(string propertyName, DataCreationStrategy<T> strategy, ViolationType violationType)
            {
                if (violationType == ViolationType.MaximumValue && _max.HasValue)
                {
                    DoubleStrategy doubleStrategy = new DoubleStrategy();

                    doubleStrategy.Increment(_max.Value + 1);
                    strategy.Add<double, DoubleStrategy>(propertyName, doubleStrategy);
                }
                else if (violationType == ViolationType.MinimumValue && _min.HasValue)
                {
                    DoubleStrategy doubleStrategy = new DoubleStrategy();

                    doubleStrategy.Decrement(_min.Value - 1);

                    strategy.Add<double, DoubleStrategy>(propertyName, doubleStrategy);
                }
                else
                {
                    throw new UnsupportedViolationException();
                }
            }
        }
    }
}
