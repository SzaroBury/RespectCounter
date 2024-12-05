using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Repositories;

namespace RespectCounter.Application.Commands
{
    public record VerifyActivityCommand(string Id) : IRequest<Activity>;

    public class VerifyActivityCommandHandler : IRequestHandler<VerifyActivityCommand, Activity>
    {
        private readonly IUnitOfWork uow;

        public VerifyActivityCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Activity> Handle(VerifyActivityCommand request, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.Now;
            Guid guid;

            if(!Guid.TryParse(request.Id, out guid))
            {
                throw new ArgumentException("The given 'id' parameter is not a valid guid value.");
            }

            //Get the activity
            Activity? a = await uow.Repository().GetById<Activity>(guid);
            if(a == null) throw new KeyNotFoundException();

            //Modify the person
            a.Status = ActivityStatus.Verified;
            a.LastUpdated = now;
            await uow.CommitAsync(cancellationToken);

            return a;
        }
    }
}