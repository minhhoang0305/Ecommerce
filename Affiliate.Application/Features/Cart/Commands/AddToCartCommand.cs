using MediatR;

public record AddToCartCommand(int ProductId, int Quantity, int UserId) : IRequest<CartAddItemResult>;
