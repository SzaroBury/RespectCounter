using MediatR;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Model;
using RespectCounter.Application.DTOs;
using System.Security.Claims;

namespace RespectCounter.Application.Queries
{
    public record GetVerifiedPersonsQuery(string Search, string Order, ClaimsPrincipal? User) : IRequest<IEnumerable<PersonDTO>>;

    // to do: to find better solution for searching terms (contains is like "LIKE '%searchterm%'", so it is not an optimal solution)
    public class GetVerifiedPersonsQueryHandler : IRequestHandler<GetVerifiedPersonsQuery, IEnumerable<PersonDTO>>
    {
        private readonly IUnitOfWork uow;

        public GetVerifiedPersonsQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IEnumerable<PersonDTO>> Handle(GetVerifiedPersonsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        //     string? userGuid = null;
        //     if(request.User != null)
        //     {
        //         IdentityUser? user = await uow.UserManager.GetUserAsync(request.User);
        //         userGuid = user?.Id.ToString();
        //     }

        //     IQueryable<Person> persons;
        //     if(string.IsNullOrEmpty(request.Search))
        //     {
        //         persons = uow.Repository().FindQueryable<Person>(p => p.Status == PersonStatus.Verified);
        //     }
        //     else
        //     {
        //         var search = request.Search.ToLower();
        //         persons = uow.Repository().FindQueryable<Person>(
        //             p => p.Status == PersonStatus.Verified
        //                 && ( p.FirstName.ToLower().Contains(search)
        //                     || p.LastName.ToLower().Contains(search)
        //                     || p.Nationality.ToLower().Contains(search)
        //                     || p.Description.ToLower().Contains(search)
        //                     || p.Tags.Any(t => t.Name.ToLower().Contains(search))
        //                 )
        //         );
        //     }

        //     var token = uow.JwtService.GenerateAccessToken;
            
        //     var included = persons.Include(p => p.Tags).Include(p => p.Reactions).Include(p => p.CreatedBy);
            
        //     var ordered = await RespectService.OrderPersonsAsync(included, request.Order);
        //     return ordered.Select(p => p.ToDTO(userGuid)).ToList();
        }
    }
}