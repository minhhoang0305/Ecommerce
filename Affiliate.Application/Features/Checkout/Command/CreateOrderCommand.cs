using MediatR;

public record CreateOrderCommand(List<(Guid ProductId, string ProductName, decimal Price, int Quantity)> Items) : IRequest<Guid>;
