using System.Security;
using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Commands
{
    public record AddCommentToParentCommentCommand(string ParentCommentId, string Content, Guid UserId) : IRequest<Comment>;

    public class AddCommentToParentCommentCommandHandler : IRequestHandler<AddCommentToParentCommentCommand, Comment>
    {
        private readonly IUnitOfWork uow;
        private readonly IUserService userService;

        public AddCommentToParentCommentCommandHandler(IUnitOfWork uow, IUserService userService)
        {
            this.uow = uow;
            this.userService = userService;
        }

        public async Task<Comment> Handle(AddCommentToParentCommentCommand request, CancellationToken cancellationToken)
        {
            User? user = await userService.GetByIdAsync(request.UserId);
            if(user == null) throw new SecurityException("Authentication issue. No user found.");

            DateTime now = DateTime.Now;
            Guid parentCommentId;
            if(!Guid.TryParse(request.ParentCommentId, out parentCommentId))  throw new ArgumentException("Invalid id format.");

            Comment? targetComment = await uow.Repository().FindByIdAsync<Comment>(parentCommentId);
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