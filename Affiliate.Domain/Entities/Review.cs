public class Review
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid UserId { get; private set; }

    public Guid ProductId { get; private set; }

    public int Rating { get; private set; }

    public string Comment { get; private set; } = default!;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private Review() {}

    public Review(Guid userId, Guid productId, int rating, string comment)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5");

        UserId = userId;
        ProductId = productId;
        Rating = rating;
        Comment = comment;
    }
}