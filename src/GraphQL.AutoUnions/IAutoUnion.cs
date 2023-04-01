namespace GraphQL.AutoUnions
{
    using GraphQL.Types;

    internal interface IAutoUnion
    {
        bool IsMember(IGraphType member);

        void ModifyMember(IObjectGraphType graphType);
    }
}