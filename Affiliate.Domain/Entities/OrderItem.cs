public class OrderItems
{
    public int Id { get; private set; }
    public int OrderId { get; private set; }
    public string ProductName { get; private set; } = default!;
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
    public Orders Order { get; private set; } = null!;
    public int ProductId { get; private set; }

    private OrderItems()
    {
    }

    public OrderItems(int productId, string productName, decimal price, int quantity)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name is required", nameof(productName));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

        ProductId = productId;
        ProductName = productName;
        Price = price;
        Quantity = quantity;
    }
}
