using core_first.Application.Features.Users.Interfaces;
using core_first.Application.Interfaces.Repositories;
using core_first.Domain.Enums;

namespace core_first.Application.Features.Users.Commands;

public class UpdateUserRoleCommand : IUpdateUserRoleCommand
{
    private readonly IUserRepository _userRepository;

    public UpdateUserRoleCommand(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> ExecuteAsync(int id, UserRole newRole)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return false;

        if (user.Role == UserRole.Admin && newRole != UserRole.Admin)
        {
            var admins = await _userRepository.FindAsync(u => u.Role == UserRole.Admin);
            if (admins.Count() == 1)
                throw new InvalidOperationException("لا يمكن إزالة دور المدير من المستخدم الأخير الذي يملك هذا الدور.");
        }

        user.Role = newRole;
        await _userRepository.UpdateAsync(user);
        return true;
    }
}