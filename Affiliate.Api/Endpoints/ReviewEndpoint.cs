using MediatR;
using System.Security.Claims;
using Affiliate.Application.DTOs.Reviews;
using Affiliate.Domain.Entities;
public static class ReviewEndpoints
{
    public static void MapReviewEndpoints(this WebApplication app)
    {
        app.MapGet("/api/v1/products/{productId:guid}/reviews", async (
            Guid productId,
            int? take,
            IReviewRepository reviewRepository) =>
        {
            var reviews = await reviewRepository.GetByProductIdAsync(productId, take ?? 20);

            var total = reviews.Count;
            var average = total == 0 ? 0m : Math.Round((decimal)reviews.Sum(x => x.Rating) / total, 2, MidpointRounding.AwayFromZero);

            var dto = new ProductReviewsDto(
                productId,
                average,
                total,
                reviews.Select(x => new ReviewItemDto(x.Id, x.UserId, x.Rating, x.Comment, x.CreatedAt)).ToList());

            return Results.Ok(dto);
        });

        app.MapPost("/api/v1/reviews", async (
            CreateReviewRequest request,
            ClaimsPrincipal user,
            IMediator mediator) =>
        {
            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
                return Results.Unauthorized();

            var id = await mediator.Send(new CreateReviewCommand(
                userId,
                request.ProductId,
                request.Rating,
                request.Comment));
            return Results.Ok(id);
        }).RequireAuthorization("UserOnly");
    }
}
