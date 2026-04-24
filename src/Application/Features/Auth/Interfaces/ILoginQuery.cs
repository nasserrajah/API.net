using core_first.Application.Features.Auth.DTOs;

namespace core_first.Application.Features.Auth.Interfaces;

public interface ILoginQuery
{
    Task<TokenDto?> ExecuteAsync(LoginDto loginDto);
}