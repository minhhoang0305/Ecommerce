using MediatR;

public record AddToCartCommand(Guid UserId,Guid ProductId, string Name,decimal Price, int Quantity) : IRequest<Unit>;