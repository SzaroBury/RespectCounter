using MediatR;
using RespectCounter.Application.DTOs;
using RespectCounter.Application.Services;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Model;

namespace RespectCounter.Application.Queries
{
    public record GetTagsQuery(int Level = 5) : IRequest<IEnumerable<TagDTO>>;

    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, IEnumerable<TagDTO>>
    {
        private readonly IUnitOfWork uow;

        public GetTagsQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IEnumerable<TagDTO>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {               
            var tags = await uow.Repository().FindListAsync<Tag>(
                t => t.Level < request.Level,
                ["Activities", "Persons"],
                q => q.OrderByDescending(c => c.Created),
                cancellationToken
            );
            return tags.Select(p => p.ToDTO());
        }
    }
}