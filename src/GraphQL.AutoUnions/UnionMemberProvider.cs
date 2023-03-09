namespace GraphQL.AutoUnions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GraphQL.Execution;
    using GraphQL.Types;

    internal class UnionMemberProvider<TUnion> : IUnionMemberProvider<TUnion>
    {
        private readonly IEnumerable<IGraphTypeMappingProvider> _graphTypeMappingProviders;
        private readonly IUnionMemberTypeResolver<TUnion> _unionMemberTypeResolver;

        public UnionMemberProvider(
            IEnumerable<IGraphTypeMappingProvider> graphTypeMappingProviders,
            IUnionMemberTypeResolver<TUnion> unionMemberTypeResolver)
        {
            this._graphTypeMappingProviders =
                graphTypeMappingProviders ?? throw new ArgumentNullException(nameof(graphTypeMappingProviders));
            this._unionMemberTypeResolver =
                unionMemberTypeResolver ?? throw new ArgumentNullException(nameof(unionMemberTypeResolver));
        }

        private Type GetForMember(Type unionMemberClrType)
        {
            Type type = null;
        
            foreach (var graphTypeMappingProvider in this._graphTypeMappingProviders)
            {
                type = graphTypeMappingProvider.GetGraphTypeFromClrType(
                    unionMemberClrType, 
                    false,
                    typeof(AutoRegisteringUnionMemberGraphType<,>
                ).MakeGenericType(typeof(TUnion), unionMemberClrType));
            }

            if (type is null)
            {
                throw new InvalidOperationError($"AutoUnion member could not be determined for clr type '{unionMemberClrType}'");
            }

            return type;
        }

        public Type[] Provide(Type unionImplementation)
        {
            return this._unionMemberTypeResolver
                .Resolve(unionImplementation)
                .Select(this.GetForMember)
                .ToArray();
        }
    }
}