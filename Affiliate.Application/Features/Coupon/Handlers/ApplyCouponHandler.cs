using MediatR;

public class ApplyCouponHandler : IRequestHandler<ApplyCouponCommand, Coupon>
{
    private readonly IOrderRepository _orderRepo;
    private readonly ICouponRepository _couponRepo;

    public ApplyCouponHandler(IOrderRepository orderRepo, ICouponRepository couponRepo)
    {
        _orderRepo = orderRepo;
        _couponRepo = couponRepo;
    }

    public async Task<Coupon> Handle(ApplyCouponCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepo.GetByIdAsync(request.OrderId);
        if (order == null)
            throw new ArgumentException("Order not found");

        var coupon = await _couponRepo.GetByCodeAsync(request.Code);
        if (coupon == null)
            throw new ArgumentException("Coupon not found");

        order.ApplyCoupon(coupon);
        coupon.TimesUsed++;

        await _couponRepo.UpdateAsync(coupon);
        await _orderRepo.UpdateAsync(order);

        return coupon;
    }
}
