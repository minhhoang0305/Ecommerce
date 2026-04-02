using MediatR;

namespace Affiliate.Application.Features.Checkout.Command;

public record CompleteOrderCommand(Guid OrderId) : IRequest<CompleteOrderResult>;

public record CompleteOrderResult(
    Guid OrderId,
    int PointsAwarded,
    int UserTotalPoints,
    string UserRank);

