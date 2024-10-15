using System.Security;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Repositories;

namespace RespectCounter.Application.Commands
{
    public record AddCommentToPersonCommand() : IRequest<Person>
    {
        public required string PersonId { get; set; }
        public required string Content { get; set; }
        public required ClaimsPrincipal User { get; set; }
    }

    public class AddCommentToPersonCommandHandler : IRequestHandler<AddCommentToPersonCommand, Person>
    {
        private readonly IUnitOfWork uow;

        public AddCommentToPersonCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Person> Handle(AddCommentToPersonCommand request, CancellationToken cancellationToken)
        {
            IdentityUser? user = await uow.UserManager.GetUserAsync(request.User);
            if(user == null) throw new SecurityException("Authentication issue. No user found.");

            DateTime now = DateTime.Now;
            Guid personId;
            if(!Guid.TryParse(request.PersonId, out personId))  throw new ArgumentException("Invalid id format.");

            Person? targetPerson = await uow.Repository().GetById<Person>(personId);
            if(targetPerson == null) throw new KeyNotFoundException("There is no person object with the given id value.");

            Comment comment = new Comment 
            {
                PersonId = personId,
                Content = request.Content,
                
                Created = now,
                CreatedById = user.Id,
                LastUpdated = now,
                LastUpdatedById = user.Id,
            };
            targetPerson.Comments.Add(comment);
            await uow.CommitAsync(cancellationToken);

            //cleaning data before sending it to the client
            comment.CreatedBy = null;
            comment.LastUpdatedBy = null;

            return targetPerson;
        }
    }
}