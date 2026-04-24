using core_first.Application.Features.Auth.DTOs;

namespace core_first.Application.Features.Auth.Interfaces;

public interface IRegisterCommand
{
    Task<bool> ExecuteAsync(RegisterDto registerDto);
}