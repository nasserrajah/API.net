using core_first.Application.Features.Users.DTOs;

namespace core_first.Application.Features.Users.Interfaces;

public interface IGetAllUsersQuery
{
    Task<IEnumerable<UserResponseDto>> ExecuteAsync();
}