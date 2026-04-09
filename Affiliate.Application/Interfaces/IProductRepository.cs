public interface IProductRepository
{
    Task<int> CreateAsync(Products product);
    Task<Products?> GetByIdAsync(int id);
    Task<IEnumerable<Products>> GetAllAsync();
    Task<(IReadOnlyList<Products> Items, int TotalCount)> GetPagedAsync(
        int page,
        int size,
        string? category,
        decimal? minPrice,
        decimal? maxPrice);
    Task DeleteAsync(int id);
    Task UpdateAsync(Products product);
}
