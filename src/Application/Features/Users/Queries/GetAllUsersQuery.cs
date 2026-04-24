using core_first.Application.Features.Users.DTOs;
using core_first.Application.Features.Users.Interfaces;
using core_first.Application.Interfaces.Repositories;

namespace core_first.Application.Features.Users.Queries;

public class GetAllUsersQuery : IGetAllUsersQuery
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQuery(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserResponseDto>> ExecuteAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => new UserResponseDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            Role = u.Role,
            CreatedAt = u.CreatedAt
        });
    }
}