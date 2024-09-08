using Basket.API.DTOs;
using BuildingBlocks.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;

namespace Basket.API.Services;

public class BasketService : IBasketService
{
    private readonly RedisService _redisService;

    public BasketService(RedisService redisService)
    {
        _redisService = redisService;
    }

    public async Task<Response<NoContent>> Delete(string userId)
    {
        var status = await _redisService.GetDb().KeyDeleteAsync(userId);
        return status ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("Basket not found", 404, true);
    }

    public async Task<Response<BasketDto>> GetBasket(string userId)
    {
        var existBasket = await _redisService.GetDb().StringGetAsync(userId);
        if (String.IsNullOrEmpty(existBasket)) return Response<BasketDto>.Fail("Basket not found", 404, true);
        return Response<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(existBasket), 200);
    }

    public async Task<Response<NoContent>> SaveOrUpdate(BasketDto basketDto)
    {
        var status = await _redisService.GetDb().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));

        return status ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("Basket could not update or save", 500,true);
    }
}
