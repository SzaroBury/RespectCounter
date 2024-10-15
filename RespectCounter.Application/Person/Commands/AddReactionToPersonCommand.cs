using System.Security;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Repositories;

namespace RespectCounter.Application.Commands
{
    public record AddReactionToPersonCommand() : IRequest<Person>
    {
        public required string PersonId { get; set; }
        public required int ReactionType { get; set; }
        public required ClaimsPrincipal User { get; set; }
    }

    public class AddReactionToPersonCommandHandler : IRequestHandler<AddReactionToPersonCommand, Person>
    {
        private readonly IUnitOfWork uow;

        public AddReactionToPersonCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Person> Handle(AddReactionToPersonCommand request, CancellationToken cancellationToken)
        {
            IdentityUser? user = await uow.UserManager.GetUserAsync(request.User);
            if(user == null) throw new SecurityException("Authentication issue. No user found.");

            DateTime now = DateTime.Now;
            Guid personId;
            if(!Guid.TryParse(request.PersonId, out personId))  throw new ArgumentException("Invalid id format.");

            Person? targetPerson = await uow.Repository().GetById<Person>(personId);
            if(targetPerson == null) throw new KeyNotFoundException("There is no person object with the given id value.");

            Reaction? reaction = uow.Repository().FindQueryable<Reaction>(r => r.PersonId == personId && r.CreatedById == user.Id).FirstOrDefault();
            if(reaction != null) throw new InvalidOperationException("This user has already reacted to this person.");

            if(!Enum.IsDefined(typeof(ReactionType), request.ReactionType)) throw new ArgumentException("Invalid format of the reaction type.");

            reaction = new Reaction 
            {
                PersonId = personId,
                ReactionType = (ReactionType) request.ReactionType,
                
                Created = now,
                CreatedById = user.Id,
                LastUpdated = now,
                LastUpdatedById = user.Id,
            };
            targetPerson.Reactions.Add(reaction);
            await uow.CommitAsync(cancellationToken);

            //cleaning data before sending it to the client
            reaction.CreatedBy = null;
            reaction.LastUpdatedBy = null;

            return targetPerson;
        }
    }
}