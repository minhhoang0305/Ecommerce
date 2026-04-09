public interface ICouponRepository
{
    Task<int> CreateAsync(Coupon coupon);
    Task<Coupon?> GetByCodeAsync(string code);
    Task UpdateAsync(Coupon coupon);
}
