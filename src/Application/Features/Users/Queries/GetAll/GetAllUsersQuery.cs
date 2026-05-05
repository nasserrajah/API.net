using MediatR;
using core_first.Application.Features.Users.DTOs;

namespace core_first.Application.Features.Users.Queries.GetAll;

public class GetAllUsersQuery : IRequest<IEnumerable<UserResponseDto>>
{
}