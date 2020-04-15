using DataCreationFramework.Exceptions;

namespace DataCreationFramework.Data
{
    public partial class DefinitionOfValid<T>
    where T : class
    {
        public class BooleanValidity : Validity
        {
            private bool? _value;

            public BooleanValidity Value(bool value)
            {
                _value = value;
                return this;
            }

            public override void ApplyDataCreationStrategy(string propertyName, DataCreationStrategy<T> strategy, ViolationType violationType)
            {
                if (violationType == ViolationType.OppositeValue)
                {
                    var booleanStrategy = new BooleanStrategy();

                    booleanStrategy.Value(!_value.Value);

                    strategy.Add<bool, BooleanStrategy>(propertyName, booleanStrategy);
                }
                else
                {
                    throw new UnsupportedViolationException();
                }
            }
        }
    }
}
