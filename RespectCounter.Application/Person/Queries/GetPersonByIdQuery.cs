using MediatR;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;

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
            throw new NotImplementedException();
            // var person = await uow.Repository().FindQueryable<Person>(p => p.Id == request.Id)
            //     .Include(p => p.Tags)
            //     .Include(p => p.Comments).ThenInclude(c => c.Children)
            //     .Include(p => p.Activities)
            //     .Include(p => p.Reactions)
            //     .Include(a => a.CreatedBy)
            //     .FirstOrDefaultAsync(cancellationToken);
            // if (person is null)
            // {
            //     throw new KeyNotFoundException("Person not found. Please enter the ID of an existing person.");
            // }

            // string? userGuid = null;
            // if(request.UserId.HasValue)
            // {
            //     User? user = await userService.GetByIdAsync(request.UserId.Value);
            //     userGuid = user?.Id ?? null;
            // }

            // return person.ToDTO(userGuid);
        }
    }
}