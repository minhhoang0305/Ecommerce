using MediatR;
public class AddToCartHandler : IRequestHandler<AddToCartCommand, CartAddItemResult>
{
    private readonly IProductRepository _productRepo;
    private readonly ICartRepository _cartRepo;

    public AddToCartHandler(IProductRepository productRepo, ICartRepository cartRepo)
    {
        _productRepo = productRepo;
        _cartRepo = cartRepo;
    }

    public async Task<CartAddItemResult> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepo.GetByIdAsync(request.ProductId);

        if (product == null || product.IsDeleted)
            throw new Exception("Product not found");

        if (product.Stock < request.Quantity)
            throw new Exception("Not enough stock");

        var cart = await _cartRepo.GetByUserIdAsync(request.UserId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = request.UserId,
                Items = new List<CartItem>()
            };
        }

        var existingItem = cart.Items
            .FirstOrDefault(x => x.ProductId == request.ProductId);

        if (existingItem != null)
        {
            if (product.Stock < existingItem.Quantity + request.Quantity)
                throw new Exception("Not enough stock");

            existingItem.Quantity += request.Quantity;
            existingItem.Price = product.Price;
            existingItem.Name = product.Name;
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                ProductId = product.Id,
                Quantity = request.Quantity,
                Name = product.Name,
                Price = product.Price
            });
        }

        await _cartRepo.SaveAsync(cart);

        var affectedItem = cart.Items.FirstOrDefault(x => x.ProductId == product.Id);
        if (affectedItem == null)
            throw new Exception("Cart item not found after save");

        // If the cart was rehydrated inside repository logic, the Id might not be on the original instance.
        if (affectedItem.Id == 0)
        {
            var reloaded = await _cartRepo.GetByUserIdAsync(request.UserId);
            affectedItem = reloaded?.Items.FirstOrDefault(x => x.ProductId == product.Id);
            if (reloaded != null) cart = reloaded;
            if (affectedItem == null)
                throw new Exception("Cart item not found after reload");
        }

        return new CartAddItemResult(
            cart.Id,
            affectedItem.Id,
            affectedItem.ProductId,
            affectedItem.Name,
            affectedItem.Price,
            affectedItem.Quantity);
    }
}
