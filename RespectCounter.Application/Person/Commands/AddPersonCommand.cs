using System.Globalization;
using System.Security;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Repositories;

namespace RespectCounter.Application.Commands;

public record AddPersonCommand(
    string FirstName, 
    string LastName, 
    string NickName,
    string Description, 
    string Nationality, 
    string? Birthday, 
    string? DeathDate, 
    string Tags,
    ClaimsPrincipal User
) : IRequest<PersonDTO>;

public class AddPersonCommandHandler : IRequestHandler<AddPersonCommand, PersonDTO>
{
    private readonly IUnitOfWork uow;

    public AddPersonCommandHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<PersonDTO> Handle(AddPersonCommand request, CancellationToken cancellationToken)
    {
        DateTime now = DateTime.Now;
        Guid newId = Guid.NewGuid();
        DateOnly? birthday = null;
        DateOnly? deathDate = null;

        IdentityUser? user = await uow.UserManager.GetUserAsync(request.User);
        if(user == null) throw new SecurityException("Authentication issue. No user found.");

        if(!string.IsNullOrEmpty(request.Birthday))
        {
            if(!DateOnly.TryParseExact(request.Birthday, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate))
            {
                throw new ArgumentException("Invalid date format. Expected format: yyyy-MM-dd", "Birthday");
            }
            birthday = parsedDate;
        }

        if(!string.IsNullOrEmpty(request.DeathDate))
        {
            if(!DateOnly.TryParseExact(request.DeathDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate))
            {
                throw new ArgumentException("Invalid date format. Expected format: yyyy-MM-dd", "DeathDate");
            }
            deathDate = parsedDate;
        }

        Person? newPerson = new Person
        {
            Id = newId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            NickName = request.NickName,
            Description = request.Description,
            Nationality = request.Nationality,
            Birthday = birthday,
            DeathDate = deathDate,

            Status = PersonStatus.NotVerified,
            Created = now,
            CreatedById = user.Id,
            LastUpdated = now,
            LastUpdatedById = user.Id
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
        var result = uow.Repository().Add(newPerson);
        await uow.CommitAsync(cancellationToken);

        if(newPerson != null) return result.ToDTO();
        else throw new Exception("Unknown error during saving.");
    }
}
