using MediatR;
using RespectCounter.Domain.DTO;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Repositories;

namespace RespectCounter.Application.Commands
{
    public record AddPersonCommand() : IRequest<Person>
    {
        public required NewPerson Person { get; set; }
    }

    public class AddPersonCommandHandler : IRequestHandler<AddPersonCommand, Person>
    {
        private readonly IUnitOfWork uow;

        public AddPersonCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Person> Handle(AddPersonCommand request, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.Now;
            Guid newId = Guid.NewGuid();

            Person? newPerson = new Person
            {
                Id = newId,
                FirstName = request.Person.FirstName,
                LastName = request.Person.LastName,
                Description = request.Person.Description,
                Nationality = request.Person.Nationality,
                Birthday = DateOnly.Parse(request.Person.Birthday),
                DeathDate = request.Person.DeathDate == null ? null : DateOnly.Parse(request.Person.DeathDate),

                Status = PersonStatus.NotVerified,
                Created = now,
                CreatedById = "sys",
                LastUpdated = now,
                LastUpdatedById = "sys"
            };

            List<string> tags = request.Person.Tags.Split(",").ToList();
            foreach(string tag in tags)
            {
                Tag? existingTag = uow.Repository().FindQueryable<Tag>(t => t.Name == tag).FirstOrDefault();
                if(existingTag == null)
                {
                    Tag newTag = new Tag 
                    {
                        Name = tag,
                        Description = $"Created with {request.Person.FirstName} {request.Person.LastName} person object.",
                        Level = 5,
                        
                        Created = now,
                        CreatedById = "sys",
                        LastUpdated = now,
                        LastUpdatedById = "sys"
                    };
                    existingTag = uow.Repository().Add(newTag);
                }
                newPerson.Tags.Add(existingTag);
            }
            uow.Repository().Add(newPerson);
            await uow.CommitAsync(cancellationToken);

            newPerson = await uow.Repository().SingleOrDefaultAsync<Person>(p => p.Id == newId, "");

            if(newPerson != null)   return newPerson;
            else throw new Exception("Unknown error during saving.");
        }
    }
}