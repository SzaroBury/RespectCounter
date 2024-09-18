using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Repositories;
using System.Net;

namespace RespectCounter.Application.Queries
{
    public record GetPersonByIdQuery : IRequest<Person>
    {
        public string Id { get; init; } = "";
        
        public GetPersonByIdQuery(string id)
        {
            Id = id;
        }
    }

    public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, Person>
    {
        private readonly IUnitOfWork uow;
        
        public GetPersonByIdQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Person> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            var person = await uow.Repository().SingleOrDefaultAsync<Person>(p => p.Id.ToString() == request.Id, "Tags,Comments,Activities,Reactions");
            if (person is null)
                throw new KeyNotFoundException("Person not found. Please enter the existing Person Id.");

            return person;
        }
    }
}