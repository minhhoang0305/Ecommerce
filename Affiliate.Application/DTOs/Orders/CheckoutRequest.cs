namespace Affiliate.Application.DTOs;

public class CheckoutRequest
{
    public string PaymentMethod { get; set; } = default!;
    public string? CouponCode { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
}
