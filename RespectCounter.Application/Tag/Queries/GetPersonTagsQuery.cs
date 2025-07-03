using MediatR;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Services;

namespace RespectCounter.Application.Queries
{
    public record GetPersonTagsQuery(string Id) : IRequest<IEnumerable<SimpleTagDTO>>;

    public class GetPersonTagsQueryHandler : IRequestHandler<GetPersonTagsQuery, IEnumerable<SimpleTagDTO>>
    {
        private readonly IUnitOfWork uow;
        
        public GetPersonTagsQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IEnumerable<SimpleTagDTO>> Handle(GetPersonTagsQuery request, CancellationToken cancellationToken)
        {
            var person = await uow.Repository().SingleOrDefaultAsync<Person>(p => p.Id.ToString() == request.Id, "Tags");
            if (person is null)
            {
                throw new KeyNotFoundException("Person was not found. Please enter the existing Person Id.");
            }
            var tags = person.Tags.Select(t => t.ToSimpleDTO());
            return tags;
        }
    }
}