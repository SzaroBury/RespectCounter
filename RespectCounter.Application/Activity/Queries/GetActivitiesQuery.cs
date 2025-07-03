using MediatR;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Model;
using RespectCounter.Application.DTOs;
using RespectCounter.Application.Common;
using RespectCounter.Application.Services;

namespace RespectCounter.Application.Queries;

public record GetActivitiesQuery(
    string Search, 
    ActivitySortBy Order, 
    string Tags, 
    Guid? UserId, 
    List<ActivityStatus>? Status = null
) : IRequest<IEnumerable<ActivityDTO>>;

public class GetActivitiesQueryHandler : IRequestHandler<GetActivitiesQuery, IEnumerable<ActivityDTO>>
{
    private readonly IUnitOfWork uow;
    private readonly IUserService userService;

    public GetActivitiesQueryHandler(IUnitOfWork uow, IUserService userService)
    {
        this.uow = uow;
        this.userService = userService;
    }

    public async Task<IEnumerable<ActivityDTO>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
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

        IQueryable<Activity> query = uow.Repository().FindQueryable<Activity>(
            a => statusFilter.Contains(a.Status)
        );

        if(!string.IsNullOrEmpty(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(a =>
                (a.Value != null && a.Value.ToLower().Contains(search)) ||
                (a.Source != null && a.Source.ToLower().Contains(search)) ||
                (a.Description != null && a.Description.ToLower().Contains(search)) ||
                a.Tags.Any(t => t.Name != null && t.Name.ToLower().Contains(search))
            );
        }
        
        if(!string.IsNullOrEmpty(request.Tags))
        {
            var tags = request.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim().ToLower());
            query = query.Where(
                a => tags.All(
                    tag => a.Tags.Any(
                        at => at.Name.Equals(tag, StringComparison.CurrentCultureIgnoreCase)
                    )
                )
            );
        }

        var orderedQuery = query.ApplySorting(request.Order);

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