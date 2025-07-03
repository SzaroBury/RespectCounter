using MediatR;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Commands;

public record LogoutCommand(Guid UserId) : IRequest;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly IUserService userService;

    public LogoutCommandHandler(IUserService userService)
    {
        this.userService = userService;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await userService.SetUserRefreshTokenAsync(request.UserId, null, null);

        return;
    }
}