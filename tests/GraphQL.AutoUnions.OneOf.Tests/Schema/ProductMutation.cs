namespace GraphQL.AutoUnions.Tests;

using System.Linq;

public class ProductMutation
{
    public static ProductUnion AddProduct(bool book = true) => book ? ProductData.Books.First() : ProductData.Electronics.First();
}