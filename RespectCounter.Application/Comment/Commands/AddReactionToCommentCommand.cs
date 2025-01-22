using System.Security;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Repositories;

namespace RespectCounter.Application.Commands
{
    public record AddReactionToCommentCommand(string CommentId, int ReactionType, ClaimsPrincipal User) : IRequest<Comment>;

    public class AddReactionToCommentCommandHandler : IRequestHandler<AddReactionToCommentCommand, Comment>
    {
        private readonly IUnitOfWork uow;

        public AddReactionToCommentCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Comment> Handle(AddReactionToCommentCommand request, CancellationToken cancellationToken)
        {
            IdentityUser? user = await uow.UserManager.GetUserAsync(request.User);
            if(user == null) throw new SecurityException("Authentication issue. No user found.");

            DateTime now = DateTime.Now;
            Guid commentId;
            if(!Guid.TryParse(request.CommentId, out commentId))  throw new ArgumentException("Invalid id format.");

            Comment? targetComment = await uow.Repository().GetById<Comment>(commentId);
            if(targetComment == null) throw new KeyNotFoundException("There is no comment with the given id value.");

            Reaction? reaction = uow.Repository().FindQueryable<Reaction>(r => r.CommentId == commentId && r.CreatedById == user.Id).FirstOrDefault();
            if(reaction != null) throw new InvalidOperationException("This user has already reacted to this comment.");

            if(!Enum.IsDefined(typeof(ReactionType), request.ReactionType)) throw new ArgumentException("Invalid format of the reaction type.");

            reaction = new Reaction 
            {
                CommentId = commentId,
                ReactionType = (ReactionType) request.ReactionType,
                
                Created = now,
                CreatedById = user.Id,
                LastUpdated = now,
                LastUpdatedById = user.Id,
            };
            targetComment.Reactions.Add(reaction);
            await uow.CommitAsync(cancellationToken);

            //cleaning data before sending it to the client
            reaction.CreatedBy = null;
            reaction.LastUpdatedBy = null;

            return targetComment;
        }
    }
}