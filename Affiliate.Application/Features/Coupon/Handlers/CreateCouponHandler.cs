using MediatR;

public class CreateCouponHandler : IRequestHandler<CreateCouponCommand, Guid>
{
    private readonly ICouponRepository _couponRepository;

    public CreateCouponHandler(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository;
    }

    public async Task<Guid> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        var existingCoupon = await _couponRepository.GetByCodeAsync(request.Code);
        if (existingCoupon != null)
            throw new ArgumentException("Coupon code already exists");

        var coupon = new Coupon
        {
            Id = Guid.NewGuid(),
            Code = request.Code.Trim(),
            DiscountPercent = request.DiscountPercent,
            StartDate = DateTime.UtcNow,
            EndDate = request.EndDate,
            IsActive = request.IsActive,
            UsageLimit = request.UsageLimit,
            TimesUsed = 0
        };

        return await _couponRepository.CreateAsync(coupon);
    }
}
