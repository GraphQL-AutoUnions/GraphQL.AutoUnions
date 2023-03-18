namespace GraphQL
{
    using System.Collections.Generic;
    using GraphQL.AutoUnions;
    using GraphQL.Types;
    using GraphQL.Utilities;

    internal class CollectUnionMembersVisitor : BaseSchemaNodeVisitor
    {
        private HashSet<IAutoUnion> unions = new HashSet<IAutoUnion>();
        
        public override void VisitUnion(UnionGraphType type, ISchema schema)
        {
            base.VisitUnion(type, schema);

            var union = type as IAutoUnion;

            if (union != null)
            {
                this.unions.Add(union);
            }
        }

        public IReadOnlyCollection<IAutoUnion> Unions => this.unions;
    }
}