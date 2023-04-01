namespace GraphQL.AutoUnions
{
    using System;
    using System.Linq;
    using GraphQL.Types;

    /// <summary>
    /// Represents an auto-registering union graph type that automatically registers all of its union member types and
    /// provides automatic resolution of the concrete union member types based on a discriminator field.
    /// </summary>
    /// <typeparam name="TUnion">The union type.</typeparam>
    /// <typeparam name="TUnionImplementation">The implementation of the union type.</typeparam>
    public class AutoRegisteringAutoUnionGraphType<TUnion, TUnionImplementation> : UnionGraphType, IAutoUnion
    {
        private readonly IUnionCast<TUnion> _unionCast;
        private readonly Type[] _unionMemberGraphTypes;
        private readonly IUnionMemberFieldResolverFactory<TUnion> _unionMemberFieldResolverFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoRegisteringAutoUnionGraphType{TUnion, TUnionImplementation}"/> class.
        /// </summary>
        /// <param name="unionNameProvider">The union name provider.</param>
        /// <param name="unionMemberProvider">The union member provider.</param>
        /// <param name="unionResolverFactory">The union resolver factory.</param>
        /// <param name="unionCast">The union cast.</param>
        /// <param name="unionMemberFieldResolverFactory">The union member field resolver factory.</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the arguments are null.</exception>
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

            this._unionCast = unionCast;
            this._unionMemberFieldResolverFactory = unionMemberFieldResolverFactory;

            this._unionMemberGraphTypes = unionMemberProvider.Provide(typeof(TUnionImplementation));

            foreach (var unionMemberGraphType in this._unionMemberGraphTypes)
            {
                this.Type(unionMemberGraphType);
            }

            this.Name = unionNameProvider.Provide(typeof(TUnionImplementation), this._unionMemberGraphTypes);
            this.ResolveType = unionResolverFactory.Create(this._unionMemberGraphTypes);
        }

        /// <summary>
        /// Checks if the graph type is a member of this union.
        /// </summary>
        /// <param name="member">The potential member graph type.</param>
        /// <returns>True if the member is part of the union.</returns>
        public bool IsMember(IGraphType member)
        {
            return this._unionMemberGraphTypes.Any((gt) => gt == member.GetType());
        }

        /// <summary>
        /// Modify the union member object graph type to support usage as the union member. 
        /// </summary>
        /// <param name="graphType">The graph type to modify.</param>
        public void ModifyMember(IObjectGraphType graphType)
        {
            var isTypeOf = graphType.IsTypeOf;

            graphType.IsTypeOf = (o) => this._unionCast.TryCast(o, out _) || (isTypeOf?.Invoke(o) ?? false);

            foreach (var fieldType in graphType.Fields)
            {
                if (fieldType.Resolver is null)
                {
                    continue;
                }

                fieldType.Resolver = this._unionMemberFieldResolverFactory.Create(fieldType.Resolver);
            }
        }
    }
}