namespace GraphQL
{
    using System;
    using System.Linq;
    using System.Reflection;
    using GraphQL.DI;
    using GraphQL.Execution;
    using GraphQL.Types;
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
                    var type = oneOfType;

                    while (type != typeof(object))
                    {
                        if (type.IsGenericType &&
                            (type.GetGenericTypeDefinition().AssemblyQualifiedName.StartsWith("OneOf.OneOfBase`") ||
                             type.GetGenericTypeDefinition().AssemblyQualifiedName.StartsWith("OneOf.IOneOf`")))
                        {
                            return type.GetGenericArguments();
                        }

                        type = oneOfType.BaseType;
                    }

                    return oneOfType.GetGenericArguments();
                },
                (actualType, _) => actualType.GraphQLName()
            );

            return builder;
        }
    }
}