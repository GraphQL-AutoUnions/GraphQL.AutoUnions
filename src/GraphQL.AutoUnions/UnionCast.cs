namespace GraphQL.AutoUnions
{
    internal class UnionCast<T> : IUnionCast<T>
    {
        public bool TryCast(object obj, out T union)
        {
            if (obj is T matching)
            {
                union = matching;

                return true;
            }

            union = default;
            return false;
        }
    }
}