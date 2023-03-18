namespace GraphQL.AutoUnions
{
    using GraphQL.Types;

    public interface IAutoUnion
    {
        bool IsMember(IGraphType member);
        void ModifyMember(IObjectGraphType graphType);
    }
}