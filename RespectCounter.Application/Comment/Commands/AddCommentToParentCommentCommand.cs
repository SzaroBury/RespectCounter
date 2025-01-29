using System.Security;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Interfaces;

namespace RespectCounter.Application.Commands
{
    public record AddCommentToParentCommentCommand(string ParentCommentId, string Content, ClaimsPrincipal User) : IRequest<Comment>;

    public class AddCommentToParentCommentCommandHandler : IRequestHandler<AddCommentToParentCommentCommand, Comment>
    {
        private readonly IUnitOfWork uow;

        public AddCommentToParentCommentCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Comment> Handle(AddCommentToParentCommentCommand request, CancellationToken cancellationToken)
        {
            IdentityUser? user = await uow.UserManager.GetUserAsync(request.User);
            if(user == null) throw new SecurityException("Authentication issue. No user found.");

            DateTime now = DateTime.Now;
            Guid parentCommentId;
            if(!Guid.TryParse(request.ParentCommentId, out parentCommentId))  throw new ArgumentException("Invalid id format.");

            Comment? targetComment = await uow.Repository().GetById<Comment>(parentCommentId);
            if(targetComment == null) throw new KeyNotFoundException("There is no comment with the given id value.");

            Comment comment = new Comment 
            {
                ParentId = parentCommentId,
                Content = request.Content,
                
                Created = now,
                CreatedById = user.Id,
                LastUpdated = now,
                LastUpdatedById = user.Id,
            };
            targetComment.Children.Add(comment);
            await uow.CommitAsync(cancellationToken);

            //cleaning data before sending it to the client
            comment.CreatedBy = null;
            comment.LastUpdatedBy = null;

            return targetComment;
        }
    }
}