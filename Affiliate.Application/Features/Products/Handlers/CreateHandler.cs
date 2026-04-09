using MediatR;

public class CreateHandler : IRequestHandler<CreateCommand, int>
{
    private readonly IProductRepository _productRepository;

    public CreateHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<int> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        var product = new Products
        {
            Name = request.Name,
            Category = request.Category,
            Price = request.Price,
            Description = request.Description,
            Stock = request.Stock,
        };

        return await _productRepository.CreateAsync(product);
    }
}
