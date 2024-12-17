using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Repositories;

namespace RespectCounter.Application.Commands;

public record AddPersonCommand(
    string FirstName, 
    string LastName, 
    string Description, 
    string Nationality, 
    string Birthday, 
    string? DeathDate, 
    string Tags
) : IRequest<Person>;

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
        DateOnly? deathDate = null;
        
        if(!DateOnly.TryParse(request.Birthday, out DateOnly birthday))
        {
            throw new ArgumentException("Invalid birthday format.");
        }

        if(!string.IsNullOrEmpty(request.DeathDate))
        {
            if(!DateOnly.TryParse(request.DeathDate, out DateOnly result))
            {
                throw new ArgumentException("Invalid deathDate format.");
            }
            deathDate = result;
        }

        Person? newPerson = new Person
        {
            Id = newId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Description = request.Description,
            Nationality = request.Nationality,
            Birthday = birthday,
            DeathDate = deathDate,

            Status = PersonStatus.NotVerified,
            Created = now,
            CreatedById = "sys",
            LastUpdated = now,
            LastUpdatedById = "sys"
        };

        List<string> tags = request.Tags.Split(",").ToList();
        foreach(string tag in tags)
        {
            Tag? existingTag = uow.Repository().FindQueryable<Tag>(t => t.Name.ToLower() == tag.ToLower()).FirstOrDefault();
            if(existingTag == null)
            {
                Tag newTag = new Tag 
                {
                    Name = tag,
                    Description = $"Created with {request.FirstName} {request.LastName} person object.",
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
