using Catalog.API.Models;

namespace Catalog.API.DTOs.Category;

public class CategoryUpdatedDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
