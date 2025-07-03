using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.DTOs;
using RespectCounter.Application.Services;

namespace RespectCounter.Application.Commands
{
    public record VerifyActivityCommand(Guid Id) : IRequest<ActivityDTO>;

    public class VerifyActivityCommandHandler : IRequestHandler<VerifyActivityCommand, ActivityDTO>
    {
        private readonly IUnitOfWork uow;

        public VerifyActivityCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<ActivityDTO> Handle(VerifyActivityCommand request, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.Now;

            //Get the activity
            Activity? a = await uow.Repository().FindByIdAsync<Activity>(request.Id)
                ?? throw new KeyNotFoundException($"The activity with the given ID ({request.Id}) was not found.");

            //Modify the person
            a.Status = ActivityStatus.Verified;
            a.LastUpdated = now;
            await uow.CommitAsync(cancellationToken);

            return a.ToDTO();
        }
    }
}