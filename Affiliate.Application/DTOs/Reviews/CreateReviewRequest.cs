namespace Affiliate.Application.DTOs.Reviews;

public record CreateReviewRequest(
    int ProductId,
    int Rating,
    string Comment);
