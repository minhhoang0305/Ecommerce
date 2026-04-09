using MediatR;

public record CartCommand(int UserId) : IRequest<Cart?>;
