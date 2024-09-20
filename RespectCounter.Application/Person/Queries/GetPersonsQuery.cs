using MediatR;
using System.Net;
using RespectCounter.Infrastructure;
using RespectCounter.Infrastructure.Repositories;
using RespectCounter.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace RespectCounter.Application.Queries
{
    public record GetPersonsQuery : IRequest<List<Person>>
    {
        public string Search { get; set; } = "";
        public string Order { get; set; } = "";

        public GetPersonsQuery(string search, string order)
        {
            Search = search;
            Order = string.IsNullOrEmpty(order) ? "la" : order;
        }
    }

    public class GetPersonsQueryHandler : IRequestHandler<GetPersonsQuery, List<Person>>
    {
        private readonly IUnitOfWork uow;

        public GetPersonsQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<List<Person>> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
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
            
            return await RespectService.OrderPersonsAsync(persons, request.Order);
        }
    }
}