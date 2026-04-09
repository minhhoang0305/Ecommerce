using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetUserId(this ClaimsPrincipal user, out int userId)
    {
        var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(userIdClaim, out userId);
    }
}
