using System.Globalization;
using System.Security;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RespectCounter.Application.DTOs;
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
    string Tags,
    ClaimsPrincipal User
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
        DateTime now = DateTime.Now;
        Guid newId = Guid.NewGuid();
        DateTime? happend = null;

        IdentityUser? user = await uow.UserManager.GetUserAsync(request.User);
        if(user == null) throw new SecurityException("Authentication issue. No user found.");

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
            CreatedById = user.Id,
            LastUpdated = now,
            LastUpdatedById = user.Id
        };

        if(!Guid.TryParse(request.Person, out Guid personGuid))
        {
            throw new ArgumentException($"Invalid person guid format: '{request.Person}'", "Person");
        }

        Person? person = uow.Repository().FindQueryable<Person>(p => p.Id == personGuid).FirstOrDefault();
        if(person == null)
        {
            throw new ArgumentException($"Person with given id was not found: '{request.Person}'", "Person");
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
                    Description = $"Created with '{newId}' activity object.",
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
        var added = uow.Repository().Add(newActivity);
        await uow.CommitAsync(cancellationToken);

        if(added != null) return added.ToDTO();
        else throw new Exception("Unknown error during saving.");
    }
}