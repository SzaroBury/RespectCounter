using System.Security;
using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.DTOs;
using RespectCounter.Application.Services;

namespace RespectCounter.Application.Commands
{
    public record AddTagToActivityCommand(Guid ActivityId, string TagName, Guid UserId) : IRequest<ActivityDTO>;

    public class AddTagToActivityCommandHandler : IRequestHandler<AddTagToActivityCommand, ActivityDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IUserService userService;

        public AddTagToActivityCommandHandler(IUnitOfWork uow, IUserService userService)
        {
            this.uow = uow;
            this.userService = userService;
        }

        public async Task<ActivityDTO> Handle(AddTagToActivityCommand request, CancellationToken cancellationToken)
        {
            User? user = await userService.GetByIdAsync(request.UserId)
                ?? throw new SecurityException("Authentication issue. No user found.");

            Activity? targetActivity = await uow.Repository().SingleOrDefaultAsync<Activity>(
                a => a.Id == request.ActivityId,
                "Tags",
                cancellationToken
            ) ?? throw new KeyNotFoundException("There is no activity object with the given id value.");
                
            Tag? existingTag = uow.Repository().FindQueryable<Tag>(
                t => t.Name.Equals(request.TagName, StringComparison.CurrentCultureIgnoreCase)
            ).FirstOrDefault();
            
            if (existingTag == null)
            {
                DateTime now = DateTime.Now;
                Tag newTag = new()
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
            else if (targetActivity.Tags.Any(t => t.Name.ToLower() == existingTag.Name.ToLower()))
            {
                throw new InvalidOperationException("The pointed activity already has the given tag.");
            }
            targetActivity.Tags.Add(existingTag);

            await uow.CommitAsync(cancellationToken);

            //cleaning data before sending it to the client
            // existingTag.CreatedBy = null;
            // existingTag.LastUpdatedBy = null;

            return targetActivity.ToDTO(request.UserId);
        }
    }
}