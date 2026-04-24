using core_first.Application.Features.Auth.DTOs;
using core_first.Application.Features.Auth.Interfaces;
using core_first.Application.Interfaces.Repositories;
using core_first.Application.Interfaces.Services;
using core_first.Domain.Entities;
using core_first.Domain.Enums;

namespace core_first.Application.Features.Auth.Commands;

public class RegisterCommand : IRegisterCommand
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommand(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> ExecuteAsync(RegisterDto registerDto)
    {
        if (await _userRepository.ExistsAsync(u => u.Username == registerDto.Username))
            return false;

        var user = new User
        {
            Username = registerDto.Username,
            PasswordHash = _passwordHasher.Hash(registerDto.Password),
            Email = registerDto.Email,
            Role = UserRole.User,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
        return true;
    }
}