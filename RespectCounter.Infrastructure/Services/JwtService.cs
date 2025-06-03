using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RespectCounter.Application.Common;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly JwtSettings jwtSettings;

    public JwtService(IOptions<JwtSettings> jwtSettings)
    {
        this.jwtSettings = jwtSettings.Value;
    }

    public (string accessToken, DateTime accessTokenExpiration) GenerateAccessToken(User user, IEnumerable<string>? roles = null)
    {
        var secret = jwtSettings.AccessTokenSecret
            ?? throw new NullReferenceException("JWT secret cannot be null.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
        };

        if (roles != null)
        {
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        }

        var encodedKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(encodedKey, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings.AccessTokenExpirationMinutes)),
            signingCredentials: creds
        );

        string accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        DateTime accessTokenExpiration = DateTime.Now.AddMinutes(jwtSettings.AccessTokenExpirationMinutes);

        return (accessToken, accessTokenExpiration);
    }

    public (string refreshToken, DateTime refreshTokenExpiration) GenerateRefreshToken(int size = 32)
    {
        var randomNumber = new byte[size];

        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        var guid = Guid.NewGuid().ToString("N");

        var refreshToken = $"{Convert.ToBase64String(randomNumber)}.{guid}";
        var refreshTokenExpiration = DateTime.Now.AddDays(jwtSettings.RefreshTokenExpirationDays);

        return new (refreshToken, refreshTokenExpiration);
    }
}