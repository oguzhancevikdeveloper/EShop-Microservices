using BuildingBlocks.ControllerBases;
using Catalog.API.DTOs.Product;
using Catalog.API.Services.Product;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : CustomBaseController
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    [HttpPost("create")]
    public async Task<IActionResult> Create(ProductCreatedDto productCreatedDto)
    {
        return CreateActionResultInstance(await _productService.CreateProduct(productCreatedDto));
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(ProductUpdatedDto productUpdatedDto)
    {
        return CreateActionResultInstance(await _productService.UpdateProduct(productUpdatedDto));
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(string Id)
    {
        return CreateActionResultInstance(await _productService.DeleteProduct(Id));
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById(string Id)
    {
        return CreateActionResultInstance(await _productService.GetByIdAsync(Id));
    }

}
