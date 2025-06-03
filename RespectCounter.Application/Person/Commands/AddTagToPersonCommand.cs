using System.Security;
using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Commands;

public record AddTagToPersonCommand(Guid PersonId, string TagName, Guid UserId) : IRequest<Person>;

public class AddTagToPersonCommandHandler : IRequestHandler<AddTagToPersonCommand, Person>
{
    private readonly IUnitOfWork uow;
    private readonly IUserService userService;

    public AddTagToPersonCommandHandler(IUnitOfWork uow, IUserService userService)
    {
        this.uow = uow;
        this.userService = userService;
    }

    public async Task<Person> Handle(AddTagToPersonCommand request, CancellationToken cancellationToken)
    {
        User? user = await userService.GetByIdAsync(request.UserId)
            ?? throw new SecurityException("Authentication issue. No user found.");

        Person? targetPerson = await uow.Repository().SingleOrDefaultAsync<Person>(p => p.Id == request.PersonId, "Tags")
            ?? throw new KeyNotFoundException("There is no person object with the given id value.");

        Tag? existingTag = uow.Repository().FindQueryable<Tag>(t => t.Name.ToLower() == request.TagName.ToLower()).FirstOrDefault();
        if(existingTag == null)
        {
            var now = DateTime.Now;
            Tag newTag = new()
            {
                Name = request.TagName,
                Description = $"Created for {targetPerson.FirstName} {targetPerson.LastName} person object.",
                Level = 5,
                
                Created = now,
                CreatedById = user.Id,
                LastUpdated = now,
                LastUpdatedById = user.Id
            };
            existingTag = uow.Repository().Add(newTag);
        }
        else if(targetPerson.Tags.Any(t => t.Name.ToLower() == existingTag.Name.ToLower()))
        {
            throw new InvalidOperationException("The pointed person already has the given tag.");
        }
        targetPerson.Tags.Add(existingTag);

        await uow.CommitAsync(cancellationToken);

        //cleaning data before sending it to the client
        existingTag.CreatedBy = null;
        existingTag.LastUpdatedBy = null;

        return targetPerson; //toDTO
    }
}