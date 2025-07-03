using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.DTOs;
using RespectCounter.Application.Services;

namespace RespectCounter.Application.Queries
{
    public record GetActivityByIdQuery(Guid Id, Guid? UserId) : IRequest<ActivityDTO>;

    public class GetActivityByIdQueryHandler : IRequestHandler<GetActivityByIdQuery, ActivityDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IUserService userService;

        public GetActivityByIdQueryHandler(IUnitOfWork uow, IUserService userService)
        {
            this.uow = uow;
            this.userService = userService;
        }

        public async Task<ActivityDTO> Handle(GetActivityByIdQuery request, CancellationToken cancellationToken)
        {
            Guid? userId = null;
            if(request.UserId.HasValue)
            {
                User? user = await userService.GetByIdAsync(request.UserId.Value);
                userId = user?.Id;
            }
            
            var act = await uow.Repository()
                .SingleOrDefaultAsync<Activity>(
                    a => a.Id == request.Id,
                    "Person,Comments.Children,Reactions,Tags",
                    cancellationToken
                ) ?? throw new KeyNotFoundException("The activity was not found. Please enter Id of an existing activity.");

            act.CreatedBy = await userService.GetByIdAsync(act.CreatedById);

            return act.ToDTO(userId);
        }
    }
}