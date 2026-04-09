using Affiliate.Application.DTOs;
using MediatR;

public record CheckoutCommand(
    int UserId,
    string PaymentMethod,
    string? CouponCode,
    string? Name,
    string? Address,
    string? PhoneNumber) : IRequest<OrderDTO>;
