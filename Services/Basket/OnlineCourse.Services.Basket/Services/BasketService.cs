using System.Text.Json;
using OnlineCourse.Services.Basket.Dtos;
using OnlineCourse.Shared.Dtos;

namespace OnlineCourse.Services.Basket.Services;

public class BasketService : IBasketService
{
    private readonly RedisService _redisService;

    public BasketService(RedisService redisService)
    {
        _redisService = redisService;
    }

    public async Task<Response<bool>> Delete(string userId)
    {
        var status = await _redisService.GetDb().KeyDeleteAsync(userId);
        return status ? Response<bool>.Success(204) : Response<bool>.Fail("Basket not found", 404);
    }

    public async Task<Response<BasketDto>> GetBasket(string userId)
    {
        var existBasket = await _redisService.GetDb().StringGetAsync(userId);

        return String.IsNullOrEmpty(existBasket)
            ? Response<BasketDto>
                .Fail("Basket not found", 404)
            : Response<BasketDto>
                .Success(200, JsonSerializer.Deserialize<BasketDto>(existBasket));
    }

    public async Task<Response<bool>> SaveOrUpdate(BasketDto basketDto)
    {
        var status = await _redisService.GetDb().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));

        return status 
            ? Response<bool>.Success(204) 
            : Response<bool>.Fail("Basket could not update or save", 500);
    }
}