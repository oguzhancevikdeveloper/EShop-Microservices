using BuildingBlocks.DTOs;
using Catalog.API.DTOs.Category;
using Catalog.API.Settings;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;
namespace Catalog.API.Services.Category;

public class CategoryService : ICategoryService
{
    private readonly IMongoCollection<Models.Category> _categoryCollection;
    private readonly IMongoCollection<Models.Product> _productCollection;
    private readonly IMapper _mapper;

    public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _categoryCollection = database.GetCollection<Models.Category>(databaseSettings.CategoryCollectionName);
        _productCollection = database.GetCollection<Models.Product>(databaseSettings.ProductCollectionName);
        _mapper = mapper;
    }
    public async Task<Response<CategoryCreatedDto>> CreateAsync(CategoryCreatedDto category)
    {
        var createdCategory = _mapper.Map<Models.Category>(category);
        await _categoryCollection.InsertOneAsync(createdCategory);
        return Response<CategoryCreatedDto>.Success(_mapper.Map<CategoryCreatedDto>(category), StatusCodes.Status201Created);
    }

    public async Task<Response<NoContent>> DeleteAsync(string id)
    {
        var result = await _categoryCollection.DeleteOneAsync(x => x.Id == id);
        if (result.DeletedCount > 0) return Response<NoContent>.Success(StatusCodes.Status200OK);
        else return Response<NoContent>.Fail("Category not a found!", StatusCodes.Status404NotFound, true);

    }

    public async Task<Response<List<CategoryDto>>> GetAllAsync()
    {
        List<Models.Category> categories = await _categoryCollection.Find(category => true).ToListAsync();
        return Response<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories), StatusCodes.Status200OK);
    }

    public async Task<Response<CategoryDto>> GetByIdAsync(string id)
    {
        var category = await _categoryCollection.Find<Models.Category>(x => x.Id == id).FirstAsync();
        if (category == null) return Response<CategoryDto>.Fail("Category not found!", StatusCodes.Status404NotFound, true);

        return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), StatusCodes.Status200OK);
    }

    public async Task<Response<CategoryUpdatedDto>> UpdateAsync(CategoryUpdatedDto updatedCategoryDto)
    {
        Models.Category updatedCategory = _mapper.Map<Models.Category>(updatedCategoryDto);

        var result = await _categoryCollection.FindOneAndReplaceAsync(x => x.Id == updatedCategory.Id, updatedCategory);


        if (result == null) return Response<CategoryUpdatedDto>.Fail("Course not a found", StatusCodes.Status404NotFound, true);

        else
        {
            var products = await _productCollection.Find<Models.Product>(x => x.CategoryId == updatedCategory.Id).ToListAsync();

            if (products.Any())
            {
                foreach (var item in products)
                {
                    item.Category.Name = updatedCategory.Name;
                    item.Category.Description = updatedCategory.Description;
                    await _productCollection.FindOneAndReplaceAsync(x => x.CategoryId == updatedCategory.Id, item);
                }


            }

            return Response<CategoryUpdatedDto>.Success(StatusCodes.Status200OK);
        }

    }

}
