* * *

GraphQL.AutoUnions.OneOf
========================

A library that adds support for auto schema with discriminated unions when using the [OneOf](https://github.com/mcintyre321/OneOf) library.

Problem
-----

GraphQL is an awesome technology for building APIs, but it can be tricky to define complex types with unions and interfaces, especially when you're using .NET Core. It can be difficult to create a schema that's easy to understand, and it often requires a lot of boilerplate code.

Our library is here to help! We provide an easy way to define discriminated unions in your GraphQL schema using the popular [OneOf](https://github.com/mcintyre321/OneOf) library. This library is specifically designed to be used with `AddAutoSchema` from the [GraphQL](https://github.com/graphql-dotnet/graphql-dotnet) package.

With our library, you can define your discriminated unions in C# using the OneOf library, and then automatically generate the corresponding GraphQL schema and resolvers using `AddAutoSchema` from the `GraphQL` package. Our goal is to make it easy for developers to work with discriminated unions in their GraphQL APIs, without having to worry about the details. Whether you're building a new project or adding features to an existing one, our library can help you save time and effort.

Usage
-----

To use this library, you first need to install it using NuGet:

```bash
dotnet add package GraphQL.AutoUnions.OneOf
```

Once you have installed the package, you can use it in your .NET Core project. Here's an example of how to use this library:

```csharp
    using GraphQL;
    
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGraphQL((gqlBuilder) =>
            {
                gqlBuilder.AddOneOfAutoUnions();
                gqlBuilder.AddAutoSchema<ProductQuery>();
            });
        }
    
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGraphQL();
        }
    }
    
    public class ProductQuery
    {
        public static IEnumerable<ProductUnion> Products([FromServices] IProductService productService)
        {
            return productService.GetProducts();
        }
    }
    
    [GenerateOneOf]
    public partial class ProductUnion : OneOfBase<Book, Electronic>
    {
    }
    
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int PageCount { get; set; }
    }
    
    public class Electronic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
    }
    
    public interface IProductService
    {
        IEnumerable<ProductUnion> GetProducts();
    }
```
This code will generate the following schema:

```graphql
type Book {
    id: Int!
    name: String!
    author: String!
    pageCount: Int!
}

type Electronic {
    id: Int!
    name: String!
    price: Float!
    description: String!
}

union ProductUnion = Book | Electronic

type Query {
  products: [ProductUnion!]!
}
```

License
-------

This library is released under the Apache 2 License.