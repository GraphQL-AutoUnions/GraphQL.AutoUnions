namespace GraphQL.AutoUnions
{
    using System;
    using System.Collections.Generic;

    public interface IUnionMemberTypeResolver<T>
    {
        IReadOnlyCollection<Type> Resolve(Type actualType);
    }
}