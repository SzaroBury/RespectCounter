using System.Security;
using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Commands;

public record AddReactionToActivityCommand(Guid ActivityId, int ReactionType, Guid UserId) : IRequest<int>;

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

        Activity? targetActivity = await uow.Repository().SingleOrDefaultAsync<Activity>(
            a => a.Id == request.ActivityId,
            "Reactions",
            cancellationToken)
            ?? throw new ArgumentException("There is no activity object with the given id value.", "activityId");

        if (!Enum.IsDefined(typeof(ReactionType), request.ReactionType))
        {
            throw new ArgumentException("Invalid format of the reaction type.", "reaction");
        }

        Reaction? reaction = uow.Repository().FindQueryable<Reaction>(r => r.ActivityId == request.ActivityId && r.CreatedById == user.Id).FirstOrDefault();

        DateTime now = DateTime.Now;
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
                ActivityId = request.ActivityId,
                ReactionType = (ReactionType) request.ReactionType,
                
                Created = now,
                CreatedById = user.Id,
                LastUpdated = now,
                LastUpdatedById = user.Id,
            };
            targetActivity.Reactions.Add(reaction);
        }
        await uow.CommitAsync(cancellationToken);

        var result = targetActivity.Reactions.Sum(r => (int)r.ReactionType);

        return result;
    }
}