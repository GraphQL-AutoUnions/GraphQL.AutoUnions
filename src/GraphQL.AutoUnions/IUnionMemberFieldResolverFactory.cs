namespace GraphQL.AutoUnions
{
    using GraphQL.Resolvers;

    /// <summary>
    /// Provides a method for creating a field resolver for a union member field.
    /// </summary>
    /// <typeparam name="TUnion">The type of the union.</typeparam>
    public interface IUnionMemberFieldResolverFactory<TUnion>
    {
        /// <summary>
        /// Creates a field resolver for a union member field.
        /// </summary>
        /// <param name="parentResolver">The parent field resolver.</param>
        /// <returns>The field resolver for the union member field.</returns>
        IFieldResolver Create(IFieldResolver parentResolver);
    }
}