using MediatR;

public record CreateCouponCommand(
    string Code,
    decimal DiscountPercent,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive,
    int UsageLimit) : IRequest<Guid>;
