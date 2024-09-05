using MediatR;
using System.Net;
using RespectCounter.Infrastructure;
using RespectCounter.Infrastructure.Repositories;

namespace RespectCounter.Application.Queries
{
    public record GetPersonsQuery() : IRequest<string>;

    public class GetPersonsQueryHandler : IRequestHandler<GetPersonsQuery, string>
    {
        private readonly IUnitOfWork uow;

        public GetPersonsQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public Task<string> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}