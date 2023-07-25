using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Discount.Services;
using OnlineCourse.Shared.Services;
using System.Reflection;
using OnlineCourse.Shared.ControllerBase;

namespace OnlineCourse.Services.Discount.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiscountsController : CustomControllerBase
{
    private readonly IDiscountService _discountService;

    private readonly ISharedIdentityService _sharedIdentityService;

    public DiscountsController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
    {
        _discountService = discountService;
        _sharedIdentityService = sharedIdentityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return CreateActionResultInstance(await _discountService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var discount = await _discountService.GetById(id);

        return CreateActionResultInstance(discount);
    }

    [HttpGet("GetByCode/{code}")]
    public async Task<IActionResult> GetByCode(string code)

    {
        var userId = _sharedIdentityService.GetUserId;

        var discount = await _discountService.GetByCodeAndUserId(code, userId);

        return CreateActionResultInstance(discount);
    }

    [HttpPost]
    public async Task<IActionResult> Save(OnlineCourse.Discount.Models.Discount discount)
    {
        return CreateActionResultInstance(await _discountService.Save(discount));
    }

    [HttpPut]
    public async Task<IActionResult> Update(OnlineCourse.Discount.Models.Discount discount)
    {
        return CreateActionResultInstance(await _discountService.Update(discount));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return CreateActionResultInstance(await _discountService.Delete(id));
    }
}


