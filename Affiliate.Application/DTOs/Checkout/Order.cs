namespace Affiliate.Application.DTOs;

public record OrderDTO(
    Guid Id,
    List<OrderItemDTO> Items,
    decimal TotalAmount,
    decimal Discount,
    decimal FinalAmount,
    string? CouponCode,
    bool IsPaid,
    DateTime CreatedAt);
