namespace GraphQL.AutoUnions
{
    using System;

    internal interface IIsTypeOfFactory<TUnion>
    {
        Func<object, bool> Create(Func<object, bool> previous);
    }
}