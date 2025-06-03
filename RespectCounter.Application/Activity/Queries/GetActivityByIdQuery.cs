using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.DTOs;
using System.Security.Claims;

namespace RespectCounter.Application.Queries
{
    public record GetActivityByIdQuery(string Id, Guid? UserId) : IRequest<ActivityDTO>;

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
            throw new NotImplementedException();
            // var act = await uow.Repository().FindQueryable<Activity>(a => a.Id.ToString() == request.Id)
            //     .Include(a => a.Person)
            //     .Include(a => a.Comments).ThenInclude(c => c.Children)
            //     .Include(a => a.Reactions)
            //     .Include(a => a.Tags)
            //     .Include(a => a.CreatedBy)
            //     .FirstOrDefaultAsync(cancellationToken);
            // if (act is null)
            // {
            //     throw new KeyNotFoundException("The activity was not found. Please enter Id of an existing activity.");
            // }

            // string? userGuid = null;
            // if(request.UserId.HasValue)
            // {
            //     User? user = await userService.GetByIdAsync(request.UserId.Value);
            //     userGuid = user?.Id.ToString();
            // }
            
            // return act.ToDTO(userGuid);
        }
    }
}