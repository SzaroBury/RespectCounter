using System.Security;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Repositories;

namespace RespectCounter.Application.Commands
{
    public record AddCommentToActivityCommand(string ActivityId, string Content, ClaimsPrincipal User) : IRequest<Activity>;

    public class AddCommentToActivityCommandHandler : IRequestHandler<AddCommentToActivityCommand, Activity>
    {
        private readonly IUnitOfWork uow;

        public AddCommentToActivityCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Activity> Handle(AddCommentToActivityCommand request, CancellationToken cancellationToken)
        {
            IdentityUser? user = await uow.UserManager.GetUserAsync(request.User);
            if(user == null) throw new SecurityException("Authentication issue. No user found.");

            DateTime now = DateTime.Now;
            Guid activityId;
            if(!Guid.TryParse(request.ActivityId, out activityId))  throw new ArgumentException("Invalid id format.");

            Activity? targetActivity = await uow.Repository().GetById<Activity>(activityId);
            if(targetActivity == null) throw new KeyNotFoundException("There is no person object with the given id value.");

            Comment comment = new Comment 
            {
                PersonId = activityId,
                Content = request.Content,
                
                Created = now,
                CreatedById = user.Id,
                LastUpdated = now,
                LastUpdatedById = user.Id,
            };
            targetActivity.Comments.Add(comment);
            await uow.CommitAsync(cancellationToken);

            //cleaning data before sending it to the client
            comment.CreatedBy = null;
            comment.LastUpdatedBy = null;

            return targetActivity;
        }
    }
}