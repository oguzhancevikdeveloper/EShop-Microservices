using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Models;

public class Category
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public Category()
    {
        Id = Guid.NewGuid().ToString();
    }

}