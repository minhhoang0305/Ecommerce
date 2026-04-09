using MediatR;
using Affiliate.Application.DTOs.Reviews;

public record CreateReviewCommand(
    int UserId,
    int ProductId,
    int Rating,
    string Comment
) : IRequest<ReviewItemDto>;
