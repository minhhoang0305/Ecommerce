using MediatR;

namespace Affiliate.Application.Features.Checkout.Command;

public record CompleteOrderCommand(int OrderId) : IRequest<CompleteOrderResult>;

public record CompleteOrderResult(
    int OrderId,
    int PointsAwarded,
    int UserTotalPoints,
    string UserRank);
