using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Repositories;

namespace RespectCounter.Application.Commands
{
    public record VerifyPersonCommand() : IRequest<Person>
    {
        public required string Id { get; set; }
    }

    public class VerifyPersonCommandHandler : IRequestHandler<VerifyPersonCommand, Person>
    {
        private readonly IUnitOfWork uow;

        public VerifyPersonCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Person> Handle(VerifyPersonCommand request, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.Now;
            Guid guid;

            if(!Guid.TryParse(request.Id, out guid))
            {
                throw new ArgumentException("The given 'id' parameter is not a valid guid value.");
            }

            //Get the person
            Person? p = await uow.Repository().GetById<Person>(guid);
            if(p == null) throw new KeyNotFoundException();

            //Modify the person
            p.Status = PersonStatus.Verified;
            p.LastUpdated = now;
            await uow.CommitAsync(cancellationToken);

            return p;
        }
    }
}