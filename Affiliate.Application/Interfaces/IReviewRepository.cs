public interface IReviewRepository
{
    Task AddAsync(Review review);
    Task<bool> ExistAsync(int UserId, int ProductId);
    Task<IReadOnlyList<Review>> GetByProductIdAsync(int productId, int take = 20);
}
