namespace GraphQL
{
    using System.Collections.Generic;
    using GraphQL.AutoUnions;
    using GraphQL.Types;
    using GraphQL.Utilities;

    internal class CollectUnionMembersVisitor : BaseSchemaNodeVisitor
    {
        private HashSet<IAutoUnion> _unions = new HashSet<IAutoUnion>();
        
        public override void VisitUnion(UnionGraphType type, ISchema schema)
        {
            base.VisitUnion(type, schema);

            var union = type as IAutoUnion;

            if (union != null)
            {
                this._unions.Add(union);
            }
        }

        public IReadOnlyCollection<IAutoUnion> Unions => this._unions;
    }
}