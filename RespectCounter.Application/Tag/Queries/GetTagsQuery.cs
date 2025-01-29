using MediatR;
using RespectCounter.Domain.Interfaces;
using RespectCounter.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace RespectCounter.Application.Queries
{
    public record GetTagsQuery : IRequest<List<Tag>>
    {
        public int Level { get; set; } = 5;

        public GetTagsQuery(int level)
        {
            Level = level;
        }
    }

    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, List<Tag>>
    {
        private readonly IUnitOfWork uow;

        public GetTagsQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<List<Tag>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            return await uow.Repository().FindQueryable<Tag>(t => t.Level > 0)
                .Include("Activities").Include("Persons")
                .OrderByDescending(t => t.Activities.Count + t.Persons.Count)
                .ToListAsync(cancellationToken);
        }
    }
}