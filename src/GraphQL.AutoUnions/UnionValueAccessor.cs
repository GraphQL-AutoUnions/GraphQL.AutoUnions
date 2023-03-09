namespace GraphQL.AutoUnions
{
    using System;

    internal class UnionValueAccessor<T> : IUnionValueAccessor<T>
    {
        private readonly Func<T, object> _selectValue;

        public UnionValueAccessor(Func<T, object> selectValue)
        {
            this._selectValue = selectValue ?? throw new ArgumentNullException(nameof(selectValue));
        }

        public object Access(T union)
        {
            return this._selectValue(union);
        }
    }
}