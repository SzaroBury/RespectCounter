using MediatR;
using RespectCounter.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace RespectCounter.Application.Commands;

public record RegisterCommand(string Email, string Username, string Password) : IRequest<IdentityResult>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, IdentityResult>
{
    private readonly IUnitOfWork uow;

    public RegisterCommandHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<IdentityResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var listOfErrors = new List<IdentityError>();
        if (string.IsNullOrEmpty(request.Username))
        {
            IdentityError error = new();
            error.Code = "UsernameRequired";
            error.Description = "Username is required.";
            listOfErrors.Add(error);
        }
        if (await uow.UserManager.FindByNameAsync(request.Username) != null)
        {
            IdentityError error = new();
            error.Code = "UsernameTaken";
            error.Description = "Username is already in use.";
            listOfErrors.Add(error);
        }
        if (string.IsNullOrEmpty(request.Email))
        {
            IdentityError error = new();
            error.Code = "EmailRequired";
            error.Description = "Email is required.";
            listOfErrors.Add(error);
        }
        if(await uow.UserManager.FindByEmailAsync(request.Email) != null)
        {
            IdentityError error = new();
            error.Code = "EmailTaken";
            error.Description = "Email is already in use.";
            listOfErrors.Add(error);
        }
        if (string.IsNullOrEmpty(request.Password))
        {
            IdentityError error = new();
            error.Code = "PasswordRequired";
            error.Description = "Password cannot be empty.";
            listOfErrors.Add(error);
        }

        if(listOfErrors.Count > 0)
        {
            return IdentityResult.Failed(listOfErrors.ToArray());
        }

        var user = new IdentityUser(request.Username);
        user.Email = request.Email;
        var result = await uow.UserManager.CreateAsync(user, request.Password); //to-do: implement cancellationToken
        return result;
    }
}