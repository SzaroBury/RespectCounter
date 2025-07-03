using System.Globalization;
using MediatR;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Services;

namespace RespectCounter.Application.Commands;

public record AddActivityCommand(
    string Person,
    string Value, 
    string Description, 
    string Location, 
    string Happend, 
    string Source, 
    int Type, 
    string Tags,
    Guid UserId
) : IRequest<ActivityDTO>;

public class AddActivityCommandHandler : IRequestHandler<AddActivityCommand, ActivityDTO>
{
    private readonly IUnitOfWork uow;

    public AddActivityCommandHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<ActivityDTO> Handle(AddActivityCommand request, CancellationToken cancellationToken)
    {
        var newActivity = CreateActivity(request, request.UserId);
        await AddTagsToActivity(request.Tags, newActivity);
        await SaveActivity(newActivity, cancellationToken);
        return newActivity.ToDTO(request.UserId);
    }

    public Activity CreateActivity(AddActivityCommand request, Guid userId)
    {
        DateTime now = DateTime.Now;
        Guid newId = Guid.NewGuid();
        DateTime? happend = null;

        if(!string.IsNullOrEmpty(request.Happend))
        {
            if(!DateTime.TryParseExact(request.Happend, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                throw new ArgumentException("Invalid date format. Expected format: yyyy-MM-ddTHH:mm:ss.fffZ", "Happend");
            }
            happend = parsedDate;
        }

        var type = (ActivityType)request.Type;
        if(!Enum.IsDefined(typeof(ActivityType), type))
        {
            throw new ArgumentException("Invalid Type value. Expected value: 1 or 2.", "Type");
        }

        if(!Guid.TryParse(request.Person, out Guid personGuid))
        {
            throw new ArgumentException($"Invalid person guid format: '{request.Person}'", "Person");
        }

        Person? person = uow.Repository().FindQueryable<Person>(p => p.Id == personGuid).FirstOrDefault();
        if(person == null)
        {
            throw new ArgumentException($"Person with given id was not found: '{request.Person}'", "Person");
        }

        return new Activity
        {
            Id = newId,
            Value = request.Value,
            Location = request.Location,
            Description = request.Description,
            Source = request.Source,
            Type = type,
            Happend = happend,
            Person = person,

            Created = now,
            CreatedById = userId,
            LastUpdated = now,
            LastUpdatedById = userId
        };
    }

    private async Task AddTagsToActivity(string tagsString, Activity newActivity)
    {
        List<string> tags = tagsString.Split(",").ToList();
        foreach (string tag in tags)
        {
            Tag? existingTag = await uow.Repository().SingleOrDefaultAsync<Tag>(t => t.Name.ToLower() == tag.ToLower());
            if (existingTag == null)
            {
                Tag newTag = new Tag
                {
                    Name = tag,
                    Description = $"Created with '{newActivity.Id}' activity object.",
                    Level = 5,
                    Created = DateTime.Now,
                    CreatedById = Guid.Empty,
                    LastUpdated = DateTime.Now,
                    LastUpdatedById = Guid.Empty
                };
                existingTag = uow.Repository().Add(newTag);
            }
            newActivity.Tags.Add(existingTag);
        }
    }

    private async Task SaveActivity(Activity newActivity, CancellationToken cancellationToken)
    {
        uow.Repository().Add(newActivity);
        await uow.CommitAsync(cancellationToken);
    }
}