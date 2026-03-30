using FluentValidation;

public class CreateCouponValidator : AbstractValidator<CreateCouponCommand>
{
    public CreateCouponValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.DiscountPercent)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate);

        RuleFor(x => x.UsageLimit)
            .GreaterThanOrEqualTo(0);
    }
}
