using MediatR;
using core_first.Application.Interfaces.Repositories;
using core_first.Domain.Enums;

namespace core_first.Application.Features.Users.Commands.Delete;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    public DeleteUserCommandHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
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