using MediatR;
using Microsoft.EntityFrameworkCore;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Interfaces;

namespace RespectCounter.Application.Queries
{
    public record GetPersonByIdQuery(string Id) : IRequest<PersonDTO>;

    public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, PersonDTO>
    {
        private readonly IUnitOfWork uow;
        
        public GetPersonByIdQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<PersonDTO> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            var person = await uow.Repository().FindQueryable<Person>(p => p.Id.ToString() == request.Id)
                .Include(p => p.Tags)
                .Include(p => p.Comments)
                .Include(p => p.Activities)
                .Include(p => p.Reactions)
                .FirstOrDefaultAsync(cancellationToken);
            if (person is null)
                throw new KeyNotFoundException("Person not found. Please enter the existing Person Id.");

            return person.ToDTO();
        }
    }
}