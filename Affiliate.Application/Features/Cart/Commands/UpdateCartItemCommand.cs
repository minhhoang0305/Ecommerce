using MediatR;

public record UpdateCartItemCommand(int CartItemId, int Quantity, int UserId) : IRequest<Unit>;
