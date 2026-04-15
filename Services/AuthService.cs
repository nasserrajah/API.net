using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using core_first.API.Models;
using core_first.API.DTOs;
using core_first.API.Repositories;

namespace core_first.API.Services
{
    public interface IAuthService
    {
        Task<string?> Authenticate(LoginDto loginDto);
        Task<bool> Register(RegisterDto registerDto);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<string?> Authenticate(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return null;

            return GenerateJwtToken(user);
        }

        public async Task<bool> Register(RegisterDto registerDto)
        {
            if (await _userRepository.ExistsAsync(u => u.Username == registerDto.Username))
                return false;

            var user = new User
            {
                Username = registerDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Email = registerDto.Email,
                Role = registerDto.Role,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            return true;
        }

        private string GenerateJwtToken(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var jwtSettings = _config.GetSection("JwtSettings");
            var secret = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret missing");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiryMinutes = double.TryParse(jwtSettings["ExpiryMinutes"], out var minutes) ? minutes : 60.0;

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"] ?? "CoreFirstAPI",
                audience: jwtSettings["Audience"] ?? "CoreFirstApp",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}