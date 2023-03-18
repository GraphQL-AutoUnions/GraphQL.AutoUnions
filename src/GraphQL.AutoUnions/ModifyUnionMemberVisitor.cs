namespace GraphQL
{
    using System.Linq;
    using GraphQL.Types;
    using GraphQL.Utilities;

    internal class ModifyUnionMemberVisitor : BaseSchemaNodeVisitor
    {
        private readonly CollectUnionMembersVisitor _visitor;

        public ModifyUnionMemberVisitor(CollectUnionMembersVisitor visitor)
        {
            this._visitor = visitor;
        }

        public override void VisitObject(IObjectGraphType type, ISchema schema)
        {
            base.VisitObject(type, schema);

            foreach (var union in this._visitor.Unions.Where(u => u.IsMember(type)))
            {
                union.ModifyMember(type);
            }
        }
    }
}