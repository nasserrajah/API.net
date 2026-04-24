using core_first.Domain.Enums;

namespace core_first.Application.Features.Users.Interfaces;

public interface IUpdateUserRoleCommand
{
    Task<bool> ExecuteAsync(int id, UserRole newRole);
}