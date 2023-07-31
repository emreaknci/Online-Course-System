using OnlineCourse.Web.Models.Discount;

namespace OnlineCourse.Web.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<DiscountViewModel> GetDiscount(string discountCode);

    }
}
