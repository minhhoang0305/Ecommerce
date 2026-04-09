using MediatR;
using Affiliate.Domain.Entities;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, int>
{
    private readonly IUserRepository _userRepository;

    public RegisterUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Check email
        var exists = await _userRepository.ExistsByEmailAsync(request.Email);
        if (exists)
            throw new Exception("Email already exists");

        // Hash password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);


        var user = new User
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            Role = request.Role
        };

        await _userRepository.AddAsync(user);

        return user.Id;
    }
}
