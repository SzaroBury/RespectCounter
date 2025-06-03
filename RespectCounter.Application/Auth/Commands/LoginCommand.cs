using System.Security;
using MediatR;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Commands;

public record LoginCommand(string Username, string Password) : IRequest<AuthTokensDTO>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthTokensDTO>
{
    private readonly IUserService userService;
    private readonly IMediator mediator;

    public LoginCommandHandler(IUserService userService, IMediator mediator)
    {
        this.userService = userService;
        this.mediator = mediator;
    }

    public async Task<AuthTokensDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.FindByNameAsync(request.Username);
        if (user == null)
        {
            user = await userService.FindByEmailAsync(request.Username);
            if(user == null)
            {
                throw new SecurityException($"{request.Username} was not found.");
            }
        }  

        if (!await userService.CheckPasswordAsync(request.Username, request.Password))
        {
            throw new SecurityException("Incorrect password.");
        }

        var generateTokensCommand = new GenerateTokensCommand(user);
        var tokens = await mediator.Send(generateTokensCommand, cancellationToken);

        return tokens;
    }
}