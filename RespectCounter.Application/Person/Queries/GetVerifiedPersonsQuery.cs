using MediatR;
using System.Net;
using RespectCounter.Infrastructure;
using RespectCounter.Infrastructure.Repositories;
using RespectCounter.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace RespectCounter.Application.Queries
{
    public record GetVerifiedPersonsQuery : IRequest<List<Person>>
    {
        public string Search { get; set; } = "";
        public string Order { get; set; } = "mr";

        public GetVerifiedPersonsQuery(string search, string order)
        {
            Search = search;
            Order = order.IsNullOrEmpty() ? "mr" : order;
        }
    }

    // to do: to find better solution for searching terms (contains is like "LIKE '%searchterm%'", so it is not an optimal solution)
    public class GetVerifiedPersonsQueryHandler : IRequestHandler<GetPersonsQuery, List<Person>>
    {
        private readonly IUnitOfWork uow;

        public GetVerifiedPersonsQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<List<Person>> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
        {
            var persons = await uow.Repository().FindQueryable<Person>(
            p => p.Status == PersonStatus.Verified
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