namespace DataCreationFramework.Data
{
    public interface IEdgeValues<T>
    {
        IEdgeValues<T> Min(T value);

        IEdgeValues<T> Max(T value);
    }
}
