namespace GraphQL.AutoUnions
{
    internal interface IUnionValueAccessor<T>
    {
        object Access(T union);
    }
}