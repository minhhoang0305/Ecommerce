namespace Affiliate.Application.Loyalty;

public static class LoyaltyRanker
{
    public const string Silver = "Silver";
    public const string Gold = "Gold";
    public const string Diamond = "Diamond";

    public static string GetRank(int totalPoints, LoyaltyOptions options)
    {
        if (totalPoints >= options.DiamondFromPoints)
            return Diamond;

        if (totalPoints >= options.GoldFromPoints)
            return Gold;

        return Silver;
    }

    public static decimal GetDiscountPercent(string? rank, LoyaltyOptions options)
    {
        return rank switch
        {
            Diamond => options.DiamondDiscountPercent,
            Gold => options.GoldDiscountPercent,
            _ => options.SilverDiscountPercent
        };
    }
}

