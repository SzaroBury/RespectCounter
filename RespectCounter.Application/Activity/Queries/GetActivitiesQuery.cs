using MediatR;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Model;
using RespectCounter.Application.DTOs;

namespace RespectCounter.Application.Queries
{
    public record GetActivitiesQuery(
        string Search, 
        string Order, 
        string Tags, 
        Guid? UserId, 
        List<ActivityStatus>? Status = null
    ) : IRequest<List<ActivityDTO>>;

    public class GetActivitesQueryHandler : IRequestHandler<GetActivitiesQuery, List<ActivityDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IUserService userService;

        public GetActivitesQueryHandler(IUnitOfWork uow, IUserService userService)
        {
            this.uow = uow;
            this.userService = userService;
        }

        public async Task<List<ActivityDTO>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // IEnumerable<ActivityStatus> statuses;
            // if (request.Status == null || request.Status.Count == 0)
            // {
            //     statuses = [ActivityStatus.Verified, ActivityStatus.NotVerified];
            // }
            // else
            // {
            //     statuses = request.Status!;
            // }

            // IQueryable<Activity> activities = uow.Repository().FindQueryable<Activity>(
            //     a => statuses.Contains(a.Status)
            // ).Include(a => a.Person)
            // .Include(a => a.Comments).ThenInclude(c => c.Children)
            // .Include(a => a.Reactions)
            // .Include(a => a.Tags)
            // .Include(a => a.CreatedBy);

            // if(!string.IsNullOrEmpty(request.Search))
            // {
            //     var search = request.Search.ToLower();
            //     activities = activities.Where(a =>
            //         (a.Value != null && a.Value.ToLower().Contains(search)) ||
            //         (a.Source != null && a.Source.ToLower().Contains(search)) ||
            //         (a.Description != null && a.Description.ToLower().Contains(search)) ||
            //         a.Tags.Any(t => t.Name != null && t.Name.ToLower().Contains(search))
            //     );
            // }

            // IEnumerable<Activity> result = await activities.ToListAsync(cancellationToken);
            
            // if(!string.IsNullOrEmpty(request.Tags))
            // {
            //     var tags = request.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim().ToLower());
            //     result = result.Where(a => tags.All(tag => a.Tags.Any(at => at.Name.ToLower() == tag)));
            // }
            
            // var order = string.IsNullOrEmpty(request.Order) ? "la" : request.Order;  
            // var orderedResult = RespectService.OrderActivities(result, order);

            // string? userGuid = null;
            // if(request.UserId.HasValue)
            // {
            //     User? user = await userService.GetByIdAsync(request.UserId.Value);
            //     userGuid = user?.Id.ToString();
            // }

            // return orderedResult.Select(a => a.ToDTO(userGuid)).ToList();
        }
    }
}