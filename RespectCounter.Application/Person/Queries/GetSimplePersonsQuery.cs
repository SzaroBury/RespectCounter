using MediatR;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Services;

namespace RespectCounter.Application.Queries
{
    public record GetSimplePersonsQuery() : IRequest<IEnumerable<SimplePersonDTO>>;

    public class GetSimplePersonsQueryHandler : IRequestHandler<GetSimplePersonsQuery, IEnumerable<SimplePersonDTO>>
    {
        private readonly IUnitOfWork uow;
        
        public GetSimplePersonsQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IEnumerable<SimplePersonDTO>> Handle(GetSimplePersonsQuery request, CancellationToken cancellationToken)
        {
            var persons = await uow.Repository().FindListAsync<Person>(
                p => p.Status != PersonStatus.Hidden,
                ["Activities", "Persons"],
                q => q.OrderByDescending(c => c.Created),
                cancellationToken
            );
            return persons.Select(p => p.ToSimpleDTO());
        }
    }
}