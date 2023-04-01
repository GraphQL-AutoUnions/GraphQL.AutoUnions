namespace GraphQL.AutoUnions
{
    /// <summary>
    /// Provides a method for casting an object to a union member type.
    /// </summary>
    /// <typeparam name="TUnion">The type of the union.</typeparam>
    public interface IUnionCast<TUnion>
    {
        /// <summary>
        /// Attempts to cast the specified object to a union member type.
        /// </summary>
        /// <param name="obj">The object to cast.</param>
        /// <param name="union">The resulting union member type, if the cast succeeded.</param>
        /// <returns>True if the cast succeeded, false otherwise.</returns>
        bool TryCast(object obj, out TUnion union);
    }
}