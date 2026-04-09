using MediatR;

public record ApplyCouponToCartCommand(int UserId, string Code) : IRequest<CartCouponResult>;
