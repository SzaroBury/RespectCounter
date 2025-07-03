using MediatR;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Model;
using RespectCounter.Application.DTOs;
using RespectCounter.Application.Common;
using RespectCounter.Application.Services;

namespace RespectCounter.Application.Queries;

public record GetActivitiesByPersonQuery(
    Guid PersonId,
    Guid? UserId, 
    ActivitySortBy? Order = null, 
    ActivityType? Type = null, 
    List<ActivityStatus>? Status = null
) : IRequest<IEnumerable<ActivityDTO>>;

public class GetActivitiesByPersonQueryHandler : IRequestHandler<GetActivitiesByPersonQuery, IEnumerable<ActivityDTO>>
{
    private readonly IUnitOfWork uow;
    private readonly IUserService userService;

    public GetActivitiesByPersonQueryHandler(IUnitOfWork uow, IUserService userService)
    {
        this.uow = uow;
        this.userService = userService;
    }

    public async Task<IEnumerable<ActivityDTO>> Handle(GetActivitiesByPersonQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<ActivityStatus> statusFilter;
        if (request.Status == null || request.Status.Count == 0)
        {
            statusFilter = [ActivityStatus.Verified, ActivityStatus.NotVerified];
        }
        else
        {
            statusFilter = request.Status!;
        }

        var query = uow.Repository().FindQueryable<Activity>(
            a => a.PersonId == request.PersonId && statusFilter.Contains(a.Status)
        );

        if(request.Type.HasValue)
        {
            query = query.Where(a => a.Type == request.Type);
        }

        var order = request.Order ?? ActivitySortBy.LatestAdded;
        var orderedQuery = query.ApplySorting(order);

        Guid? userGuid = null;
        if(request.UserId.HasValue)
        {
            User? user = await userService.GetByIdAsync(request.UserId.Value);
            userGuid = user?.Id;
        }

        var activities = await uow.Repository()
            .FindListAsync(
                orderedQuery,
                ["Person", "Comments.Children", "Reactions", "Tags"],
                null,
                cancellationToken
            );

        foreach (var act in activities)
        {
            act.CreatedBy = await userService.GetByIdAsync(act.CreatedById);
        }

        return activities.Select(a => a.ToDTO(userGuid));
    }
}