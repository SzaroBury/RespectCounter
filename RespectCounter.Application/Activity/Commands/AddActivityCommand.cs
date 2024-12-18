using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Repositories;

namespace RespectCounter.Application.Commands;

public record AddActivityCommand(
    string Person,
    string Value, 
    string Description, 
    string Location, 
    string Happend, 
    string Source, 
    int Type, 
    string Tags
) : IRequest<Activity>;

public class AddActivityCommandHandler : IRequestHandler<AddActivityCommand, Activity>
{
    private readonly IUnitOfWork uow;

    public AddActivityCommandHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<Activity> Handle(AddActivityCommand request, CancellationToken cancellationToken)
    {
        DateTime now = DateTime.Now;
        Guid newId = Guid.NewGuid();
        
        if(!DateTime.TryParse(request.Happend, out DateTime happend))
        {
            throw new ArgumentException("Invalid date format.");
        }

        var type = (ActivityType)request.Type;
        if(!Enum.IsDefined(typeof(ActivityType), type))
            throw new ArgumentException("Invalid Type value.");
        

        Activity? newActivity = new Activity
        {
            Id = newId,
            Value = request.Value,
            Location = request.Location,
            Description = request.Description,
            Source = request.Source,
            Type = type,
            Happend = happend,

            Created = now,
            CreatedById = "sys",
            LastUpdated = now,
            LastUpdatedById = "sys"
        };

        if(!Guid.TryParse(request.Person, out Guid personGuid))
        {
            throw new ArgumentException($"Invalid person guid format: '{request.Person}'");
        }

        Person? person = uow.Repository().FindQueryable<Person>(p => p.Id == personGuid).FirstOrDefault();
        if(person == null)
        {
            throw new ArgumentException($"Person with given id was not found: '{request.Person}'");
        }
        newActivity.Person = person;

        List<string> tags = request.Tags.Split(",").ToList();
        foreach(string tag in tags)
        {
            Tag? existingTag = uow.Repository().FindQueryable<Tag>(t => t.Name.ToLower() == tag.ToLower()).FirstOrDefault();
            if(existingTag == null)
            {
                Tag newTag = new Tag 
                {
                    Name = tag,
                    Description = $"Created with {newId} activity object.",
                    Level = 5,
                    
                    Created = now,
                    CreatedById = "sys",
                    LastUpdated = now,
                    LastUpdatedById = "sys"
                };
                existingTag = uow.Repository().Add(newTag);
            }
            newActivity.Tags.Add(existingTag);
        }
        uow.Repository().Add(newActivity);
        await uow.CommitAsync(cancellationToken);

        newActivity = await uow.Repository().SingleOrDefaultAsync<Activity>(a => a.Id == newId, "");

        if(newActivity != null) return newActivity;
        else throw new Exception("Unknown error during saving.");
    }
}