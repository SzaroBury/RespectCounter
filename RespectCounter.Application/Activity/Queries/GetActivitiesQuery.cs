using MediatR;
using RespectCounter.Infrastructure.Repositories;
using RespectCounter.Domain.Model;
using Microsoft.IdentityModel.Tokens;

namespace RespectCounter.Application.Queries
{
    public record GetActivitesQuery(string Search, string Order, List<ActivityStatus>? Status = null) : IRequest<List<Activity>>;

    public class GetActivitesQueryHandler : IRequestHandler<GetActivitesQuery, List<Activity>>
    {
        private readonly IUnitOfWork uow;

        public GetActivitesQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<List<Activity>> Handle(GetActivitesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Activity> activities;
            var status = request.Status == null || !request.Status.Any() ? [ActivityStatus.Verified, ActivityStatus.NotVerified] : request.Status!;

            if(string.IsNullOrEmpty(request.Search))
            {
                activities = uow.Repository().FindQueryable<Activity>(a => status.Contains(a.Status));
            }
            else
            {
                var search = request.Search.ToLower();
                activities = uow.Repository().FindQueryable<Activity>(
                    a => status.Contains(a.Status)
                        && ( a.Value.ToLower().Contains(search)
                            || a.Source.ToLower().Contains(search)
                            || a.Description.ToLower().Contains(search)
                            || a.Tags.Any(t => t.Name.ToLower().Contains(search))
                        )
                );
            }
            
            var order = string.IsNullOrEmpty(request.Order) ? "la" : request.Order;
            return await RespectService.OrderActivitiesAsync(activities, order);
        }
    }
}