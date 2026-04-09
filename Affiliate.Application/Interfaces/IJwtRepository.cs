public interface IJwtRepository
{
    string GenerateToken(int userId, string email, string role);
}
