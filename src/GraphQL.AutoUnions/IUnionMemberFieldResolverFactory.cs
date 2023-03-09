namespace GraphQL.AutoUnions
{
    using GraphQL.Resolvers;

    internal interface IUnionMemberFieldResolverFactory<TUnion>
    {
        IFieldResolver Create(IFieldResolver parentResolver);
    }
}