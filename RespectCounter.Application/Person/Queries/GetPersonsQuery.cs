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
        public string Order { get; set; } = "mr";

        public GetPersonsQuery(string search, string order)
        {
            Search = search;
            Order = order.IsNullOrEmpty() ? "mr" : order;
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
            var persons = await uow.Repository().FindQueryable<Person>(
                p => p.Status != PersonStatus.Hidden
                    && ( p.FirstName.ToLower().Contains(request.Search.ToLower())
                    || p.LastName.ToLower().Contains(request.Search.ToLower())
                    || p.Nationality.ToLower().Contains(request.Search.ToLower())
                    || p.Description.ToLower().Contains(request.Search.ToLower())
                    || p.Tags.Any(t => t.Name.ToLower().Contains(request.Search.ToLower())))
            ).ToListAsync();

            return persons;
        }
    }
}