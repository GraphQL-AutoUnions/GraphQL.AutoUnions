namespace GraphQL.AutoUnions.Tests;

using System.Collections.Generic;
using System.Linq;

public class ProductQuery
{
    public static IEnumerable<ProductUnion> Products()
    {
        foreach (var book in ProductData.Books)
        {
            yield return book;
        }

        foreach (var electronic in ProductData.Electronics)
        {
            yield return electronic;
        }
    }

    public static ProductUnion Product(bool book = true) => book ? ProductData.Books.First() : ProductData.Electronics.First();
    public static Book? Book(int bookId) => ProductData.Books.FirstOrDefault(book => book.Id == bookId);
    public static Electronic? Electronic(int electronicId) => ProductData.Electronics.FirstOrDefault(electronic => electronic.Id == electronicId);
}