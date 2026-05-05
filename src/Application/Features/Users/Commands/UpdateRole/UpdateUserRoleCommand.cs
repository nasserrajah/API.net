using MediatR;
using core_first.Domain.Enums;

namespace core_first.Application.Features.Users.Commands.UpdateRole;

public class UpdateUserRoleCommand : IRequest<bool>
{
    public int Id { get; set; }
    public UserRole NewRole { get; set; }
}