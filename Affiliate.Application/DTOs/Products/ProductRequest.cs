namespace Affiliate.Application.DTOs.Products;

public class ProductsRequest
{
    public string Name { get; set; } = default!;
    public string Category {get; set;} = default!;
    public decimal Price { get; set; }
    public string Description { get; set; } = default!;
    public string Stock {get; set;} = default!;
    public int Status {get; set;}
    public bool IsDelete {get; set;}
}