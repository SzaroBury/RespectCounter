using MediatR;
using RespectCounter.Application.DTOs;

namespace RespectCounter.Application.Queries
{
    public record GetClaimsQuery(string? Cookie) : IRequest<IEnumerable<ClaimDTO>>;

    public class GetClaimsQueryHandler : IRequestHandler<GetClaimsQuery, IEnumerable<ClaimDTO>>
    {

        public Task<IEnumerable<ClaimDTO>> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}