namespace GraphQL.AutoUnions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class UnionNameProvider<TUnion> : IUnionNameProvider<TUnion>
    {
        private readonly Func<Type, IReadOnlyCollection<Type>, string> _selectName;

        public UnionNameProvider(Func<Type, IReadOnlyCollection<Type>, string> selectName)
        {
            this._selectName = selectName;
        }
        
        public string Provide(Type actualType, IReadOnlyCollection<Type> unionMemberGraphTypes)
        {
            var nameMaybe = this._selectName?.Invoke(actualType, unionMemberGraphTypes);

            if (nameMaybe != null)
            {
                return nameMaybe;
            }
            
            var unionMemberNames =
                unionMemberGraphTypes.Select((unionMemberGraphType) => unionMemberGraphType.GraphQlUnionMemberName());

            return "_" + string.Join("_", unionMemberNames) + "_";
        }
    }
}