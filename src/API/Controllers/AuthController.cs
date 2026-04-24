using Microsoft.AspNetCore.Mvc;
using core_first.Application.Features.Auth.DTOs;
using core_first.Application.Features.Auth.Interfaces;

namespace core_first.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILoginQuery _loginQuery;
    private readonly IRegisterCommand _registerCommand;

    public AuthController(ILoginQuery loginQuery, IRegisterCommand registerCommand)
    {
        _loginQuery = loginQuery;
        _registerCommand = registerCommand;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var tokenDto = await _loginQuery.ExecuteAsync(loginDto);
        if (tokenDto == null)
            return Unauthorized(new { message = "Invalid username or password" });
        return Ok(tokenDto);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var success = await _registerCommand.ExecuteAsync(registerDto);
        if (!success)
            return BadRequest(new { message = "Username already exists" });
        return Ok(new { message = "User registered successfully" });
    }
}