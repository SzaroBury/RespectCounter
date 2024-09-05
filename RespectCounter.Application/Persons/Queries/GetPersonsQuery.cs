using MediatR;
using System.Net;

namespace RespectCounter.Application.Persons.Queries
{
    public record GetPersonsQuery() : IRequest<string>;

    public class GetPersonsQueryHandler : IRequestHandler<GetPersonsQuery, string>
    {
        public async Task<string> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
        {
            return "test";
        }
    }
}