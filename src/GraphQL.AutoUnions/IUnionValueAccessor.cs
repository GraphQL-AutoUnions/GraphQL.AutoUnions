namespace GraphQL.AutoUnions
{
    public interface IUnionValueAccessor<T>
    {
        object Access(T union);
    }
}