using System.Security;
using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Commands
{
    public record AddReactionToActivityCommand(string ActivityId, int ReactionType, Guid UserId) : IRequest<int>;

    public class AddReactionToActivityCommandHandler : IRequestHandler<AddReactionToActivityCommand, int>
    {
        private readonly IUnitOfWork uow;
        private readonly IUserService userService;

        public AddReactionToActivityCommandHandler(IUnitOfWork uow, IUserService userService)
        {
            this.uow = uow;
            this.userService = userService;
        }

        public async Task<int> Handle(AddReactionToActivityCommand request, CancellationToken cancellationToken)
        {
            User? user = await userService.GetByIdAsync(request.UserId) ?? throw new SecurityException("Authentication issue. No user found.");
            DateTime now = DateTime.Now;
            Guid activityId;
            if(!Guid.TryParse(request.ActivityId, out activityId))  throw new ArgumentException("Invalid id format.", "activityId");

            Activity? targetActivity = await uow.Repository().SingleOrDefaultAsync<Activity>(a => a.Id == activityId, "Reactions");
            if(targetActivity == null) throw new ArgumentException("There is no activity object with the given id value.", "activityId");

            if(!Enum.IsDefined(typeof(ReactionType), request.ReactionType)) throw new ArgumentException("Invalid format of the reaction type.", "reaction");

            Reaction? reaction = uow.Repository().FindQueryable<Reaction>(r => r.ActivityId == activityId && r.CreatedById == user.Id).FirstOrDefault();
            if(reaction != null)
            { 
                // throw new InvalidOperationException("This user has already reacted to this activity.");
                reaction.ReactionType = (ReactionType) request.ReactionType;
                reaction.LastUpdated = now;
                uow.Repository().Update(reaction);
            }
            else
            {
                reaction = new Reaction 
                {
                    ActivityId = activityId,
                    ReactionType = (ReactionType) request.ReactionType,
                    
                    Created = now,
                    CreatedById = user.Id,
                    LastUpdated = now,
                    LastUpdatedById = user.Id,
                };
                targetActivity.Reactions.Add(reaction);
            }
       
            await uow.CommitAsync(cancellationToken);
            return RespectService.CountRespect(targetActivity.Reactions);
        }
    }
}