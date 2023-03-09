namespace GraphQL.AutoUnions
{
    using System;
    using GraphQL.Resolvers;

    internal class UnionMemberFieldResolverFactory<TUnion> : IUnionMemberFieldResolverFactory<TUnion>
    {
        private readonly IUnionCast<TUnion> _unionCast;
        private readonly IUnionValueAccessor<TUnion> _unionValueAccessor;

        public UnionMemberFieldResolverFactory(
            IUnionCast<TUnion> unionCast,
            IUnionValueAccessor<TUnion> unionValueAccessor)
        {
            this._unionCast = unionCast ?? throw new ArgumentNullException(nameof(unionCast));
            this._unionValueAccessor = unionValueAccessor ?? throw  new ArgumentNullException(nameof(unionValueAccessor));
        }
        
        public IFieldResolver Create(IFieldResolver parentResolver)
        {
            return new UnionMemberFieldResolver<TUnion>(parentResolver, this._unionCast, this._unionValueAccessor);
        }
    }
}