using Microsoft.EntityFrameworkCore;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;
    public ReviewRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistAsync(Guid userId, Guid productId)
    {
        return await _context.Reviews
            .AnyAsync(x => x.UserId == userId && x.ProductId == productId);
    }

    public async Task<IReadOnlyList<Review>> GetByProductIdAsync(Guid productId, int take = 20)
    {
        var safeTake = take <= 0 ? 20 : Math.Min(take, 100);
        return await _context.Reviews
            .Where(x => x.ProductId == productId)
            .OrderByDescending(x => x.CreatedAt)
            .Take(safeTake)
            .ToListAsync();
    }
}
