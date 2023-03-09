namespace GraphQL.AutoUnions
{
    using System;

    public interface IUnionMemberProvider<TUnion>
    {
        Type[] Provide(Type unionImplementation);
    }
}