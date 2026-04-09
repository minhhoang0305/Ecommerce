using MediatR;

public record UpdateCommand(int Id, string Name, string Category, decimal Price, string Description, int Stock, bool IsDelete) : IRequest<Unit>;
