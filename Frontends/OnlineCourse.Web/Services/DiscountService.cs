using OnlineCourse.Shared.Dtos;
using OnlineCourse.Web.Models.Discount;
using OnlineCourse.Web.Services.Interfaces;

namespace OnlineCourse.Web.Services;
public class DiscountService : IDiscountService
{
    private readonly HttpClient _httpClient;

    public DiscountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DiscountViewModel> GetDiscount(string discountCode)
    {

        var response = await _httpClient.GetAsync($"discounts/GetByCode/{discountCode}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var discount = await response.Content.ReadFromJsonAsync<Response<DiscountViewModel>>();

        return discount.Data;
    }
}
