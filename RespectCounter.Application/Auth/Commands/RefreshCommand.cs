using MediatR;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Commands;

public record RefreshCommand(string RefreshToken) : IRequest<AuthTokensDTO>;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, AuthTokensDTO>
{
    private readonly IUserService userService;
    private readonly IMediator mediator;

    public RefreshCommandHandler(IUserService userService, IMediator mediator)
    {
        this.userService = userService;
        this.mediator = mediator;
    }

    public async Task<AuthTokensDTO> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.GetByRefreshTokenAsync(request.RefreshToken);

        var generateTokensCommand = new GenerateTokensCommand(user);
        var tokens = await mediator.Send(generateTokensCommand, cancellationToken);

        return tokens;
    }
}