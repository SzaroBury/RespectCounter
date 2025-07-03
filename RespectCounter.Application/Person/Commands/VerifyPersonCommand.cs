using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.DTOs;
using RespectCounter.Application.Services;

namespace RespectCounter.Application.Commands
{
    public record VerifyPersonCommand(Guid Id) : IRequest<PersonDTO>;

    public class VerifyPersonCommandHandler : IRequestHandler<VerifyPersonCommand, PersonDTO>
    {
        private readonly IUnitOfWork uow;

        public VerifyPersonCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<PersonDTO> Handle(VerifyPersonCommand request, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.Now;

            //Get the person
            Person? p = await uow.Repository().FindByIdAsync<Person>(request.Id)
                ?? throw new KeyNotFoundException($"A person with the given ID ({request.Id} was not found.)");

            //Modify the person
            p.Status = PersonStatus.Verified;
            p.LastUpdated = now;
            await uow.CommitAsync(cancellationToken);

            return p.ToDTO(null); //ToDTO
        }
    }
}