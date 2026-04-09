using Affiliate.Application.DTOs;
using MediatR;
using System.Data;

public class CheckoutHandler : IRequestHandler<CheckoutCommand, OrderDTO>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutHandler(IOrderRepository orderReponsitory, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderReponsitory;
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderDTO> Handle(CheckoutCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        try
        {
            var order = await _orderRepository.CheckoutAsync(
                request.UserId,
                request.PaymentMethod,
                request.CouponCode,
                request.Name,
                request.Address,
                request.PhoneNumber,
                cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return new OrderDTO(
                order.Id,
                order.Items.Select(x => new OrderItemDTO(x.ProductName, x.Price, x.Quantity)).ToList(),
                order.TotalAmount,
                order.Discount,
                order.FinalAmount,
                order.Coupon?.Code,
                order.IsPaid,
                order.Status,
                order.LoyaltyPointsAwarded,
                order.CreatedAt,
                order.CustomerName,
                order.Address,
                order.PhoneNumber);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
