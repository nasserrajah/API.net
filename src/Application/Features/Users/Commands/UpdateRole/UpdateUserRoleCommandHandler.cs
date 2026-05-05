using MediatR;
using core_first.Application.Interfaces.Repositories;
using core_first.Domain.Enums;

namespace core_first.Application.Features.Users.Commands.UpdateRole;

public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, bool>
{
    private readonly IUserRepository _userRepository;
    public UpdateUserRoleCommandHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<bool> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null) return false;

        if (user.Role == UserRole.Admin && request.NewRole != UserRole.Admin)
        {
            var admins = await _userRepository.FindAsync(u => u.Role == UserRole.Admin);
            if (admins.Count() == 1)
                throw new InvalidOperationException("لا يمكن إزالة دور المدير من المستخدم الأخير الذي يملك هذا الدور.");
        }

        user.Role = request.NewRole;
        await _userRepository.UpdateAsync(user);
        return true;
    }
}