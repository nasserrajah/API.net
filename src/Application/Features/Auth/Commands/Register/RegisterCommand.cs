using MediatR;

namespace core_first.Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<bool>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}