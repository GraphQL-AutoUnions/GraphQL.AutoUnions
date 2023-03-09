namespace GraphQL.AutoUnions
{
    using System;

    internal class IsTypeOfFactory<TUnion> : IIsTypeOfFactory<TUnion>
    {
        private readonly IUnionValueAccessor<TUnion> _unionValueAccessor;
        private readonly IUnionCast<TUnion> _unionCast;

        public IsTypeOfFactory(
            IUnionValueAccessor<TUnion> unionValueAccessor,
            IUnionCast<TUnion> unionCast)
        {
            this._unionValueAccessor = unionValueAccessor;
            this._unionCast = unionCast;
        }
        
        public Func<object, bool> Create(Func<object, bool> previous)
        {
            return (result) => (this._unionCast.TryCast(result, out var union)
                ? previous?.Invoke(this._unionValueAccessor.Access(union))
                : previous?.Invoke(result)) ?? false;
        }
    }
}