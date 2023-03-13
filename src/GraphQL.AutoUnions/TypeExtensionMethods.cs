namespace GraphQL.AutoUnions
{
    using System;
    using GraphQL.Execution;

    internal static class TypeExtensionMethods
    {
        public static Type GraphQlUnionMemberClrType(this Type type)
        {
            if (type.GetGenericTypeDefinition() != typeof(AutoRegisteringUnionMemberGraphType<,>))
            {
                throw new InvalidOperationError(
                    $"Cannot retrieve the GraphQlUnionMemberClrType from a non union type ({type.Name})"
                );
            }
            
            return type.GetGenericArguments()[1];
        }
        
        public static string GraphQlUnionMemberName(this Type type)
        {
            return type.GraphQlUnionMemberClrType().GraphQLName();
        }
    }
}