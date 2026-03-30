public interface ICouponRepository
{
    Task<Guid> CreateAsync(Coupon coupon);
    Task<Coupon?> GetByCodeAsync(string code);
    Task UpdateAsync(Coupon coupon);
}
