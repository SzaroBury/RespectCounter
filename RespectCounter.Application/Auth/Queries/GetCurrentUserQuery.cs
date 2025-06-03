using MediatR;
using RespectCounter.Domain.Contracts;
using System.Security;
using RespectCounter.Domain.Model;

namespace RespectCounter.Application.Queries
{
    public record GetUserQuery(Guid UserId) : IRequest<User>;

    public class GetCurrentUserQueryHandler : IRequestHandler<GetUserQuery, User>
    {
        private readonly IUserService userService;
        
        public GetCurrentUserQueryHandler(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            User? user = await userService.GetByIdAsync(request.UserId)
                ?? throw new SecurityException("Authentication issue. No user found.");

            return user;
        }
    }
}