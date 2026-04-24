using core_first.Domain.Entities;

namespace core_first.Application.Interfaces.Services;

public interface IJwtProvider
{
    string GenerateToken(User user);
    (string Token, DateTime ExpiresAt) GenerateTokenWithExpiry(User user);
}