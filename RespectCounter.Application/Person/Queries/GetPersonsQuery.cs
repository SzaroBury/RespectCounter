using MediatR;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Model;
using RespectCounter.Application.DTOs;
using RespectCounter.Application.Services;
using RespectCounter.Application.Common;

namespace RespectCounter.Application.Queries;

public record GetPersonsQuery(
    string Search,
    PersonSortBy Order,
    int Page, 
    int PageSize,
    Guid? UserId
) : IRequest<IEnumerable<PersonDTO>>;

public class GetPersonsQueryHandler : IRequestHandler<GetPersonsQuery, IEnumerable<PersonDTO>>
{
    private readonly IUnitOfWork uow;
    private readonly IUserService userService;

    public GetPersonsQueryHandler(IUnitOfWork uow, IUserService userService)
    {
        this.uow = uow;
        this.userService = userService;
    }

    public async Task<IEnumerable<PersonDTO>> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = null;
        if (request.UserId.HasValue)
        {
            var user = await userService.GetByIdAsync(request.UserId.Value);
            userId = user.Id;
        }

        IQueryable<Person> query = uow.Repository().FindQueryable<Person>(p => p.Status != PersonStatus.Hidden);
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

        orderedQuery = orderedQuery
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);

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