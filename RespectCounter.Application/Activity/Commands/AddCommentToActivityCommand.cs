using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Commands;

public record AddCommentToActivityCommand(string ActivityId, string Content, Guid UserId) : IRequest<Activity>;

public class AddCommentToActivityCommandHandler : IRequestHandler<AddCommentToActivityCommand, Activity>
{
    private readonly IUnitOfWork uow;

    public AddCommentToActivityCommandHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<Activity> Handle(AddCommentToActivityCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        // DateTime now = DateTime.Now;
        // Guid activityId;
        // if(!Guid.TryParse(request.ActivityId, out activityId))  throw new ArgumentException("Invalid id format.");

        // Activity? targetActivity = await uow.Repository().FindByIdAsync<Activity>(activityId);
        // if(targetActivity == null) throw new KeyNotFoundException("There is no activity object with the given id value.");

        // Comment comment = new()
        // {
        //     PersonId = activityId,
        //     Content = request.Content,
            
        //     Created = now,
        //     CreatedById = request.UserId,
        //     LastUpdated = now,
        //     LastUpdatedById = request.UserId,
        // };
        // targetActivity.Comments.Add(comment);
        // await uow.CommitAsync(cancellationToken);

        // //cleaning data before sending it to the client
        // comment.CreatedBy = null;
        // comment.LastUpdatedBy = null;

        // return targetActivity;
    }
}