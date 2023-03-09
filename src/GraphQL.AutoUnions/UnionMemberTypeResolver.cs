namespace GraphQL.AutoUnions
{
    using System;
    using System.Collections.Generic;

    internal class UnionMemberTypeResolver<T> : IUnionMemberTypeResolver<T>
    {
        private readonly Func<Type, IReadOnlyCollection<Type>> _selectMembers;

        public UnionMemberTypeResolver(Func<Type, IReadOnlyCollection<Type>> selectMembers)
        {
            this._selectMembers = selectMembers ?? throw new ArgumentNullException(nameof(selectMembers));
        }

        public IReadOnlyCollection<Type> Resolve(Type actualType)
        {
            return this._selectMembers(actualType);
        }
    }
}