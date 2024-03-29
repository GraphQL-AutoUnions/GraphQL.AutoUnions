namespace GraphQL.AutoUnions
{
    using System;
    using GraphQL.Types;

    /// <inheritdoc cref="IGraphTypeMappingProvider"/>
    internal class AutoRegisteringUnionGraphTypeMappingProvider<TUnion> : IGraphTypeMappingProvider
    {
        /// <inheritdoc cref="IGraphTypeMappingProvider"/>
        public Type GetGraphTypeFromClrType(Type clrType, bool isInputType, Type preferredGraphType)
        {
            if (!typeof(TUnion).IsAssignableFrom(clrType))
            {
                return preferredGraphType;
            }

            return typeof(AutoRegisteringAutoUnionGraphType<,>).MakeGenericType(typeof(TUnion), clrType);
        }
    }
}