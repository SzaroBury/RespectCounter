using System.Security;
using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.DTOs;
using RespectCounter.Application.Services;

namespace RespectCounter.Application.Commands
{
    public record AddCommentToParentCommentCommand(Guid ParentCommentId, string Content, Guid UserId) : IRequest<CommentDTO>;

    public class AddCommentToParentCommentCommandHandler : IRequestHandler<AddCommentToParentCommentCommand, CommentDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IUserService userService;

        public AddCommentToParentCommentCommandHandler(IUnitOfWork uow, IUserService userService)
        {
            this.uow = uow;
            this.userService = userService;
        }

        public async Task<CommentDTO> Handle(AddCommentToParentCommentCommand request, CancellationToken cancellationToken)
        {
            await uow.BeginTransactionAsync(cancellationToken);
            try
            {
                User? user = await userService.GetByIdAsync(request.UserId)
                    ?? throw new SecurityException("Authentication issue. No user found.");

                Comment parentComment = await uow.Repository().SingleOrDefaultAsync<Comment>(
                    comment => comment.Id == request.ParentCommentId,
                    "Parent",
                    cancellationToken
                ) ?? throw new KeyNotFoundException("There is no comment with the given id value.");

                parentComment.DirectChildrenCount++;
                parentComment.AllChildrenCount++;

                DateTime now = DateTime.Now;
                Comment comment = new()
                {
                    ParentId = parentComment.Id,
                    Content = request.Content,

                    Created = now,
                    CreatedById = user.Id,
                    LastUpdated = now,
                    LastUpdatedById = user.Id,
                };

                parentComment.Children.Add(comment);
                uow.Repository().Update(parentComment);
                uow.Repository().Add(comment);

                Comment? nextParentComment = parentComment.Parent;
                while (nextParentComment != null)
                {
                    nextParentComment.AllChildrenCount++;
                    uow.Repository().Update(nextParentComment);
                    nextParentComment = await uow.Repository().SingleOrDefaultAsync<Comment>(
                        comment => comment.Id == parentComment.ParentId,
                        cancellationToken: cancellationToken
                    );
                }

                await uow.CommitTransactionAsync(cancellationToken);
                return parentComment.ToDTO(1, user.Id);
            }
            catch
            {
                await uow.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}