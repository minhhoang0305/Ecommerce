using MediatR;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Orders();

        foreach (var item in request.Items)
            order.AddItem(item.ProductId, item.ProductName, item.Price, item.Quantity);

        await _orderRepository.SaveAsync(order);
        return order.Id;
    }
}
