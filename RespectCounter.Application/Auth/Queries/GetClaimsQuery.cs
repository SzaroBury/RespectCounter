using MediatR;
using RespectCounter.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using RespectCounter.Application.DTOs;

namespace RespectCounter.Application.Queries
{
    public record GetClaimsQuery(string cookie) : IRequest<IEnumerable<ClaimDTO>>;

    public class GetClaimsQueryHandler : IRequestHandler<GetClaimsQuery, IEnumerable<ClaimDTO>>
    {
        private readonly IUnitOfWork uow;
        
        public GetClaimsQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public Task<IEnumerable<ClaimDTO>> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
        {
            var token = request.cookie;
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("No token provided");
            }

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken == null)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }

            var claims = jsonToken.Claims.Select(c => c.ToDTO());

            return Task.FromResult(claims);
        }
    }
}