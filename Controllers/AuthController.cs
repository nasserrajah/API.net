using Microsoft.AspNetCore.Mvc;
using core_first.API.DTOs;
using core_first.API.Services;

namespace core_first.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var token = await _authService.Authenticate(loginDto);
            if (token == null)
                return Unauthorized(new { message = "Invalid username or password" });
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _authService.Register(registerDto);
            if (!result)
                return BadRequest(new { message = "Username already exists" });
            return Ok(new { message = "User registered successfully" });
        }
    }
}