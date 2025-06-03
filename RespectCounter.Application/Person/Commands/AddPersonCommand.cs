using System.Security;
using MediatR;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Commands;

public record AddPersonCommand(
    string FirstName, 
    string LastName, 
    string NickName,
    string Description, 
    string Nationality, 
    DateTime? Birthday, 
    DateTime? DeathDate, 
    string Tags,
    Guid UserId
) : IRequest<PersonDTO>;

public class AddPersonCommandHandler : IRequestHandler<AddPersonCommand, PersonDTO>
{
    private readonly IUnitOfWork uow;
    private readonly IUserService userService;

    public AddPersonCommandHandler(IUnitOfWork uow, IUserService userService)
    {
        this.uow = uow;
        this.userService = userService;
    }

    public async Task<PersonDTO> Handle(AddPersonCommand request, CancellationToken cancellationToken)
    {
        User? user = await userService.GetByIdAsync(request.UserId)
            ?? throw new SecurityException("Authentication issue. No user found.");

        DateOnly? birthday = request.Birthday.HasValue ? DateOnly.FromDateTime(request.Birthday.Value) : null;
        DateOnly? deathDate = request.DeathDate.HasValue ? DateOnly.FromDateTime(request.DeathDate.Value) : null;

        DateTime now = DateTime.Now;
        Person? newPerson = new()
        {
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
                    CreatedById = Guid.Empty,
                    LastUpdated = now,
                    LastUpdatedById = Guid.Empty
                };
                existingTag = uow.Repository().Add(newTag);
            }
            newPerson.Tags.Add(existingTag);
        }
        var result = uow.Repository().Add(newPerson);
        await uow.CommitAsync(cancellationToken);

        return result.ToDTO(request.UserId);
    }
}
