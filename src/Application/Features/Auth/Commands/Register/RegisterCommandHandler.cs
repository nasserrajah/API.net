using MediatR;
using core_first.Application.Interfaces.Repositories;
using core_first.Application.Interfaces.Services;
using core_first.Domain.Entities;
using core_first.Domain.Enums;

namespace core_first.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsAsync(u => u.Username == request.Username))
            return false;

        var user = new User
        {
            Username = request.Username,
            PasswordHash = _passwordHasher.Hash(request.Password),
            Email = request.Email,
            Role = UserRole.User,
            CreatedAt = DateTime.UtcNow
        };
        await _userRepository.AddAsync(user);
        return true;
    }
}