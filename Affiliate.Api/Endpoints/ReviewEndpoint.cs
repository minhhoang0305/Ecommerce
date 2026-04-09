using MediatR;
using System.Security.Claims;
using Affiliate.Application.DTOs.Reviews;
using Affiliate.Domain.Entities;
public static class ReviewEndpoints
{
    public static void MapReviewEndpoints(this WebApplication app)
    {
        app.MapGet("/api/v1/products/{productId:int}/reviews", async (
            int productId,
            int? take,
            IMediator mediator) =>
        {
            var result = await mediator.Send(
                new GetReviewCommand(productId, take));

            return Results.Ok(result);
        }).WithTags("Review");

        app.MapPost("/api/v1/reviews", async (
            CreateReviewRequest request,
            ClaimsPrincipal user,
            IMediator mediator) =>
        {
            if (!user.TryGetUserId(out var userId))
                return Results.Unauthorized();

            var created = await mediator.Send(new CreateReviewCommand(
                userId,
                request.ProductId,
                request.Rating,
                request.Comment));
            return Results.Ok(created);
        }).RequireAuthorization("UserOnly").WithTags("Review");
    }
}
