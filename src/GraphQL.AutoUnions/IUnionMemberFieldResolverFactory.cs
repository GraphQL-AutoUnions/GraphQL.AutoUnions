namespace GraphQL.AutoUnions
{
    using GraphQL.Resolvers;

    public interface IUnionMemberFieldResolverFactory<TUnion>
    {
        IFieldResolver Create(IFieldResolver parentResolver);
    }
}