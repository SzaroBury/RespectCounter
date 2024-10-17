using MediatR;
using RespectCounter.Infrastructure.Repositories;
using RespectCounter.Domain.Model;

namespace RespectCounter.Application.Queries
{
    public record GetActivitesQuery : IRequest<List<Activity>>
    {
        public string Search { get; set; } = "";
        public string Order { get; set; } = "";

        public GetActivitesQuery(string search, string order)
        {
            Search = search;
            Order = string.IsNullOrEmpty(order) ? "la" : order;
        }
    }

    public class GetActivitesQueryHandler : IRequestHandler<GetActivitesQuery, List<Activity>>
    {
        private readonly IUnitOfWork uow;

        public GetActivitesQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<List<Activity>> Handle(GetActivitesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Activity> posts;
            if(string.IsNullOrEmpty(request.Search))
            {
                posts = uow.Repository().FindQueryable<Activity>(p => p.Status != ActivityStatus.Hidden);
            }
            else
            {
                var search = request.Search.ToLower();
                posts = uow.Repository().FindQueryable<Activity>(
                    a => a.Status != ActivityStatus.Hidden
                        && ( a.Value.ToLower().Contains(search)
                            || a.Source.ToLower().Contains(search)
                            || a.Description.ToLower().Contains(search)
                            || a.Tags.Any(t => t.Name.ToLower().Contains(search))
                        )
                );
            }
            
            return await RespectService.OrderActivitiesAsync(posts, request.Order);
        }
    }
}