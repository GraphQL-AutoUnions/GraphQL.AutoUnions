namespace GraphQL.AutoUnions
{
    using System;
    using GraphQL.Types;

    public class AutoRegisteringUnionGraphType<TUnion, TUnionImplementation> : UnionGraphType
    {
        public AutoRegisteringUnionGraphType(
            IUnionNameProvider<TUnion> unionNameProvider,
            IUnionMemberProvider<TUnion> unionMemberProvider,
            IUnionResolverFactory<TUnion> unionResolverFactory)
        {
            if (unionNameProvider is null) throw new ArgumentNullException(nameof(unionNameProvider));
            if (unionMemberProvider is null) throw new ArgumentNullException(nameof(unionMemberProvider));
            if (unionResolverFactory is null) throw new ArgumentNullException(nameof(unionResolverFactory));
            
            var unionMemberGraphTypes = unionMemberProvider.Provide(typeof(TUnionImplementation));

            foreach (var unionMemberGraphType in unionMemberGraphTypes)
            {
                this.Type(unionMemberGraphType);
            }

            this.Name = unionNameProvider.Provide(typeof(TUnionImplementation), unionMemberGraphTypes);
            this.ResolveType = unionResolverFactory.Create(unionMemberGraphTypes);
        }
    }
}
