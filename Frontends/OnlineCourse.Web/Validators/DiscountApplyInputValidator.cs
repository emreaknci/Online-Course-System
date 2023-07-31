using FluentValidation;
using OnlineCourse.Web.Models.Discount;

namespace OnlineCourse.Web.Validators;

public class DiscountApplyInputValidator : AbstractValidator<DiscountApplyInput>
{
    public DiscountApplyInputValidator()
    {
        RuleFor(x => x.Code).NotEmpty().WithMessage("indirim kupon alanı boş olamaz");
    }
}