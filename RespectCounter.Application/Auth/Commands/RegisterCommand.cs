using System.Security;
using MediatR;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Model;

namespace RespectCounter.Application.Commands;

public record RegisterCommand(string Email, string Username, string Password) : IRequest<AuthTokensDTO>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthTokensDTO>
{
    private readonly IUserService userService;
    private readonly IMediator mediator;

    public RegisterCommandHandler(IUserService userService, IMediator mediator)
    {
        this.userService = userService;
        this.mediator = mediator;
    }

    public async Task<AuthTokensDTO> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Username))
        {
            throw new SecurityException("Username is required.");
        }
        if (await userService.GetByNameAsync(request.Username) != null)
        {
            throw new SecurityException("Username is already in use.");
        }
        if (string.IsNullOrEmpty(request.Email))
        {
            throw new SecurityException("Email is required.");
        }
        if(await userService.GetByEmailAsync(request.Email) != null)
        {
            throw new SecurityException("Email is already in use.");
        }
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new SecurityException("Password cannot be empty.");
        }

        var user = await userService.CreateAsync(request.Username, request.Email, request.Password);

        var generateTokensCommand = new GenerateTokensCommand(user);
        var tokens = await mediator.Send(generateTokensCommand, cancellationToken);

        return tokens;
    }
}