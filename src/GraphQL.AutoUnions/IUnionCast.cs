namespace GraphQL.AutoUnions
{
    internal interface IUnionCast<TUnion>
    {
        bool TryCast(object obj, out TUnion union);
    }
}