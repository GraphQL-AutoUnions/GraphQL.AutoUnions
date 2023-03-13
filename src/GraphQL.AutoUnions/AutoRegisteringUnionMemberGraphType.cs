namespace GraphQL.AutoUnions
{
    using System;
    using GraphQL.Types;

    /// <inheritdoc />
    public class AutoRegisteringUnionMemberGraphType<TUnion, TUnionMember> : AutoRegisteringObjectGraphType<TUnionMember>
    {
        /// <inheritdoc />
        public AutoRegisteringUnionMemberGraphType(
            IIsTypeOfFactory<TUnion> isTypeOfFactory,
            IUnionMemberFieldResolverFactory<TUnion> unionMemberFieldResolverFactory
        )
        {
            if (isTypeOfFactory is null) throw new ArgumentNullException(nameof(isTypeOfFactory));
            if (unionMemberFieldResolverFactory is null) throw new ArgumentNullException(nameof(unionMemberFieldResolverFactory));
            
            this.IsTypeOf = isTypeOfFactory.Create(this.IsTypeOf);
            
            foreach (var fieldType in this.Fields)
            {
                if (fieldType.Resolver is null)
                {
                    continue;
                }

                fieldType.Resolver = unionMemberFieldResolverFactory.Create(fieldType.Resolver);
            }
        }
    }
}