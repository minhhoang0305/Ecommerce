public class Coupon
{
    public Guid Id {get; set;}
    public string Code {get; set;} = null!;
    public decimal DiscountPercent {get; set;}
    public DateTime StartDate {get; set;}
    public DateTime EndDate {get; set;}
    public bool IsActive {get; set;}
    public int UsageLimit {get; set;}
    public int TimesUsed {get; set;}
}