using System.Security;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Interfaces;

namespace RespectCounter.Application.Commands
{
    public record AddTagToActivityCommand(string ActivityId, string TagName, ClaimsPrincipal User) : IRequest<Activity>;

    public class AddTagToActivityCommandHandler : IRequestHandler<AddTagToActivityCommand, Activity>
    {
        private readonly IUnitOfWork uow;

        public AddTagToActivityCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Activity> Handle(AddTagToActivityCommand request, CancellationToken cancellationToken)
        {
            IdentityUser? user = await uow.UserManager.GetUserAsync(request.User);
            if(user == null) throw new SecurityException("Authentication issue. No user found.");

            DateTime now = DateTime.Now;
            Guid activityId;
            if(!Guid.TryParse(request.ActivityId, out activityId))  throw new ArgumentException("Invalid id format.");

            Activity? targetActivity = await uow.Repository().SingleOrDefaultAsync<Activity>(a => a.Id == activityId, "Tags");
            if(targetActivity == null) throw new KeyNotFoundException("There is no activity object with the given id value.");

            Tag? existingTag = uow.Repository().FindQueryable<Tag>(t => t.Name.ToLower() == request.TagName.ToLower()).FirstOrDefault();
            if(existingTag == null)
            {
                Tag newTag = new Tag 
                {
                    Name = request.TagName,
                    Description = $"Created for {targetActivity.Id} activity object.",
                    Level = 5,
                    
                    Created = now,
                    CreatedById = user.Id,
                    LastUpdated = now,
                    LastUpdatedById = user.Id
                };
                existingTag = uow.Repository().Add(newTag);
            }
            else if(targetActivity.Tags.Any(t => t.Name.ToLower() == existingTag.Name.ToLower()))
            {
                throw new InvalidOperationException("The pointed activity already has the given tag.");
            }
            targetActivity.Tags.Add(existingTag);

            await uow.CommitAsync(cancellationToken);

            //cleaning data before sending it to the client
            existingTag.CreatedBy = null;
            existingTag.LastUpdatedBy = null;

            return targetActivity;
        }
    }
}