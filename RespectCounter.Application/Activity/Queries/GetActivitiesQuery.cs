using MediatR;
using RespectCounter.Infrastructure.Repositories;
using RespectCounter.Domain.Model;
using RespectCounter.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace RespectCounter.Application.Queries
{
    public record GetActivitesQuery(string Search, string Order, string Tags, List<ActivityStatus>? Status = null) : IRequest<List<ActivityDTO>>;

    public class GetActivitesQueryHandler : IRequestHandler<GetActivitesQuery, List<ActivityDTO>>
    {
        private readonly IUnitOfWork uow;

        public GetActivitesQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<List<ActivityDTO>> Handle(GetActivitesQuery request, CancellationToken cancellationToken)
        {
            var status = request.Status == null || !request.Status.Any() ? [ActivityStatus.Verified, ActivityStatus.NotVerified] : request.Status!;
            IQueryable<Activity> activities = uow.Repository().FindQueryable<Activity>(
                a => status.Contains(a.Status)
            ).Include(a => a.Person)
            .Include(a => a.Comments).ThenInclude(c => c.Children)
            .Include(a => a.Reactions)
            .Include(a => a.Tags)
            .Include(a => a.CreatedBy);

            if(!string.IsNullOrEmpty(request.Search))
            {
                var search = request.Search.ToLower();
                activities = activities.Where(a =>
                    (a.Value != null && a.Value.ToLower().Contains(search)) ||
                    (a.Source != null && a.Source.ToLower().Contains(search)) ||
                    (a.Description != null && a.Description.ToLower().Contains(search)) ||
                    a.Tags.Any(t => t.Name != null && t.Name.ToLower().Contains(search))
                );
            }

            IEnumerable<Activity> result = await activities.ToListAsync(cancellationToken);
            
            if(!string.IsNullOrEmpty(request.Tags))
            {
                var tags = request.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim().ToLower());
                result = result.Where(a => tags.All(tag => a.Tags.Any(at => at.Name.ToLower() == tag)));
            }
            
            var order = string.IsNullOrEmpty(request.Order) ? "la" : request.Order;  
            var orderedResult = RespectService.OrderActivities(result, order);

            return orderedResult.Select(a => a.ToDTO()).ToList();
        }
    }
}