using MediatR;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Model;
using RespectCounter.Application.Services;

namespace RespectCounter.Application.Queries
{
    public record GetSimpleTagsQuery() : IRequest<IEnumerable<SimpleTagDTO>>;

    public class GetSimpleTagsQueryHandler : IRequestHandler<GetSimpleTagsQuery, IEnumerable<SimpleTagDTO>>
    {
        private readonly IUnitOfWork uow;

        public GetSimpleTagsQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IEnumerable<SimpleTagDTO>> Handle(GetSimpleTagsQuery request, CancellationToken cancellationToken)
        {
            var tags = await uow.Repository().FindListAsync<Tag>(
                t => t.Level == 1,
                ["Activities", "Persons"],
                q => q.OrderByDescending(c => c.Created),
                cancellationToken
            );
            return tags.Select(t => t.ToSimpleDTO());
        }
    }
}