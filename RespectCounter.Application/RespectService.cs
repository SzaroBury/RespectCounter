using Microsoft.EntityFrameworkCore;
using RespectCounter.Domain.Model;

namespace RespectCounter.Application;

public static class RespectService
{
    private static int defaultRespect = 10;
    public static int CountRespect(List<Reaction> reactions)
    {
        int result = defaultRespect;
        foreach(Reaction r in reactions)
        {
            switch(r.ReactionType)
            {
                case ReactionType.Hate:
                    result -= 2;
                    break;
                case ReactionType.Dislike:
                    result -= 1;
                    break;
                case ReactionType.Like:
                    result += 1;
                    break;
                case ReactionType.Love:
                    result += 2;
                    break;
            }
        }

        return result;
    }


    public static async Task<List<Person>> OrderPersonsAsync(IQueryable<Person> persons, string order)
    {
        List<Person> ordered = new();
        if(string.IsNullOrEmpty(order)) order = "la";
        switch(order)
        {
            case "bm":
                throw new NotImplementedException();
            case "mr":
                ordered = await persons.Include("Reactions").ToListAsync();
                ordered = ordered.Select(p => new Tuple<Person, int>(p, CountRespect(p.Reactions)))
                                    .OrderByDescending(t => t.Item2)
                                    .Select(t => t.Item1)
                                    .ToList();
                break;
            case "lr":
                ordered = await persons.Include("Reactions").ToListAsync();
                ordered = ordered.Select(p => new Tuple<Person, int>(p, CountRespect(p.Reactions)))
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
                    CountRespect
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