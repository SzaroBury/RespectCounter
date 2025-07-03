using MediatR;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Model;
using RespectCounter.Application.DTOs;
using System.Security.Claims;
using RespectCounter.Application.Common;
using RespectCounter.Application.Services;

namespace RespectCounter.Application.Queries
{
    public record GetVerifiedPersonsQuery(string Search, PersonSortBy Order, Guid? UserId) : IRequest<IEnumerable<PersonDTO>>;

    // to do: to find better solution for searching terms (contains is like "LIKE '%searchterm%'", so it is not an optimal solution)
    public class GetVerifiedPersonsQueryHandler : IRequestHandler<GetVerifiedPersonsQuery, IEnumerable<PersonDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IUserService userService;

        public GetVerifiedPersonsQueryHandler(IUnitOfWork uow, IUserService userService)
        {
            this.uow = uow;
            this.userService = userService;
        }

        public async Task<IEnumerable<PersonDTO>> Handle(GetVerifiedPersonsQuery request, CancellationToken cancellationToken)
        {
            Guid? userId = null;
            if (request.UserId.HasValue)
            {
                var user = await userService.GetByIdAsync(request.UserId.Value);
                userId = user.Id;
            }

            IQueryable<Person> query = uow.Repository().FindQueryable<Person>(p => p.Status == PersonStatus.Verified);
            if(!string.IsNullOrEmpty(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(
                    p => p.FirstName.Contains(search, StringComparison.CurrentCultureIgnoreCase)
                        || p.LastName.Contains(search, StringComparison.CurrentCultureIgnoreCase)
                        || p.Nationality.Contains(search, StringComparison.CurrentCultureIgnoreCase)
                        || p.Description.Contains(search, StringComparison.CurrentCultureIgnoreCase)
                        || p.Tags.Any(t => t.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase))

                );
            }

            var orderedQuery = query.ApplySorting(request.Order);

            var persons = await uow.Repository()
                .FindListAsync(
                    orderedQuery,
                    ["Comments.Children", "Reactions", "Tags"],
                    null,
                    cancellationToken
                );

            foreach (var person in persons)
            {
                person.CreatedBy = await userService.GetByIdAsync(person.CreatedById);
            }
            
            return persons.Select(p => p.ToDTO(userId));
        }
    }
}