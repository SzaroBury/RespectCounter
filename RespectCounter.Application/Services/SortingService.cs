using RespectCounter.Application.Common;
using RespectCounter.Domain.Model;

namespace RespectCounter.Application.Services;

public static class SortingService
{

    public static IQueryable<Activity> ApplySorting(this IQueryable<Activity> query, ActivitySortBy sortBy, int trendingDays = 7)
    {
        return sortBy switch
        {
            ActivitySortBy.LatestAdded => query.OrderByDescending(static a => a.Created),
            ActivitySortBy.LatestHappend => query.OrderByDescending(static a => a.Happend),
            ActivitySortBy.BestMatching => throw new NotImplementedException(),
            ActivitySortBy.MostLiked => query.OrderByDescending(
                a => a.Reactions.Sum(r => (int)r.ReactionType)),
            ActivitySortBy.LeastLiked => query.OrderBy(
                a => a.Reactions.Sum(r => (int)r.ReactionType)),
            ActivitySortBy.Trending => query.OrderByDescending(
                a => a.Reactions.Where(r => r.Created > DateTime.Now.AddDays(-trendingDays))
                    .Sum(r => (int)r.ReactionType)),
            _ => throw new NotImplementedException()
        };
        throw new NotImplementedException();

    }

    public static IQueryable<Person> ApplySorting(this IQueryable<Person> query, PersonSortBy sortBy, int trendingDays = 7)
    {
        return sortBy switch
        {
            PersonSortBy.Trending => query.OrderByDescending(
                p => p.Reactions.Where(r => r.Created > DateTime.Now.AddDays(-trendingDays))
                    .Sum(r => (int)r.ReactionType)),
            PersonSortBy.MostRespected => query.OrderByDescending(
                p => p.Reactions.Sum(r => (int)r.ReactionType)),
            PersonSortBy.LeastRespected => query.OrderBy(
                p => p.Reactions.Sum(r => (int)r.ReactionType)),
            PersonSortBy.LatestAdded => query.OrderByDescending(p => p.Created),
            PersonSortBy.AlphabeticalLastname => query.OrderBy(p => p.LastName),
            PersonSortBy.AlphabeticalReversedLastname => query.OrderByDescending(p => p.LastName),
            PersonSortBy.BestMatching => throw new NotImplementedException(),
            _ => query
        };
    }

    public static IQueryable<Comment> ApplySorting(this IQueryable<Comment> query, CommentSortBy sortBy)
    {
        return sortBy switch
        {
            CommentSortBy.MostRespected => query.OrderByDescending(
                p => p.Reactions.Sum(r => (int)r.ReactionType)),
            CommentSortBy.LeastRespected => query.OrderBy(
                p => p.Reactions.Sum(r => (int)r.ReactionType)),
            CommentSortBy.LatestAdded => query.OrderByDescending(p => p.Created),
            CommentSortBy.OldestAdded => query.OrderBy(p => p.Created),
            _ => query
        };
    }
}