namespace GraphQL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GraphQL.DI;
    using OneOf;

    public static class GraphQlBuilderExtensionMethods
    {
        /// <summary>
        /// Adds one of union support.
        /// </summary>
        public static IGraphQLBuilder AddOneOfAutoUnions(this IGraphQLBuilder builder)
        {
            builder.AddAutoUnions<IOneOf>(
                (oneOf) => oneOf.Value,
                (oneOfType) =>
                {
                    return oneOfType.GetBaseTypes()
                        .FirstOrDefault((type) =>
                        {
                            var assemblyQualifiedName = type.GetGenericTypeDefinition().AssemblyQualifiedName;

                            return assemblyQualifiedName != null &&
                                   type.IsGenericType &&
                                   (assemblyQualifiedName.StartsWith("OneOf.OneOfBase`") ||
                                    assemblyQualifiedName.StartsWith("OneOf.IOneOf`"));
                        })
                        ?.GetGenericArguments() ?? throw new InvalidOperationException($"Cannot determine the member types of OneOf union {oneOfType.AssemblyQualifiedName}");
                },
                (actualType, _) => actualType.GraphQLName()
            );

            return builder;
        }
        
        private static IEnumerable<Type> GetBaseTypes(this Type type) {
            if(type.BaseType == null) return type.GetInterfaces();

            return Enumerable
                .Repeat(type.BaseType, 1)
                .Concat(type.GetInterfaces())
                .Concat(type.GetInterfaces().SelectMany<Type, Type>(GetBaseTypes))
                .Concat(type.BaseType.GetBaseTypes());
        }
    }
}