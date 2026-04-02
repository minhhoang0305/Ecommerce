using MediatR;

public record CreateReviewCommand(
    Guid UserId,
    Guid ProductId,
    int Rating,
    string Comment
) : IRequest<Guid>;
