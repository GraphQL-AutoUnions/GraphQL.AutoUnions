namespace GraphQL.AutoUnions
{
    using System;
    using System.Linq;
    using GraphQL.Types;

    public class AutoRegisteringAutoUnionGraphType<TUnion, TUnionImplementation> : UnionGraphType, IAutoUnion
    {
        private readonly IUnionCast<TUnion> unionCast;
        private readonly Type[] unionMemberGraphTypes;
        private readonly IUnionMemberFieldResolverFactory<TUnion> unionMemberFieldResolverFactory;

        public AutoRegisteringAutoUnionGraphType(
            IUnionNameProvider<TUnion> unionNameProvider,
            IUnionMemberProvider<TUnion> unionMemberProvider,
            IUnionResolverFactory<TUnion> unionResolverFactory,
            IUnionCast<TUnion> unionCast,
            IUnionMemberFieldResolverFactory<TUnion> unionMemberFieldResolverFactory)
        {
            if (unionNameProvider is null) throw new ArgumentNullException(nameof(unionNameProvider));
            if (unionMemberProvider is null) throw new ArgumentNullException(nameof(unionMemberProvider));
            if (unionResolverFactory is null) throw new ArgumentNullException(nameof(unionResolverFactory));
            if (unionCast is null) throw new ArgumentNullException(nameof(unionCast));

            this.unionCast = unionCast;
            this.unionMemberFieldResolverFactory = unionMemberFieldResolverFactory;

            this.unionMemberGraphTypes = unionMemberProvider.Provide(typeof(TUnionImplementation));

            foreach (var unionMemberGraphType in this.unionMemberGraphTypes)
            {
                this.Type(unionMemberGraphType);
            }

            this.Name = unionNameProvider.Provide(typeof(TUnionImplementation), this.unionMemberGraphTypes);
            this.ResolveType = unionResolverFactory.Create(this.unionMemberGraphTypes);
        }

        public bool IsMember(IGraphType member)
        {
            return this.unionMemberGraphTypes.Any((gt) => gt == member.GetType());
        }

        public void ModifyMember(IObjectGraphType graphType)
        {
            var isTypeOf = graphType.IsTypeOf;

            graphType.IsTypeOf = (o) => this.unionCast.TryCast(o, out _) || (isTypeOf?.Invoke(o) ?? false);

            foreach (var fieldType in graphType.Fields)
            {
                if (fieldType.Resolver is null)
                {
                    continue;
                }

                fieldType.Resolver = this.unionMemberFieldResolverFactory.Create(fieldType.Resolver);
            }
        }
    }
}