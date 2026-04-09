using MediatR;

public record ApplyCouponCommand(int OrderId, string Code) : IRequest<Coupon>;
