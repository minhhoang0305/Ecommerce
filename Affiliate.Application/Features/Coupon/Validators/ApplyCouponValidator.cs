using FluentValidation;

public class ApplyCouponValidator : AbstractValidator<ApplyCouponCommand>
{
    public ApplyCouponValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
    }
}
