namespace GraphQL.AutoUnions
{
    using System;
    using GraphQL.Types;

    public interface IUnionResolverFactory<TUnion>
    {
        Func<object, IObjectGraphType> Create(Type[] unionMemberGraphTypes);
    }
}