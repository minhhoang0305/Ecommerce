using Affiliate.Application.DTOs;
using MediatR;

public record GetUserOrdersQuery(int UserId) : IRequest<IReadOnlyList<OrderDTO>>;
