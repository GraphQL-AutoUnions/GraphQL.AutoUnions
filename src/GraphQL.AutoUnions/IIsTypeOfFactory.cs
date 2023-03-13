namespace GraphQL.AutoUnions
{
    using System;

    public interface IIsTypeOfFactory<TUnion>
    {
        Func<object, bool> Create(Func<object, bool> previous);
    }
}