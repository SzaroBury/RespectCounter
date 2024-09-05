using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Repositories;
using System.Net;

namespace RespectCounter.Application.Queries
{
    public record GetPersonByIdQuery() : IRequest<Person>
    {
        public int Id { get; init; }
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
            var person = await uow.Repository().GetById<Person>(request.Id);
            if (person is null)
                throw new KeyNotFoundException("Person not found. Please enter the existing Person Id.");

            return person;
        }
    }
}