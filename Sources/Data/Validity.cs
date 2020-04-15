namespace DataCreationFramework.Data
{
    public partial class DefinitionOfValid<T>
    where T : class
    {
        public abstract class Validity
        {
            public abstract void ApplyDataCreationStrategy(string propertyName, DataCreationStrategy<T> strategy, ViolationType violationType);
        }
    }
}
