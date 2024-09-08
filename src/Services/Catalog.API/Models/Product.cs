using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver.Core.Operations;

namespace Catalog.API.Models;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } 
    public string Name { get; set; } 
    public string Description { get; set; } 
    public decimal Price { get; set; } 
    public string ImageUrl { get; set; } 
    public string CategoryId { get; set; } 
    public Category Category { get; set; }

    public Product()
    {
        Id = Guid.NewGuid().ToString();
    }
}
