namespace GraphQL.AutoUnions
{
    using System;
    using System.Linq;
    using GraphQL.Types;

    internal class UnionResolverFactory<TUnion> : IUnionResolverFactory<TUnion>
    {
        private readonly IUnionCast<TUnion> _unionCast;
        private readonly IUnionValueAccessor<TUnion> _unionValueAccessor;

        public UnionResolverFactory(
            IUnionCast<TUnion> unionCast,
            IUnionValueAccessor<TUnion> unionValueAccessor)
        {
            this._unionCast = unionCast ?? throw new ArgumentNullException(nameof(unionCast));
            this._unionValueAccessor = unionValueAccessor ?? throw new ArgumentNullException(nameof(unionValueAccessor));
        }
        
        public Func<object, IObjectGraphType> Create(Type[] unionMemberGraphTypes)
        {
            var unionMemberGraphqlTypes = 
                unionMemberGraphTypes.ToDictionary(
                    type => TypeExtensionMethods.GraphQlUnionMemberClrType(type),
                    type => new GraphQLTypeReference(TypeExtensionMethods.GraphQlUnionMemberName(type)));

            return (result) =>
            {
                if (this._unionCast.TryCast(result, out var oneOf))
                {
                    result = this._unionValueAccessor.Access(oneOf);
                }

                if (unionMemberGraphqlTypes.TryGetValue(result.GetType(), out var objectGraphType))
                {
                    return objectGraphType;
                }

                throw new InvalidOperationException("Cannot determine graph type!");
            };
        }
    }
}