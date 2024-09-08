using BuildingBlocks.DTOs;
using Catalog.API.DTOs.Category;
using Catalog.API.DTOs.Product;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.API.Services.Product;

public interface IProductService
{
    Task<Response<ProductCreatedDto>> CreateProduct(ProductCreatedDto productCreatedDto);
    Task<Response<ProductUpdatedDto>> UpdateProduct(ProductUpdatedDto productUpdatedDto);
    Task<Response<NoContent>> DeleteProduct(string id);
    Task<Response<ProductDto>> GetByIdAsync(string id);

}
