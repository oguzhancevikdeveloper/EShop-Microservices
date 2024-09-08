using BuildingBlocks.ControllerBases;
using Catalog.API.DTOs.Category;
using Catalog.API.Services.Category;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : CustomBaseController
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CategoryCreatedDto categoryCreatedDto)
    {
        return CreateActionResultInstance(await _categoryService.CreateAsync(categoryCreatedDto));
    }
    [HttpPut("update")]
    public async Task<IActionResult> Update(CategoryUpdatedDto categoryUpdatedDto)
    {
        return CreateActionResultInstance(await _categoryService.UpdateAsync(categoryUpdatedDto));
    }
    [HttpDelete("delete/{Id}")]
    public async Task<IActionResult> Delete(string Id)
    {
        return CreateActionResultInstance(await _categoryService.DeleteAsync(Id));
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById(string Id)
    {
        return CreateActionResultInstance(await _categoryService.GetByIdAsync(Id));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return CreateActionResultInstance(await _categoryService.GetAllAsync());
    }
}
