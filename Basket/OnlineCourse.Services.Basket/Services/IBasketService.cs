using OnlineCourse.Services.Basket.Dtos;
using OnlineCourse.Shared.Dtos;

namespace OnlineCourse.Services.Basket.Services
{
    public interface IBasketService
    {
        Task<Response<BasketDto>> GetBasket(string userId);

        Task<Response<bool>> SaveOrUpdate(BasketDto basketDto);

        Task<Response<bool>> Delete(string userId);
    }
}
