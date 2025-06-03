using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Commands
{
    public record VerifyPersonCommand(Guid Id) : IRequest<Person>;

    public class VerifyPersonCommandHandler : IRequestHandler<VerifyPersonCommand, Person>
    {
        private readonly IUnitOfWork uow;

        public VerifyPersonCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Person> Handle(VerifyPersonCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // DateTime now = DateTime.Now;

            // //Get the person
            // Person? p = await uow.Repository().FindByIdAsync<Person>(request.Id)
            //     ?? throw new KeyNotFoundException($"A person with the given ID ({request.Id} was not found.)");

            // //Modify the person
            // p.Status = PersonStatus.Verified;
            // p.LastUpdated = now;
            // await uow.CommitAsync(cancellationToken);

            // return p; //ToDTO
        }
    }
}