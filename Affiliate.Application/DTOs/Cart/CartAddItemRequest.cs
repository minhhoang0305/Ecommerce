namespace Affiliate.Application.DTOs;

public class CartAddItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
