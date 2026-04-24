using System.ComponentModel.DataAnnotations;

namespace core_first.Application.Features.Auth.DTOs;

public class LoginDto
{
    [Required] public string Username { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}