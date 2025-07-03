using System.Security;
using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Services;

namespace RespectCounter.Application.Commands;

public record AddReactionToPersonCommand(Guid PersonId, int ReactionType, Guid UserId) : IRequest<int>;

public class AddReactionToPersonCommandHandler : IRequestHandler<AddReactionToPersonCommand, int>
{
    private readonly IUnitOfWork uow;
    private readonly IUserService userService;

    public AddReactionToPersonCommandHandler(IUnitOfWork uow, IUserService userService)
    {
        this.uow = uow;
        this.userService = userService;
    }

    public async Task<int> Handle(AddReactionToPersonCommand request, CancellationToken cancellationToken)
    {
        User? user = await userService.GetByIdAsync(request.UserId)
            ?? throw new SecurityException("Authentication issue. No user found.");

        Person? targetPerson = await uow.Repository().SingleOrDefaultAsync<Person>(p => p.Id == request.PersonId, "Reactions");
        if(targetPerson == null) throw new KeyNotFoundException("There is no person object with the given id value.");

        if(!Enum.IsDefined(typeof(ReactionType), request.ReactionType)) throw new ArgumentException("Invalid format of the reaction type.");

        Reaction? reaction = uow.Repository().FindQueryable<Reaction>(r => r.PersonId == request.PersonId && r.CreatedById == user.Id).FirstOrDefault();
        DateTime now = DateTime.Now;
        if(reaction != null) 
        {
            //throw new InvalidOperationException("This user has already reacted to this person.");
            reaction.ReactionType = (ReactionType) request.ReactionType;
            reaction.LastUpdated = now;
            uow.Repository().Update(reaction);
        }
        else
        {
            reaction = new Reaction 
            {
                PersonId = request.PersonId,
                ReactionType = (ReactionType) request.ReactionType,
                
                Created = now,
                CreatedById = user.Id,
                LastUpdated = now,
                LastUpdatedById = user.Id,
            };
            targetPerson.Reactions.Add(reaction);
        }

        await uow.CommitAsync(cancellationToken);
        var result = targetPerson.Reactions.Sum(r => (int)r.ReactionType);
        return result;
    }
}