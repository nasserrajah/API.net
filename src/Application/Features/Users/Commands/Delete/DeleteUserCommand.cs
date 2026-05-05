using MediatR;

namespace core_first.Application.Features.Users.Commands.Delete;

public class DeleteUserCommand : IRequest<bool>
{
    public int Id { get; set; }
}