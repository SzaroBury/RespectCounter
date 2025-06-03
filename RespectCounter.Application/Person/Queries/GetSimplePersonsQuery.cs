using MediatR;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Queries
{
    public record GetSimplePersonsQuery() : IRequest<List<SimplePersonDTO>>;

    public class GetSimplePersonsQueryHandler : IRequestHandler<GetSimplePersonsQuery, List<SimplePersonDTO>>
    {
        private readonly IUnitOfWork uow;
        
        public GetSimplePersonsQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<List<SimplePersonDTO>> Handle(GetSimplePersonsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // var persons = await uow.Repository()
            //                     .FindQueryable<Person>(p => p.Status != PersonStatus.Hidden)
            //                     .Select(p => p.ToSimpleDTO())
            //                     .ToListAsync(cancellationToken);
            // return persons;
        }
    }
}