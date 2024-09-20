using MediatR;
using System.Net;
using RespectCounter.Infrastructure;
using RespectCounter.Infrastructure.Repositories;
using RespectCounter.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Xml;

namespace RespectCounter.Application.Queries
{
    public record GetVerifiedPersonsQuery : IRequest<IEnumerable<Person>>
    {
        public string Search { get; set; } = "";
        public string Order { get; set; } = ""; //bm - Best Match; mr - Most Respected; lr - Least Respected, la, - Latest, tr - Trending (last 7d reactions and respect), aZ - Alfphabetical
    }

    // to do: to find better solution for searching terms (contains is like "LIKE '%searchterm%'", so it is not an optimal solution)
    // to do: implement BestMatch 
    public class GetVerifiedPersonsQueryHandler : IRequestHandler<GetVerifiedPersonsQuery, IEnumerable<Person>>
    {
        private readonly IUnitOfWork uow;

        public GetVerifiedPersonsQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IEnumerable<Person>> Handle(GetVerifiedPersonsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Person> persons;
            if(string.IsNullOrEmpty(request.Search))
            {
                persons = uow.Repository().FindQueryable<Person>(p => p.Status == PersonStatus.Verified);
            }
            else
            {
                var search = request.Search.ToLower();
                persons = uow.Repository().FindQueryable<Person>(
                    p => p.Status == PersonStatus.Verified
                        && ( p.FirstName.ToLower().Contains(search)
                            || p.LastName.ToLower().Contains(search)
                            || p.Nationality.ToLower().Contains(search)
                            || p.Description.ToLower().Contains(search)
                            || p.Tags.Any(t => t.Name.ToLower().Contains(search))
                        )
                );
            }


            List<Person> ordered = new();
            if(string.IsNullOrEmpty(request.Order)) request.Order = "la";
            switch(request.Order)
            {
                case "bm":
                    throw new NotImplementedException();
                case "mr":
                    ordered = await persons.Include("Reactions").ToListAsync();
                    ordered = ordered.Select(p => new Tuple<Person, int>(p, RespectService.CountRespect(p.Reactions)))
                                        .OrderByDescending(t => t.Item2)
                                        .Select(t => t.Item1)
                                        .ToList();
                    break;
                case "lr":
                    ordered = await persons.Include("Reactions").ToListAsync();
                    ordered = ordered.Select(p => new Tuple<Person, int>(p, RespectService.CountRespect(p.Reactions)))
                                        .OrderBy(t => t.Item2)
                                        .Select(t => t.Item1)
                                        .ToList();   
                    break;
                case "la":
                    ordered = await persons.OrderByDescending(p => p.Created).ToListAsync();               
                    break;
                case "tr":
                    ordered = await persons.Include("Reactions").ToListAsync();
                    ordered = persons.Select(p => new Tuple<Person, int>
                    (
                        p, 
                        RespectService.CountRespect
                        (
                            p.Reactions.Where(r => r.Created > DateTime.Now.AddDays(-7)).ToList()
                        )
                    ))
                    .OrderBy(t => t.Item2)
                    .Select(t => t.Item1)
                    .ToList();
                    break;
                case "Az":
                    ordered = await persons.OrderBy(p => p.LastName).ToListAsync();
                    break;
                case "Za":
                    ordered = await persons.OrderBy(p => p.LastName).ToListAsync();
                    break;
            }

            return ordered;
        }
    }
}