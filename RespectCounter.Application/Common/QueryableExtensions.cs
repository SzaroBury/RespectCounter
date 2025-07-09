namespace RespectCounter.Application.Common;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int page, int pageSize)
    {
        return query
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
    }
}