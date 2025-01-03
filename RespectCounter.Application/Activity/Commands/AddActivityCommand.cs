using MediatR;
using MediatR.Pipeline;
using RespectCounter.Domain.DTO;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Repositories;

namespace RespectCounter.Application.Commands;

public record AddActivityCommand(ActivityCommandDTO Activity) : IRequest<Activity>;

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
        
        if(!DateTime.TryParse(request.Activity.Happend, out DateTime happend))
        {
            throw new ArgumentException("Invalid date format.");
        }

        var type = (ActivityType)request.Activity.Type;
        if(!Enum.IsDefined(typeof(ActivityType), type))
            throw new ArgumentException("Invalid Type value.");
        

        Activity? newActivity = new Activity
        {
            Id = newId,
            Value = request.Activity.Value,
            Location = request.Activity.Location,
            Description = request.Activity.Description,
            Source = request.Activity.Source,
            Type = type,
            Happend = happend,

            Created = now,
            CreatedById = "sys",
            LastUpdated = now,
            LastUpdatedById = "sys"
        };

        List<string> persons = request.Activity.Persons.Split(",").ToList();
        foreach(string personId in persons)
        {
            if(!Guid.TryParse(personId, out Guid personGuid))
            {
                throw new ArgumentException($"Invalid person guid format: '{personId}'");
            }

            Person? person = uow.Repository().FindQueryable<Person>(p => p.Id == personGuid).FirstOrDefault();
            if(person == null)
            {
                throw new ArgumentException($"Person with given id was not found: '{personId}'");
            }
            newActivity.Persons.Add(person);
        }

        List<string> tags = request.Activity.Tags.Split(",").ToList();
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