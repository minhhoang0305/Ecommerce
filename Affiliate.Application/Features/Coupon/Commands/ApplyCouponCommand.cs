using MediatR;

public record ApplyCouponCommand(Guid OrderId, string Code) : IRequest<Coupon>;
