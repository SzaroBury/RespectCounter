using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Repositories;
using RespectCounter.Application.DTOs;

namespace RespectCounter.Application.Queries
{
    public record GetActivityByIdQuery(string Id) : IRequest<ActivityDTO>;

    public class GetActivityByIdQueryHandler : IRequestHandler<GetActivityByIdQuery, ActivityDTO>
    {
        private readonly IUnitOfWork uow;
        
        public GetActivityByIdQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<ActivityDTO> Handle(GetActivityByIdQuery request, CancellationToken cancellationToken)
        {
            var act = await uow.Repository().SingleOrDefaultAsync<Activity>(p => p.Id.ToString() == request.Id, "Person,Tags,Comments,Reactions,CreatedBy");
            if (act is null)
            {
                throw new KeyNotFoundException("The activity was not found. Please enter Id of the existing activity.");
            }
            return act.ToDTO();
        }
    }
}