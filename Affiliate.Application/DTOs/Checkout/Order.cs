namespace Affiliate.Application.DTOs;

public record OrderDTO(
    int Id,
    List<OrderItemDTO> Items,
    decimal TotalAmount,
    decimal Discount,
    decimal FinalAmount,
    string? CouponCode,
    bool IsPaid,
    string Status,
    int LoyaltyPointsAwarded,
    DateTime CreatedAt,
    string? Name,
    string? Address,
    string? PhoneNumber);
