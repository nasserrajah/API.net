using Microsoft.AspNetCore.Mvc;
using MediatR;
using core_first.Application.Features.Auth.DTOs;
using core_first.Application.Features.Auth.Queries.Login;
using core_first.Application.Features.Auth.Commands.Register;

namespace core_first.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var query = new LoginQuery { Username = loginDto.Username, Password = loginDto.Password };
        var token = await _mediator.Send(query);
        if (token == null)
            return Unauthorized(new { message = "Invalid username or password" });
        return Ok(token);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var command = new RegisterCommand
        {
            Username = registerDto.Username,
            Password = registerDto.Password,
            Email = registerDto.Email
        };
        var success = await _mediator.Send(command);
        if (!success)
            return BadRequest(new { message = "Username already exists" });
        return Ok(new { message = "User registered successfully" });
    }
}