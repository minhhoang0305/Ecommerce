using Affiliate.Application.DTOs.Reviews;
using MediatR;

public record GetReviewCommand(
    int productId, 
    int? Take
) : IRequest<ProductReviewsDto>;
