using MediatR;
using core_first.Application.Features.Auth.DTOs;
using core_first.Application.Interfaces.Repositories;
using core_first.Application.Interfaces.Services;

namespace core_first.Application.Features.Auth.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, TokenDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;

    public LoginQueryHandler(IUserRepository userRepository, IJwtProvider jwtProvider, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<TokenDto?> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            return null;

        var (token, expiresAt) = _jwtProvider.GenerateTokenWithExpiry(user);
        return new TokenDto { Token = token, ExpiresAt = expiresAt };
    }
}