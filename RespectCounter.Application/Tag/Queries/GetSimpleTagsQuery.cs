using MediatR;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.DTOs;

namespace RespectCounter.Application.Queries
{
    public record GetSimpleTagsQuery() : IRequest<List<SimpleTagDTO>>;

    public class GetSimpleTagsQueryHandler : IRequestHandler<GetSimpleTagsQuery, List<SimpleTagDTO>>
    {
        private readonly IUnitOfWork uow;

        public GetSimpleTagsQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<List<SimpleTagDTO>> Handle(GetSimpleTagsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // return await uow.Repository().FindQueryable<Tag>(t => t.Level > 0)
            //     .OrderByDescending(t => t.Activities.Count + t.Persons.Count)
            //     .Select(t => t.ToSimpleDTO())
            //     .ToListAsync(cancellationToken);
        }
    }
}