using Microsoft.EntityFrameworkCore;

public class CouponRepository : ICouponRepository
{
    private readonly AppDbContext _context;

    public CouponRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateAsync(Coupon coupon)
    {
        await _context.Coupon.AddAsync(coupon);
        await _context.SaveChangesAsync();
        return coupon.Id;
    }

    public async Task<Coupon?> GetByCodeAsync(string code)
    {
        return await _context.Coupon.FirstOrDefaultAsync(x => x.Code == code);
    }

    public async Task UpdateAsync(Coupon coupon)
    {
        _context.Coupon.Update(coupon);
        await _context.SaveChangesAsync();
    }
}
