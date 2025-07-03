using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.DTOs;
using RespectCounter.Application.Services;

namespace RespectCounter.Application.Commands;

public record AddCommentToActivityCommand(Guid ActivityId, string Content, Guid UserId) : IRequest<ActivityDTO>;

public class AddCommentToActivityCommandHandler : IRequestHandler<AddCommentToActivityCommand, ActivityDTO>
{
    private readonly IUnitOfWork uow;

    public AddCommentToActivityCommandHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<ActivityDTO> Handle(AddCommentToActivityCommand request, CancellationToken cancellationToken)
    {
        Activity? targetActivity = await uow.Repository().FindByIdAsync<Activity>(request.ActivityId, cancellationToken)
            ?? throw new KeyNotFoundException("There is no activity object with the given id value.");

        DateTime now = DateTime.Now;
        Comment comment = new()
        {
            ActivityId = request.ActivityId,
            Content = request.Content,
            
            Created = now,
            CreatedById = request.UserId,
            LastUpdated = now,
            LastUpdatedById = request.UserId,
        };
        targetActivity.Comments.Add(comment);
        await uow.CommitAsync(cancellationToken);

        //cleaning data before sending it to the client
        // comment.CreatedBy = null;
        // comment.LastUpdatedBy = null;

        return targetActivity.ToDTO(request.UserId);
    }
}