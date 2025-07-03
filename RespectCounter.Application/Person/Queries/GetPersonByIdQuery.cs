using MediatR;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Services;

namespace RespectCounter.Application.Queries
{
    public record GetPersonByIdQuery(Guid Id, Guid? UserId) : IRequest<PersonDTO>;

    public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, PersonDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IUserService userService;

        public GetPersonByIdQueryHandler(IUnitOfWork uow, IUserService userService)
        {
            this.uow = uow;
            this.userService = userService;
        }

        public async Task<PersonDTO> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            Guid? userId = null;
            if(request.UserId.HasValue)
            {
                var user = await userService.GetByIdAsync(request.UserId.Value);
                userId = user?.Id;
            }

            var person = await uow.Repository()
                .SingleOrDefaultAsync<Person>(
                    a => a.Id == request.Id,
                    "Activities,Comments.Children,Reactions,Tags",
                    cancellationToken
                ) ?? throw new KeyNotFoundException("The person was not found. Please enter Id of an existing person.");


            var comments = await uow.Repository()
                .FindListAsync<Comment>(c => c.PersonId == person.Id);

            person.CreatedBy = await userService.GetByIdAsync(person.CreatedById);

            return person.ToDTO(userId);
        }
    }
}