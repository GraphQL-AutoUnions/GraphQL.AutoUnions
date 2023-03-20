namespace GraphQL
{
    using System;
    using System.Collections.Generic;
    using GraphQL.AutoUnions;
    using GraphQL.DI;
    using GraphQL.Types;

    public static class GraphQlBuilderExtensionMethods
    {
        /// <summary>
        /// Adds one of union support.
        /// </summary>
        public static IGraphQLBuilder AddAutoUnions<T>(
            this IGraphQLBuilder builder,
            Func<T, object> selectValue,
            Func<Type, Type[]> selectMembers,
            Func<Type, IReadOnlyCollection<Type>, string> selectName = null)
            where T : class
        {
            var builderServices = builder.Services;

            builderServices.Register(
                typeof(IUnionValueAccessor<T>),
                implementationFactory: (_) => new UnionValueAccessor<T>(selectValue),
                ServiceLifetime.Singleton);
            
            builderServices.Register(
                typeof(IUnionMemberTypeResolver<T>),
                implementationFactory: (_) => new UnionMemberTypeResolver<T>(selectMembers),
                ServiceLifetime.Singleton);
            
            builderServices.Register(
                typeof(IUnionNameProvider<T>),
                implementationFactory: (_) => new UnionNameProvider<T>(selectName),
                ServiceLifetime.Singleton);
            
            builderServices.Register(
                typeof(IUnionMemberProvider<T>),
                typeof(UnionMemberProvider<T>),
                ServiceLifetime.Singleton);
            
            builderServices.Register(
                typeof(IUnionCast<T>),
                implementationFactory: (_) => new UnionCast<T>(),
                ServiceLifetime.Singleton);

            builderServices.Register(
                typeof(IUnionResolverFactory<T>),
                typeof(UnionResolverFactory<T>),
                ServiceLifetime.Singleton);

            builderServices.TryRegister(
                typeof(AutoRegisteringAutoUnionGraphType<,>),
                typeof(AutoRegisteringAutoUnionGraphType<,>),
                ServiceLifetime.Singleton);

            builderServices.TryRegister(
                typeof(IIsTypeOfFactory<T>),
                typeof(IsTypeOfFactory<T>),
                ServiceLifetime.Singleton);

            builderServices.TryRegister(
                typeof(IUnionMemberFieldResolverFactory<T>),
                typeof(UnionMemberFieldResolverFactory<T>),
                ServiceLifetime.Singleton);

            builderServices.Register(
                typeof(IGraphTypeMappingProvider),
                typeof(AutoRegisteringUnionGraphTypeMappingProvider<T>),  ServiceLifetime.Singleton);

            builder.ConfigureSchema((schema) =>
            {
                var collect = new CollectUnionMembersVisitor();

                schema.RegisterVisitor(collect);
                schema.RegisterVisitor(new ModifyUnionMemberVisitor(collect));
            });

            return builder;
        }
    }
}