using Basket.API.DTOs;
using BuildingBlocks.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Basket.API.Services;

public interface IBasketService
{
    Task<Response<BasketDto>> GetBasket(string userId);

    Task<Response<NoContent>> SaveOrUpdate(BasketDto basketDto);

    Task<Response<NoContent>> Delete(string userId);
}
