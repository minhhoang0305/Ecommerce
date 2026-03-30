using MediatR;

public static class CouponEndpoints
{
    public static void MapCouponEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/coupon/create", async (
            CreateCouponCommand command,
            IMediator mediator) =>
        {
            var couponId = await mediator.Send(command);
            return Results.Ok(new { couponId, message = "Coupon created successfully" });
        }).RequireAuthorization("AdminOnly");
    }
}
