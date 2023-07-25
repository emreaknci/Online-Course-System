using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Services.Basket.Dtos;
using OnlineCourse.Services.Basket.Services;
using OnlineCourse.Shared.ControllerBase;
using OnlineCourse.Shared.Services;

namespace OnlineCourse.Services.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : CustomControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService  _sharedIdentityService;

        public BasketsController(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            var userId = _sharedIdentityService.GetUserId;
            var basket = await _basketService.GetBasket(userId);
            return CreateActionResultInstance(basket);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateBasket(BasketDto basketDto)
        {
            basketDto.UserId = _sharedIdentityService.GetUserId;
            var response = await _basketService.SaveOrUpdate(basketDto);
            return CreateActionResultInstance(response);
        }   

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            var userId = _sharedIdentityService.GetUserId;
            var basket = await _basketService.Delete(userId);
            return CreateActionResultInstance(basket);
        }
    }
}
