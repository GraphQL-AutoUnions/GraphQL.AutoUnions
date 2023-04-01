namespace GraphQL.AutoUnions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a method for resolving the member types of a union given the actual type.
    /// </summary>
    /// <typeparam name="T">The type of the discriminator field.</typeparam>
    internal interface IUnionMemberTypeResolver<T>
    {
        /// <summary>
        /// Resolves the member types of a union given the actual type.
        /// </summary>
        /// <param name="actualType">The actual type of the object.</param>
        /// <returns>A read-only collection of member types for the union.</returns>
        IReadOnlyCollection<Type> Resolve(Type actualType);
    }
}