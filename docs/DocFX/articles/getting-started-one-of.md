# Getting started with `GraphQL.AutoUnions.OneOf`

Install the latest version of the nuget.

```powershell
dotnet add package GraphQL.AutoUnions.OneOf
```

Add the library to the GraphqQl builder.

```csharp
services.AddGraphQL(options =>
{
    options.AddOneOfAutoUnions(); // <---
    options.AddSystemTextJson();
    options.AddErrorInfoProvider((error) =>
    {
        error.ExposeExceptionDetails = true;
    });
    // register auto schemas
    gqlBuilder.AddAutoSchema<ProductQuery>();
});
```

Create union types, ideally with `[GenerateOneOf]` attributes.

```csharp
[GenerateOneOf]
public partial class ProductUnion : OneOfBase<Book, Electronic>
{
}
```

Use the union inside a auto schema endpoint. (Currently we do not support union arguments.)

```csharp
public class ProductQuery
{
    public static IEnumerable<ProductUnion> Products([FromServices] IProductService productService)
    {
        return productService.GetProducts();
    }
}
```