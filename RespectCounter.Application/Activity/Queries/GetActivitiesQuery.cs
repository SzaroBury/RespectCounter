using MediatR;
using RespectCounter.Infrastructure.Repositories;
using RespectCounter.Domain.Model;
using RespectCounter.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace RespectCounter.Application.Queries
{
    public record GetActivitesQuery(string Search, string Order, string Tag, List<ActivityStatus>? Status = null) : IRequest<List<ActivityQueryDTO>>;

    public class GetActivitesQueryHandler : IRequestHandler<GetActivitesQuery, List<ActivityQueryDTO>>
    {
        private readonly IUnitOfWork uow;

        public GetActivitesQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<List<ActivityQueryDTO>> Handle(GetActivitesQuery request, CancellationToken cancellationToken)
        {
            var status = request.Status == null || !request.Status.Any() ? [ActivityStatus.Verified, ActivityStatus.NotVerified] : request.Status!;
            IQueryable<Activity> activities = uow.Repository().FindQueryable<Activity>(
                a => status.Contains(a.Status)
            ).Include("Persons").Include("Comments").Include("Reactions").Include("Tags").Include("CreatedBy");

            if(!string.IsNullOrEmpty(request.Search))
            {
                var search = request.Search.ToLower();
                activities = activities.Where(a => a.Value.ToLower().Contains(search)
                            || a.Source.ToLower().Contains(search)
                            || a.Description.ToLower().Contains(search)
                            || a.Tags.Any(t => t.Name.ToLower().Contains(search))
                        );
            }

            if(!string.IsNullOrEmpty(request.Tag))
            {
                var tag = request.Tag.ToLower();
                activities = activities.Where(a => a.Tags.Any(t => t.Name.ToLower() == tag)
                            || a.Persons.Any(p => p.Tags.Any(pt => pt.Name.ToLower() == tag))
                        );
            }
            
            var order = string.IsNullOrEmpty(request.Order) ? "la" : request.Order;        
            var ordered = await RespectService.OrderActivitiesAsync(activities, order);
            var result = ordered.Select(
                a => RespectService.MapActivityToQueryDTO(a)
            );
            return result.ToList();
        }
    }
}