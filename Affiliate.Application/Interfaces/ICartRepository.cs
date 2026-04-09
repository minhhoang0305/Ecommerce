public interface ICartRepository
{
    Task<Cart?> GetByUserIdAsync(int userId);
    Task SaveAsync(Cart cart);
    Task ClearAsync(Cart cart);
}
