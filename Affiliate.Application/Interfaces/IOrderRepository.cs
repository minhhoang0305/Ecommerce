using Affiliate.Domain.Entities;

public interface IOrderRepository
{
    Task<Orders?> GetByIdAsync(int id);
    Task<IReadOnlyList<Orders>> GetByUserIdAsync(int userId);
    Task<bool> HasUserPurchasedProductAsync(int userId, int productId);
    Task<Orders> CheckoutAsync(int userId, string paymentMethod, string? couponCode, string? Name, string? address, string? phoneNumber, CancellationToken cancellationToken);
    Task<Orders> CreatePendingVnPayOrderAsync(int userId, string? couponCode, string? Name, string? address, string? phoneNumber, CancellationToken cancellationToken);
    Task FinalizePendingVnPayOrderAsync(int orderId, CancellationToken cancellationToken);
    Task DeletePendingOrderAsync(int orderId, CancellationToken cancellationToken);
    Task UpdateAsync(Orders order);
    Task SaveAsync(Orders order);
}
