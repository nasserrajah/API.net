using MediatR;
using core_first.Application.Features.Users.DTOs;

namespace core_first.Application.Features.Users.Queries.GetById;

public class GetUserByIdQuery : IRequest<UserResponseDto?>
{
    public int Id { get; set; }
}