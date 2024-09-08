﻿namespace Catalog.API.DTOs.Product;

public class ProductDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public string CategoryId { get; set; }
    public Models.Category Category { get; set; }

}
