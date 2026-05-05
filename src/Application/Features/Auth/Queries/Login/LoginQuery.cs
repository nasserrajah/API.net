using MediatR;
using core_first.Application.Features.Auth.DTOs;

namespace core_first.Application.Features.Auth.Queries.Login;

public class LoginQuery : IRequest<TokenDto?>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}