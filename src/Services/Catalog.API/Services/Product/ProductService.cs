using BuildingBlocks.DTOs;
using Catalog.API.DTOs.Product;
using Catalog.API.Settings;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;

namespace Catalog.API.Services.Product;

public class ProductService : IProductService
{
    private readonly IMongoCollection<Models.Product> _productCollection;
    private readonly IMongoCollection<Models.Category> _categoryCollection;
    private readonly IMapper _mapper;

    public ProductService(IDatabaseSettings databaseSettings, IMapper mapper)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _productCollection = database.GetCollection<Models.Product>(databaseSettings.ProductCollectionName);
        _categoryCollection = database.GetCollection<Models.Category>(databaseSettings.CategoryCollectionName);
        _mapper = mapper;
    }

    public async Task<Response<ProductCreatedDto>> CreateProduct(ProductCreatedDto productCreatedDto)
    {
        var product = _mapper.Map<Models.Product>(productCreatedDto);
        product.Id = Convert.ToString(Guid.NewGuid());
        product.Category = await _categoryCollection.Find(x => x.Id == productCreatedDto.CategoryId).FirstAsync();
        await _productCollection.InsertOneAsync(product);

        return Response<ProductCreatedDto>.Success(_mapper.Map<ProductCreatedDto>(product), StatusCodes.Status201Created);
    }

    public async Task<Response<NoContent>> DeleteProduct(string id)
    {
        var result = await _productCollection.DeleteOneAsync(x => x.Id == id);
        if (result.DeletedCount > 0) return Response<NoContent>.Success(StatusCodes.Status200OK);
        else return Response<NoContent>.Fail("Product not a found!", StatusCodes.Status404NotFound, true);

    }

    public async Task<Response<ProductDto>> GetByIdAsync(string id)
    {
        var product = await _productCollection.Find<Models.Product>(x => x.Id.Equals(id)).FirstAsync();

        if (product is not null)
        {
            product.Category = await _categoryCollection.Find<Models.Category>( x => x.Id == product.CategoryId).FirstAsync();
            return Response<ProductDto>.Success(_mapper.Map<ProductDto>(product), StatusCodes.Status200OK);
        }

        return Response<ProductDto>.Fail("Product not a found", StatusCodes.Status404NotFound, true);

    }

    public async Task<Response<ProductUpdatedDto>> UpdateProduct(ProductUpdatedDto productUpdatedDto)
    {

        var updatedProduct = _mapper.Map<Models.Product>(productUpdatedDto);

        updatedProduct.Category = await _categoryCollection.Find<Models.Category>(x => x.Id == productUpdatedDto.CategoryId).FirstAsync();

        await _productCollection.FindOneAndReplaceAsync(x => x.Id == productUpdatedDto.Id, updatedProduct);

        return Response<ProductUpdatedDto>.Success(_mapper.Map<ProductUpdatedDto>(updatedProduct), StatusCodes.Status200OK);
    }


}
