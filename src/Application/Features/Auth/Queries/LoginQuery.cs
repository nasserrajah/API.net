using core_first.Application.Features.Auth.DTOs;
using core_first.Application.Features.Auth.Interfaces;
using core_first.Application.Interfaces.Repositories;
using core_first.Application.Interfaces.Services;

namespace core_first.Application.Features.Auth.Queries;

public class LoginQuery : ILoginQuery
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;

    public LoginQuery(IUserRepository userRepository, IJwtProvider jwtProvider, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<TokenDto?> ExecuteAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
        if (user == null || !_passwordHasher.Verify(loginDto.Password, user.PasswordHash))
            return null;

        var (token, expiresAt) = _jwtProvider.GenerateTokenWithExpiry(user);
        return new TokenDto { Token = token, ExpiresAt = expiresAt };
    }
}