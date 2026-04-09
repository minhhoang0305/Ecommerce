using MediatR;
public record GetByIdAsync(int Id) : IRequest<Products?>;
