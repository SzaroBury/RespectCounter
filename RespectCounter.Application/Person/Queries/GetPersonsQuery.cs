using MediatR;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Model;
using RespectCounter.Application.DTOs;

namespace RespectCounter.Application.Queries
{
    public record GetPersonsQuery(string Search, string Order, Guid? UserId) : IRequest<List<PersonDTO>>;

    public class GetPersonsQueryHandler : IRequestHandler<GetPersonsQuery, List<PersonDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IUserService userService;

        public GetPersonsQueryHandler(IUnitOfWork uow, IUserService userService)
        {
            this.uow = uow;
            this.userService = userService;
        }

        public async Task<List<PersonDTO>> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // Guid? userId = null;
            // if (request.UserId.HasValue)
            // {
            //     var user = await userService.GetByIdAsync(request.UserId.Value);
            //     userId = user.Id;
            // }

            // IQueryable<Person> persons;
            // if(string.IsNullOrEmpty(request.Search))
            // {
            //     persons = uow.Repository().FindQueryable<Person>(p => p.Status != PersonStatus.Hidden);
            //     //include  ["Tags", "Reactions", "CreatedBy"]
            // }
            // else
            // {

            //     var search = request.Search.ToLower();
            //     persons = uow.Repository().FindQueryable<Person>(
            //         p => p.Status != PersonStatus.Hidden
            //             && ( p.FirstName.Contains(search, StringComparison.CurrentCultureIgnoreCase)
            //                 || p.LastName.Contains(search, StringComparison.CurrentCultureIgnoreCase)
            //                 || p.Nationality.Contains(search, StringComparison.CurrentCultureIgnoreCase)
            //                 || p.Description.Contains(search, StringComparison.CurrentCultureIgnoreCase)
            //                 || p.Tags.Any(t => t.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase))
            //             )
            //     );
            // }
            
            // var included = persons.Include(p => p.Tags).Include(p => p.Reactions).Include(p => p.CreatedBy);
            
            // var ordered = await RespectService.OrderPersonsAsync(included, request.Order);
            // return ordered.Select(p => p.ToDTO(userGuid)).ToList();
        }
    }
}