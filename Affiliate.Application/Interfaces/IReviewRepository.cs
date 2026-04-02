public interface IReviewRepository
{
    Task AddAsync(Review review);
    Task<bool> ExistAsync(Guid UserId, Guid ProductId);
    Task<IReadOnlyList<Review>> GetByProductIdAsync(Guid productId, int take = 20);
}
