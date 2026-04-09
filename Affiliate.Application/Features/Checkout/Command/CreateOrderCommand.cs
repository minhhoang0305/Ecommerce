using MediatR;

public record CreateOrderCommand(List<(int ProductId, string ProductName, decimal Price, int Quantity)> Items) : IRequest<int>;
