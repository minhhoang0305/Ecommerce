namespace Affiliate.Application.DTOs.Reviews;

public record CreateReviewRequest(
    Guid ProductId,
    int Rating,
    string Comment);

