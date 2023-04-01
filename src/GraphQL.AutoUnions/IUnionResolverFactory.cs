namespace GraphQL.AutoUnions
{
    using System;
    using GraphQL.Types;

    /// <summary>
    /// Provides a method for creating a resolver function for a union.
    /// </summary>
    /// <typeparam name="TUnion">The type of the union.</typeparam>
    public interface IUnionResolverFactory<TUnion>
    {
        /// <summary>
        /// Creates a resolver function for the union.
        /// </summary>
        /// <param name="unionMemberGraphTypes">The member types of the union as graph types.</param>
        /// <returns>A function that resolves the union type for an object.</returns>
        Func<object, IObjectGraphType> Create(Type[] unionMemberGraphTypes);
    }
}