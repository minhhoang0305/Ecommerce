using System.Data.Common;

public class Orders
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public List<OrderItems> Items { get; private set; } = new();
    public decimal TotalAmount => Items.Sum(item => item.Price * item.Quantity);
    public decimal Discount {get; private set;}
    public decimal FinalAmount => TotalAmount - Discount;
    public bool IsPaid { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public Guid? CouponId {get; private set;}
    public Coupon? Coupon {get; private set;}


    public void AddItem(string productName, decimal price, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

        Items.Add(new OrderItems(productName, price, quantity));
    }

    public void ApplyCoupon(Coupon coupon)
    {
        if (coupon == null || !coupon.IsActive)
            throw new ArgumentException("Coupon don't active");
        if (DateTime.UtcNow < coupon.StartDate || DateTime.UtcNow > coupon.EndDate)
            throw new ArgumentException("Coupon expirydate");
        if (coupon.UsageLimit > 0 && coupon.TimesUsed >= coupon.UsageLimit)
            throw new ArgumentException("Coupon đã được sử dụng");
        
        Discount = Math.Round(TotalAmount * (coupon.DiscountPercent /100),2);
        CouponId = coupon.Id;
        Coupon = coupon;
    }

    public void MarkAsPaid()
    {
        IsPaid = true;
    }
}
