namespace Affiliate.Application.DTOs.Reviews;

public record ReviewItemDto(
    int Id,
    int UserId,
    int Rating,
    string Comment,
    DateTime CreatedAt);

public record ProductReviewsDto(
    int ProductId,
    decimal AverageRating,
    int TotalReviews,
    IReadOnlyList<ReviewItemDto> Items);
