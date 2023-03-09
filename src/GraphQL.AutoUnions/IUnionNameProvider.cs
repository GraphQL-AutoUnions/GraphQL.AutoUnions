namespace GraphQL.AutoUnions
{
    using System;
    using System.Collections.Generic;

    public interface IUnionNameProvider<TUnion>
    {
        string Provide(Type actualType, IReadOnlyCollection<Type> unionMemberGraphTypes);
    }
}