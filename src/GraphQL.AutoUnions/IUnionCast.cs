namespace GraphQL.AutoUnions
{
    public interface IUnionCast<TUnion>
    {
        bool TryCast(object obj, out TUnion union);
    }
}