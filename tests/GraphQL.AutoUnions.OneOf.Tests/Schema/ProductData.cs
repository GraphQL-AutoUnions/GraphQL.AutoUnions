namespace GraphQL.AutoUnions.Tests;

public abstract class ProductData
{
    [Ignore] internal static readonly Book[] Books = new[]
    {
        new Book() {Id = 1, Name = "The Great Gatsby", Author = "F. Scott Fitzgerald", PageCount = 180},
        new Book() {Id = 2, Name = "To Kill a Mockingbird", Author = "Harper Lee", PageCount = 324},
        new Book() {Id = 3, Name = "The Catcher in the Rye", Author = "J.D. Salinger", PageCount = 224},
        new Book() {Id = 4, Name = "1984", Author = "George Orwell", PageCount = 328},
        new Book() {Id = 5, Name = "Pride and Prejudice", Author = "Jane Austen", PageCount = 432},
    };

    [Ignore] internal static readonly Electronic[] Electronics = new[]
    {
        new Electronic() { Id = 1, Name = "Sony Bravia TV", Price = 899.99f, Description = "55-inch smart LED TV with 4K resolution" },
        new Electronic() { Id = 2, Name = "Apple MacBook Pro", Price = 1499.99f, Description = "13-inch laptop with Retina display and 512GB storage" },
        new Electronic() { Id = 3, Name = "Samsung Galaxy S21", Price = 799.99f, Description = "5G smartphone with 128GB storage and triple rear camera" },
        new Electronic() { Id = 4, Name = "Amazon Echo Dot", Price = 49.99f, Description = "Smart speaker with Alexa and voice control" },
        new Electronic() { Id = 5, Name = "DJI Mavic Air 2", Price = 799.99f, Description = "4K drone with 48MP camera and 34-minute flight time" },
    };
}