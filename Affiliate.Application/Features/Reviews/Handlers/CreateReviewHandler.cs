using MediatR;
using Affiliate.Domain.Entities;
using Affiliate.Application.DTOs.Reviews;

public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, ReviewItemDto>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IOrderRepository _orderRepository;

    public CreateReviewHandler(
        IReviewRepository reviewRepository,
        IOrderRepository orderRepository)
    {
        _reviewRepository = reviewRepository;
        _orderRepository = orderRepository;
    }

    public async Task<ReviewItemDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        // Check purchased
        var hasPurchased = await _orderRepository
            .HasUserPurchasedProductAsync(userId, request.ProductId);

        if (!hasPurchased)
            throw new Exception("User has not purchased this product");

        // Check duplicate
        var exists = await _reviewRepository
            .ExistAsync(userId, request.ProductId);

        if (exists)
            throw new Exception("User already reviewed this product");

        // Create review
        var review = new Review(
            userId,
            request.ProductId,
            request.Rating,
            request.Comment
        );

        await _reviewRepository.AddAsync(review);

        return new ReviewItemDto(review.Id, review.UserId, review.Rating, review.Comment, review.CreatedAt);
    }
}
