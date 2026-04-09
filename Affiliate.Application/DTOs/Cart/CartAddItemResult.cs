public record CartAddItemResult(
    int CartId,
    int CartItemId,
    int ProductId,
    string ProductName,
    decimal Price,
    int Quantity
);

