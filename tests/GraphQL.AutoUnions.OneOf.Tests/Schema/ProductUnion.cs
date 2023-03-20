namespace GraphQL.AutoUnions.Tests;

[OneOf.GenerateOneOf]
public partial class ProductUnion : OneOf.OneOfBase<
    Book,
    Electronic
>
{
}