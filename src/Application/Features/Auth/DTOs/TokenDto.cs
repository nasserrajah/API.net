namespace core_first.Application.Features.Auth.DTOs;

public class TokenDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}