using BuildingBlocks.DTOs;
using Catalog.API.DTOs.Category;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.API.Services.Category;

public interface ICategoryService
{
    Task<Response<CategoryCreatedDto>> CreateAsync(CategoryCreatedDto category);
    Task<Response<CategoryUpdatedDto>> UpdateAsync(CategoryUpdatedDto category);
    Task<Response<NoContent>> DeleteAsync(string id);
    Task<Response<List<CategoryDto>>> GetAllAsync();
    Task<Response<CategoryDto>> GetByIdAsync(string id);
}
