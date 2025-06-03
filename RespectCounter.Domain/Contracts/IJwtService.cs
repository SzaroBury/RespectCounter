using RespectCounter.Domain.Model;

namespace RespectCounter.Domain.Contracts;

public interface IJwtService
{
    (string accessToken, DateTime accessTokenExpiration) GenerateAccessToken(User user, IEnumerable<string>? roles);
    (string refreshToken, DateTime refreshTokenExpiration) GenerateRefreshToken(int size = 32);
}