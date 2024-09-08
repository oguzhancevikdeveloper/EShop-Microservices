using BuildingBlocks.DTOs;
using Catalog.API.DTOs.Product;
using Catalog.API.Settings;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;
using MassTransit;
using BuildingBlocks.Messaging.Messages;

namespace Catalog.API.Services.Product;

public class ProductService : IProductService
{
    private readonly IMongoCollection<Models.Product> _productCollection;
    private readonly IMongoCollection<Models.Category> _categoryCollection;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public ProductService(IDatabaseSettings databaseSettings, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _productCollection = database.GetCollection<Models.Product>(databaseSettings.ProductCollectionName);
        _categoryCollection = database.GetCollection<Models.Category>(databaseSettings.CategoryCollectionName);
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<BuildingBlocks.DTOs.Response<ProductCreatedDto>> CreateProduct(ProductCreatedDto productCreatedDto)
    {
        var product = _mapper.Map<Models.Product>(productCreatedDto);
        product.Id = Convert.ToString(Guid.NewGuid());
        product.Category = await _categoryCollection.Find(x => x.Id == productCreatedDto.CategoryId).FirstAsync();
        await _productCollection.InsertOneAsync(product);

        return BuildingBlocks.DTOs.Response<ProductCreatedDto>.Success(_mapper.Map<ProductCreatedDto>(product), StatusCodes.Status201Created);
    }

    public async Task<BuildingBlocks.DTOs.Response<NoContent>> DeleteProduct(string id)
    {
        var result = await _productCollection.DeleteOneAsync(x => x.Id == id);
        if (result.DeletedCount > 0) return BuildingBlocks.DTOs.Response<NoContent>.Success(StatusCodes.Status200OK);
        else return BuildingBlocks.DTOs.Response<NoContent>.Fail("Product not a found!", StatusCodes.Status404NotFound, true);

    }

    public async Task<BuildingBlocks.DTOs.Response<ProductDto>> GetByIdAsync(string id)
    {
        var product = await _productCollection.Find<Models.Product>(x => x.Id.Equals(id)).FirstAsync();

        if (product is not null)
        {
            product.Category = await _categoryCollection.Find<Models.Category>(x => x.Id == product.CategoryId).FirstAsync();
            return BuildingBlocks.DTOs.Response<ProductDto>.Success(_mapper.Map<ProductDto>(product), StatusCodes.Status200OK);
        }

        return BuildingBlocks.DTOs.Response<ProductDto>.Fail("Product not a found", StatusCodes.Status404NotFound, true);

    }

    public async Task<BuildingBlocks.DTOs.Response<ProductUpdatedDto>> UpdateProduct(ProductUpdatedDto productUpdatedDto)
    {

        var updatedProduct = _mapper.Map<Models.Product>(productUpdatedDto);

        updatedProduct.Category = await _categoryCollection.Find<Models.Category>(x => x.Id == productUpdatedDto.CategoryId).FirstAsync();

        await _productCollection.FindOneAndReplaceAsync(x => x.Id == productUpdatedDto.Id, updatedProduct);

        await _publishEndpoint.Publish<ProductNameChangedMessage>(new ProductNameChangedMessage
        {
            ProductId = productUpdatedDto.Id,
            UpdatedName = updatedProduct.Name
        });

        return BuildingBlocks.DTOs.Response<ProductUpdatedDto>.Success(_mapper.Map<ProductUpdatedDto>(updatedProduct), StatusCodes.Status200OK);
    }


}
