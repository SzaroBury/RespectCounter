using System.Security;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Interfaces;

namespace RespectCounter.Application.Commands
{
    public record AddTagToPersonCommand() : IRequest<Person>
    {
        public required string PersonId { get; set; }
        public required string TagName { get; set; }
        public required ClaimsPrincipal User { get; set; }
    }

    public class AddTagToPersonCommandHandler : IRequestHandler<AddTagToPersonCommand, Person>
    {
        private readonly IUnitOfWork uow;

        public AddTagToPersonCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Person> Handle(AddTagToPersonCommand request, CancellationToken cancellationToken)
        {
            IdentityUser? user = await uow.UserManager.GetUserAsync(request.User);
            if(user == null) throw new SecurityException("Authentication issue. No user found.");

            DateTime now = DateTime.Now;
            Guid personId;
            if(!Guid.TryParse(request.PersonId, out personId))  throw new ArgumentException("Invalid id format.");

            Person? targetPerson = await uow.Repository().SingleOrDefaultAsync<Person>(p => p.Id == personId, "Tags");
            if(targetPerson == null) throw new KeyNotFoundException("There is no person object with the given id value.");

            Tag? existingTag = uow.Repository().FindQueryable<Tag>(t => t.Name.ToLower() == request.TagName.ToLower()).FirstOrDefault();
            if(existingTag == null)
            {
                Tag newTag = new Tag 
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

            return targetPerson;
        }
    }
}