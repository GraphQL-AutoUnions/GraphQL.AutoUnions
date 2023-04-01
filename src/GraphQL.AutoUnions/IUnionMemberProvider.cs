namespace GraphQL.AutoUnions
{
    using System;

    /// <summary>
    /// Provides a method for retrieving the member types of a union.
    /// </summary>
    /// <typeparam name="TUnion">The type of the union.</typeparam>
    public interface IUnionMemberProvider<TUnion>
    {
        /// <summary>
        /// Retrieves the member types of the specified union implementation.
        /// </summary>
        /// <param name="unionImplementation">The union implementation type.</param>
        /// <returns>An array of member types for the union implementation.</returns>
        Type[] Provide(Type unionImplementation);
    }
}