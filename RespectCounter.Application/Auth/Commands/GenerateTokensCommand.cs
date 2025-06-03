using MediatR;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Model;

namespace RespectCounter.Application.Commands;

public record GenerateTokensCommand(User User) : IRequest<AuthTokensDTO>;

public class GenerateTokensCommandHandler : IRequestHandler<GenerateTokensCommand, AuthTokensDTO>
{
    private readonly IUserService userService;
    private readonly IJwtService jwtService;

    public GenerateTokensCommandHandler(IUserService userService, IJwtService jwtService)
    {
        this.userService = userService;
        this.jwtService = jwtService;
    }

    public async Task<AuthTokensDTO> Handle(GenerateTokensCommand request, CancellationToken cancellationToken)
    {
        var roles = await userService.GetRolesAsync(request.User);
        (string accessToken, DateTime accessTokenExpiration) = jwtService.GenerateAccessToken(request.User, roles);
        (string refreshToken, DateTime refreshTokenExpiration) = jwtService.GenerateRefreshToken();
        await userService.SetUserRefreshTokenAsync(request.User.Id, refreshToken, refreshTokenExpiration);

        return new AuthTokensDTO(accessToken, accessTokenExpiration, refreshToken, refreshTokenExpiration);
    }
}