using Affiliate.Application.Features.Checkout.Command;
using Affiliate.Application.Loyalty;
using MediatR;
using Microsoft.Extensions.Options;
using System.Data;

namespace Affiliate.Application.Features.Checkout.Handler;

public class CompleteOrderHandler : IRequestHandler<CompleteOrderCommand, CompleteOrderResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly LoyaltyOptions _options;

    public CompleteOrderHandler(
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IOptions<LoyaltyOptions> options)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _options = options.Value ?? new LoyaltyOptions();
    }

    public async Task<CompleteOrderResult> Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        if (order is null)
            throw new ArgumentException("Order not found");

        var user = await _userRepository.GetByIdAsync(order.UserId);
        if (user is null)
            throw new ArgumentException("User not found");

        if (string.Equals(order.Status, Orders.StatusCompleted, StringComparison.OrdinalIgnoreCase))
        {
            // Idempotent: return existing state.
            return new CompleteOrderResult(order.Id, order.LoyaltyPointsAwarded, user.LoyaltyPoints, user.MemberRank);
        }

        if (!order.IsPaid)
            throw new InvalidOperationException("Only paid orders can be completed.");

        var vndPerPoint = _options.VndPerPoint <= 0 ? 100_000m : _options.VndPerPoint;
        var pointsAwarded = (int)Math.Floor(order.FinalAmount / vndPerPoint);
        if (pointsAwarded < 0) pointsAwarded = 0;

        await _unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
        try
        {
            order.MarkAsCompleted(pointsAwarded);

            user.LoyaltyPoints += pointsAwarded;
            user.MemberRank = LoyaltyRanker.GetRank(user.LoyaltyPoints, _options);

            await _orderRepository.UpdateAsync(order);
            await _userRepository.UpdateAsync(user);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }

        return new CompleteOrderResult(order.Id, pointsAwarded, user.LoyaltyPoints, user.MemberRank);
    }
}

