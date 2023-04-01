namespace GraphQL.AutoUnions
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Provides a method for generating the name of a union given the actual type and member types.
    /// </summary>
    /// <typeparam name="TUnion">The type of the union.</typeparam>
    public interface IUnionNameProvider<TUnion>
    {
        /// <summary>
        /// Generates the name of the union given the actual type and member types.
        /// </summary>
        /// <param name="actualType">The actual type of the object.</param>
        /// <param name="unionMemberGraphTypes">The member types of the union as graph types.</param>
        /// <returns>The name of the union.</returns>
        string Provide(Type actualType, IReadOnlyCollection<Type> unionMemberGraphTypes);
    }
}