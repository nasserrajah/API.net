using core_first.Application.Features.Users.DTOs;
using core_first.Application.Features.Users.Interfaces;
using core_first.Application.Interfaces.Repositories;

namespace core_first.Application.Features.Users.Queries;

public class GetUserByIdQuery : IGetUserByIdQuery
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQuery(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponseDto?> ExecuteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;
        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };
    }
}