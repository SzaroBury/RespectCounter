namespace RespectCounter.Application.DTOs;

public record AuthTokensDTO(
    string AccessToken,
    DateTime AccessTokenExpiration,
    string RefreshToken,
    DateTime RefreshTokenExpiration
);