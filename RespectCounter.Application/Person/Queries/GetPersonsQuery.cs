using MediatR;
using RespectCounter.Infrastructure.Repositories;
using RespectCounter.Domain.Model;
using RespectCounter.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace RespectCounter.Application.Queries
{
    public record GetPersonsQuery(string Search = "", string Order = "la") : IRequest<List<PersonDTO>>;

    public class GetPersonsQueryHandler : IRequestHandler<GetPersonsQuery, List<PersonDTO>>
    {
        private readonly IUnitOfWork uow;

        public GetPersonsQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<List<PersonDTO>> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Person> persons;
            if(string.IsNullOrEmpty(request.Search))
            {
                persons = uow.Repository().FindQueryable<Person>(p => p.Status != PersonStatus.Hidden);
            }
            else
            {
                var search = request.Search.ToLower();
                persons = uow.Repository().FindQueryable<Person>(
                    p => p.Status != PersonStatus.Hidden
                        && ( p.FirstName.ToLower().Contains(search)
                            || p.LastName.ToLower().Contains(search)
                            || p.Nationality.ToLower().Contains(search)
                            || p.Description.ToLower().Contains(search)
                            || p.Tags.Any(t => t.Name.ToLower().Contains(search))
                        )
                );
            }
            
            var included = persons.Include(p => p.Tags).Include(p => p.Reactions).Include(p => p.CreatedBy);
            
            var ordered = await RespectService.OrderPersonsAsync(included, request.Order);
            return ordered.Select(p => p.ToDTO()).ToList();
        }
    }
}