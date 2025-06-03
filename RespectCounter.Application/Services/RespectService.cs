using RespectCounter.Domain.Model;

namespace RespectCounter.Application;

public static class RespectService
{
    private static readonly int defaultRespect = 0;
    public static int CountRespect(List<Reaction> reactions)
    {
        int result = defaultRespect;
        foreach (Reaction r in reactions)
        {
            switch (r.ReactionType)
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
        throw new NotImplementedException();
        // int trendingDays = 7;
        // List<Person> ordered = new();
        // if (string.IsNullOrEmpty(order)) order = "la";
        // switch (order)
        // {
        //     case "bm":
        //         throw new NotImplementedException();
        //     case "mr":
        //         ordered = await persons.Include("Reactions").ToListAsync();
        //         ordered = ordered.Select(p => new Tuple<Person, int>(p, CountRespect(p.Reactions)))
        //                             .OrderByDescending(t => t.Item2)
        //                             .Select(t => t.Item1)
        //                             .ToList();
        //         break;
        //     case "lr":
        //         ordered = await persons.Include("Reactions").ToListAsync();
        //         ordered = ordered.Select(p => new Tuple<Person, int>(p, CountRespect(p.Reactions)))
        //                             .OrderBy(t => t.Item2)
        //                             .Select(t => t.Item1)
        //                             .ToList();
        //         break;
        //     case "la":
        //         ordered = await persons.OrderByDescending(p => p.Created).ToListAsync();
        //         break;
        //     case "tr":
        //         ordered = await persons.Include("Reactions").ToListAsync();
        //         ordered = persons.Select(p => new Tuple<Person, int>
        //         (
        //             p,
        //             CountRespect
        //             (
        //                 p.Reactions.Where(r => r.Created > DateTime.Now.AddDays(-trendingDays)).ToList()
        //             )
        //         ))
        //         .OrderBy(t => t.Item2)
        //         .Select(t => t.Item1)
        //         .ToList();
        //         break;
        //     case "Az":
        //         ordered = await persons.OrderBy(p => p.LastName).ToListAsync();
        //         break;
        //     case "Za":
        //         ordered = await persons.OrderBy(p => p.LastName).ToListAsync();
        //         break;
        // }

        // return ordered;
    }

    public static IEnumerable<Activity> OrderActivities(IEnumerable<Activity> posts, string order)
    {
        int trendingDays = 7;
        if (string.IsNullOrEmpty(order)) order = "la";
        switch (order)
        {
            case "bm":
                throw new NotImplementedException();
            case "mr":
                return posts.Select(a => new
                {
                    Activity = a,
                    RespectCount = CountRespect(a.Reactions)
                })
                        .OrderByDescending(a => a.RespectCount)
                        .Select(a => a.Activity);
            case "lr":
                return posts.Select(a => new
                {
                    Activity = a,
                    RespectCount = CountRespect(a.Reactions)
                })
                    .OrderBy(a => a.RespectCount)
                    .Select(a => a.Activity);
            case "la":
                return posts.OrderByDescending(a => a.Created);
            case "lh":
                return posts.OrderByDescending(a => a.Happend);
            case "tr":
                return posts.Select(a => new
                {
                    Activity = a,
                    RecentRespectCount = CountRespect(a.Reactions.Where(r => r.Created > DateTime.Now.AddDays(-trendingDays)).ToList())
                })
                    .OrderByDescending(t => t.RecentRespectCount)
                    .Select(t => t.Activity);
            default:
                return posts.OrderByDescending(a => a.Created);
        }
    }

    public static Func<IQueryable<Person>, IOrderedQueryable<Person>>? GetOrderByFunc(string order, int trendingDays = 7)
    {
        throw new NotImplementedException();
        // if (string.IsNullOrWhiteSpace(order))
        // {
        //     order = "la";
        // }
        
        // switch (order)
        // {
        //     case "bm": //BestMatching
        //         throw new NotImplementedException();
        //     case "mr": //MostLiked
        //         ordered = ordered.Select(p => new Tuple<Person, int>(p, CountRespect(p.Reactions)))
        //                             .OrderByDescending(t => t.Item2)
        //                             .Select(t => t.Item1)
        //                             .ToList();
        //         break;
        //     case "lr":
        //         ordered = await persons.Include("Reactions").ToListAsync();
        //         ordered = ordered.Select(p => new Tuple<Person, int>(p, CountRespect(p.Reactions)))
        //                             .OrderBy(t => t.Item2)
        //                             .Select(t => t.Item1)
        //                             .ToList();
        //         break;
        //     case "la":
        //         ordered = await persons.OrderByDescending(p => p.Created).ToListAsync();
        //         break;
        //     case "tr":
        //         ordered = await persons.Include("Reactions").ToListAsync();
        //         ordered = persons.Select(p => new Tuple<Person, int>
        //         (
        //             p,
        //             CountRespect
        //             (
        //                 p.Reactions.Where(r => r.Created > DateTime.Now.AddDays(-trendingDays)).ToList()
        //             )
        //         ))
        //         .OrderBy(t => t.Item2)
        //         .Select(t => t.Item1)
        //         .ToList();
        //         break;
        //     case "Az":
        //         ordered = await persons.OrderBy(p => p.LastName).ToListAsync();
        //         break;
        //     case "Za":
        //         ordered = await persons.OrderBy(p => p.LastName).ToListAsync();
        //         break;
        // }

        // return ordered;
    }
}