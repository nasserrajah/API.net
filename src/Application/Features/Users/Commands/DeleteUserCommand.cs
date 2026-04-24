using core_first.Application.Features.Users.Interfaces;
using core_first.Application.Interfaces.Repositories;
using core_first.Domain.Enums;

namespace core_first.Application.Features.Users.Commands;

public class DeleteUserCommand : IDeleteUserCommand
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommand(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return false;

        if (user.Role == UserRole.Admin)
        {
            var admins = await _userRepository.FindAsync(u => u.Role == UserRole.Admin);
            if (admins.Count() == 1)
                throw new InvalidOperationException("لا يمكن حذف المدير الوحيد في النظام.");
        }

        await _userRepository.DeleteAsync(user);
        return true;
    }
}