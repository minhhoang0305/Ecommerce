namespace Affiliate.Application.DTOs.Products;

public record ProductListItemDto(
    int Id,
    string Name,
    string Category,
    decimal Price,
    string Description,
    int Stock,
    int Status);
