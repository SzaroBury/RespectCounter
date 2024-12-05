using System.Security;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Repositories;

namespace RespectCounter.Application.Commands
{
    public record AddReactionToActivityCommand(string ActivityId, int ReactionType, ClaimsPrincipal User) : IRequest<Activity>;

    public class AddReactionToActivityCommandHandler : IRequestHandler<AddReactionToActivityCommand, Activity>
    {
        private readonly IUnitOfWork uow;

        public AddReactionToActivityCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Activity> Handle(AddReactionToActivityCommand request, CancellationToken cancellationToken)
        {
            IdentityUser? user = await uow.UserManager.GetUserAsync(request.User);
            if(user == null) throw new SecurityException("Authentication issue. No user found.");

            DateTime now = DateTime.Now;
            Guid activityId;
            if(!Guid.TryParse(request.ActivityId, out activityId))  throw new ArgumentException("Invalid id format.");

            Activity? targetActivity = await uow.Repository().GetById<Activity>(activityId);
            if(targetActivity == null) throw new KeyNotFoundException("There is no person object with the given id value.");

            Reaction? reaction = uow.Repository().FindQueryable<Reaction>(r => r.PersonId == activityId && r.CreatedById == user.Id).FirstOrDefault();
            if(reaction != null) throw new InvalidOperationException("This user has already reacted to this person.");

            if(!Enum.IsDefined(typeof(ReactionType), request.ReactionType)) throw new ArgumentException("Invalid format of the reaction type.");

            reaction = new Reaction 
            {
                PersonId = activityId,
                ReactionType = (ReactionType) request.ReactionType,
                
                Created = now,
                CreatedById = user.Id,
                LastUpdated = now,
                LastUpdatedById = user.Id,
            };
            targetActivity.Reactions.Add(reaction);
            await uow.CommitAsync(cancellationToken);

            //cleaning data before sending it to the client
            reaction.CreatedBy = null;
            reaction.LastUpdatedBy = null;

            return targetActivity;
        }
    }
}