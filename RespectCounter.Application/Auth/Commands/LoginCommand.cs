using MediatR;
using RespectCounter.Domain.Interfaces;

namespace RespectCounter.Application.Commands;

public record LoginCommand(string Username, string Password) : IRequest<string>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly IUnitOfWork uow;

    public LoginCommandHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await uow.UserManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            user = await uow.UserManager.FindByEmailAsync(request.Username);
            if(user == null)
            {
                throw new UnauthorizedAccessException("Invalid username.");
            }
        }        

        if (!await uow.UserManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid password.");
        }

        var roles = await uow.UserManager.GetRolesAsync(user);

        return uow.JwtService.GenerateToken(user, roles);
    }
}