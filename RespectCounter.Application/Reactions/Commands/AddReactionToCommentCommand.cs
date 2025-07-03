using System.Security;
using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Services;

namespace RespectCounter.Application.Commands
{
    public record AddReactionToCommentCommand(Guid CommentId, int ReactionType, Guid UserId) : IRequest<int>;

    public class AddReactionToCommentCommandHandler : IRequestHandler<AddReactionToCommentCommand, int>
    {
        private readonly IUnitOfWork uow;
        private readonly IUserService userService;

        public AddReactionToCommentCommandHandler(IUnitOfWork uow, IUserService userService)
        {
            this.uow = uow;
            this.userService = userService;
        }

        public async Task<int> Handle(AddReactionToCommentCommand request, CancellationToken cancellationToken)
        {
            User? user = await userService.GetByIdAsync(request.UserId)
                ?? throw new SecurityException("Authentication issue. No user found.");

            DateTime now = DateTime.Now;

            Comment? targetComment = await uow.Repository().SingleOrDefaultAsync<Comment>(c => c.Id == request.CommentId, "Reactions");
            if(targetComment == null) throw new KeyNotFoundException("There is no comment with the given id value.");

            if(!Enum.IsDefined(typeof(ReactionType), request.ReactionType)) throw new ArgumentException("Invalid format of the reaction type.");
            
            Reaction? reaction = uow.Repository().FindQueryable<Reaction>(r => r.CommentId == request.CommentId && r.CreatedById == user.Id).FirstOrDefault();
            if(reaction != null)
            {
                //throw new InvalidOperationException("This user has already reacted to this comment.");
                reaction.ReactionType = (ReactionType) request.ReactionType;
                reaction.LastUpdated = now;
                uow.Repository().Update(reaction);
            }
            else
            {
                reaction = new Reaction 
                {
                    CommentId = request.CommentId,
                    ReactionType = (ReactionType) request.ReactionType,
                    
                    Created = now,
                    CreatedById = user.Id,
                    LastUpdated = now,
                    LastUpdatedById = user.Id,
                };
                targetComment.Reactions.Add(reaction);
            }

            await uow.CommitAsync(cancellationToken);
            var result = targetComment.Reactions.Sum(r => (int)r.ReactionType);
            return result;
        }
    }
}