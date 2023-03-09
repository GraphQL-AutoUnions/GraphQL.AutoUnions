namespace GraphQL.AutoUnions
{
    using System;
    using System.Threading.Tasks;
    using GraphQL.Resolvers;

    internal class UnionMemberFieldResolver<TUnion> : IFieldResolver
    {
        private readonly IFieldResolver _parentResolver;
        private readonly IUnionCast<TUnion> _unionCast;
        private readonly IUnionValueAccessor<TUnion> _unionValueAccessor;

        public UnionMemberFieldResolver(
            IFieldResolver parentResolver,
            IUnionCast<TUnion> unionCast,
            IUnionValueAccessor<TUnion> unionValueAccessor)
        {
            this._parentResolver = parentResolver ?? throw new ArgumentNullException(nameof(parentResolver));
            this._unionCast = unionCast ?? throw new ArgumentNullException(nameof(unionCast));
            this._unionValueAccessor = unionValueAccessor ?? throw new ArgumentNullException(nameof(unionValueAccessor));
        }

        public ValueTask<object> ResolveAsync(IResolveFieldContext context)
        {
            return this._parentResolver.ResolveAsync(
                new UnionResolveFieldContext<TUnion>(context, this._unionCast, this._unionValueAccessor)
            );
        }
    }
}