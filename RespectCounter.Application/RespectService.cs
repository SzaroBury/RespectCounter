using Microsoft.EntityFrameworkCore;
using RespectCounter.Domain.DTO;
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
        int trendingDays = 7;
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
                        p.Reactions.Where(r => r.Created > DateTime.Now.AddDays(-trendingDays)).ToList()
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

    public static async Task<List<Activity>> OrderActivitiesAsync(IQueryable<Activity> posts, string order)
    {
        int trendingDays = 7;
        List<Activity> ordered = new();
        if(string.IsNullOrEmpty(order)) order = "la";
        switch(order)
        {
            case "bm":
                throw new NotImplementedException();
            case "mr":
                ordered = await posts.Include("Reactions").ToListAsync();
                ordered = ordered.Select(a => new Tuple<Activity, int>(a, CountRespect(a.Reactions)))
                                    .OrderByDescending(t => t.Item2)
                                    .Select(t => t.Item1)
                                    .ToList();
                break;
            case "lr":
                ordered = await posts.Include("Reactions").ToListAsync();
                ordered = ordered.Select(a => new Tuple<Activity, int>(a, CountRespect(a.Reactions)))
                                    .OrderBy(t => t.Item2)
                                    .Select(t => t.Item1)
                                    .ToList();   
                break;
            case "la":
                ordered = await posts.OrderByDescending(a => a.Created).ToListAsync();               
                break;
            case "lh":
                ordered = await posts.OrderByDescending(a => a.Happend).ToListAsync();               
                break;
            case "tr":
                ordered = await posts.Include("Reactions").ToListAsync();
                ordered = posts.Select(a => new Tuple<Activity, int>
                (
                    a, 
                    CountRespect
                    (
                        a.Reactions.Where(r => r.Created > DateTime.Now.AddDays(-trendingDays)).ToList()
                    )
                )).ToList()
                .OrderBy(t => t.Item2)
                .Select(t => t.Item1)
                .ToList();
                
                break;
        }

        return ordered;
    }

    public static ActivityQueryDTO MapActivityToQueryDTO(Activity a)
    {
        return new ActivityQueryDTO(
            a.Id.ToString(),
            string.Join(",", a.Persons.Select(p => p.Id.ToString())),
            string.Join(",", a.Persons.Select(p => p.FirstName + " " + p.LastName)),
            string.Join(",", a.Persons.Select(p => CountRespect(p.Reactions))),
            string.Join(",", a.Persons.Select(p => "/persons/person_" + p.LastName.ToLower() + ".jpg")),
            a.CreatedBy?.UserName ?? "??",
            a.CreatedById,
            a.Value,
            a.Description,
            a.Location,
            a.Source,
            string.Join(",", a.Tags),
            a.Happend.ToLongDateString(),
            a.Comments.Count,
            (int)a.Type,
            CountRespect(a.Reactions)
        );
    }    
}