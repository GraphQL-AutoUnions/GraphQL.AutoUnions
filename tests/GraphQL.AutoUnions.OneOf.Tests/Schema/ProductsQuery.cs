namespace GraphQL.AutoUnions.Tests;

using System.Collections.Generic;

public class ProductQuery
{
    public static IEnumerable<ProductUnion> Products()
    {
        yield return new Book() { Id = 1, Name = "The Great Gatsby", Author = "F. Scott Fitzgerald", PageCount = 180 };
        yield return new Book() { Id = 2, Name = "To Kill a Mockingbird", Author = "Harper Lee", PageCount = 324 };
        yield return new Book() { Id = 3, Name = "The Catcher in the Rye", Author = "J.D. Salinger", PageCount = 224 };
        yield return new Book() { Id = 4, Name = "1984", Author = "George Orwell", PageCount = 328 };
        yield return new Book() { Id = 5, Name = "Pride and Prejudice", Author = "Jane Austen", PageCount = 432 };
        yield return new Electronic() { Id = 1, Name = "Sony Bravia TV", Price = 899.99f, Description = "55-inch smart LED TV with 4K resolution" };
        yield return new Electronic() { Id = 2, Name = "Apple MacBook Pro", Price = 1499.99f, Description = "13-inch laptop with Retina display and 512GB storage" };
        yield return new Electronic() { Id = 3, Name = "Samsung Galaxy S21", Price = 799.99f, Description = "5G smartphone with 128GB storage and triple rear camera" };
        yield return new Electronic() { Id = 4, Name = "Amazon Echo Dot", Price = 49.99f, Description = "Smart speaker with Alexa and voice control" };
        yield return new Electronic() { Id = 5, Name = "DJI Mavic Air 2", Price = 799.99f, Description = "4K drone with 48MP camera and 34-minute flight time" };
    }

    public static ProductUnion Product() => new Book()
        {Id = 1, Name = "The Great Gatsby", Author = "F. Scott Fitzgerald", PageCount = 180};
}
    
[OneOf.GenerateOneOf]
public partial class ProductUnion : OneOf.OneOfBase<Book, Electronic>
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
