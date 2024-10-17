using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Repositories;

namespace RespectCounter.Application.Queries
{
    public record GetActivityByIdQuery(string Id) : IRequest<Activity>;

    public class GetActivityByIdQueryHandler : IRequestHandler<GetActivityByIdQuery, Activity>
    {
        private readonly IUnitOfWork uow;
        
        public GetActivityByIdQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Activity> Handle(GetActivityByIdQuery request, CancellationToken cancellationToken)
        {
            var act = await uow.Repository().SingleOrDefaultAsync<Activity>(p => p.Id.ToString() == request.Id, "Persons,Tags,Comments,Reactions");
            if (act is null)
                throw new KeyNotFoundException("The activity was not found. Please enter Id of the existing activity.");

            return act;
        }
    }
}