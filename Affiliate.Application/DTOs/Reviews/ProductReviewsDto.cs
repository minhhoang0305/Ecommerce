namespace Affiliate.Application.DTOs.Reviews;

public record ReviewItemDto(
    Guid Id,
    Guid UserId,
    int Rating,
    string Comment,
    DateTime CreatedAt);

public record ProductReviewsDto(
    Guid ProductId,
    decimal AverageRating,
    int TotalReviews,
    IReadOnlyList<ReviewItemDto> Items);

