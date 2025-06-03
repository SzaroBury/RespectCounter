using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Infrastructure.Repositories
{
    public class Repository : IRepository
    {
        private readonly RespectDbContext dbContext;

        public Repository(RespectDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<T?> FindByIdAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : Entity
        {
            return await dbContext.Set<T>().FindAsync([id], cancellationToken: cancellationToken);
        }

        public IQueryable<T> FindQueryable<T>(Expression<Func<T, bool>> expression,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null) where T : Entity
        {
            var query = dbContext.Set<T>().Where(expression);
            return orderBy != null ? orderBy(query) : query;
        }

        public Task<List<T>> FindListAsync<T>(Expression<Func<T, bool>>? expression, Func<IQueryable<T>,
            IOrderedQueryable<T>>? orderBy = null, CancellationToken cancellationToken = default)
            where T : class
        {
            var query = expression != null ? dbContext.Set<T>().Where(expression) : dbContext.Set<T>();
            return orderBy != null
                ? orderBy(query).ToListAsync(cancellationToken)
                : query.ToListAsync(cancellationToken);
        }

        public Task<T?> SingleOrDefaultAsync<T>(Expression<Func<T, bool>> expression, string? includeProperties = null) where T : Entity
        {
            var query = dbContext.Set<T>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                query = includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }

            return query.SingleOrDefaultAsync(expression);
        }

        public Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate, params string[]? includeProperties) where T : Entity
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> FindQueryableWithIncludes<T>(Expression<Func<T, bool>> expression, params string[] includeProperties) where T : Entity
        {
            throw new NotImplementedException();
        }

        public T Add<T>(T entity) where T : Entity
        {
            return dbContext.Set<T>().Add(entity).Entity;
        }

        public void Update<T>(T entity) where T : Entity
        {
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task SoftDeleteById<T>(Guid id) where T : Entity
        {
            var item = await FindByIdAsync<T>(id)
                ?? throw new KeyNotFoundException($"The item with the given ID ({id}) was not found.");

            //item.Deleted = true;
            //item.DeletedDate = DateTime.Now;
            dbContext.Update(item);
        }
        
        public async Task<List<T>> FindItemsWithRespectAsync<T>(
            Expression<Func<T, bool>> filter,
            string order,
            string[]? includeProperties = null,
            CancellationToken cancellationToken = default) where T : Entity, IReactionable
        {
            throw new NotImplementedException();
            // var query = dbContext.Set<T>().Where(filter);
            // if (includeProperties != null)
            // {
            //     foreach (var includeProperty in includeProperties)
            //     {
            //         query = query.Include(includeProperty);
            //     }
            // }

            // switch (order)
            // {
            //     case "mr": // Most Respected
            //         return await query
            //             .OrderByDescending(item => item.Reactions
            //                 .Select(r =>
            //                     r.ReactionType == ReactionType.Hate ? -2 :
            //                     r.ReactionType == ReactionType.Dislike ? -1 :
            //                     r.ReactionType == ReactionType.Like ? 1 :
            //                     r.ReactionType == ReactionType.Love ? 2 : 0
            //                 ).Sum())
            //             .ToListAsync(cancellationToken);
            //     case "lr": // Least Respected
            //         return await query
            //             .OrderBy(p => p.Reactions
            //                 .Select(r =>
            //                     r.ReactionType == ReactionType.Hate ? -2 :
            //                     r.ReactionType == ReactionType.Dislike ? -1 :
            //                     r.ReactionType == ReactionType.Like ? 1 :
            //                     r.ReactionType == ReactionType.Love ? 2 : 0
            //                 ).Sum())
            //             .ToListAsync(cancellationToken);
            //     case "Az":
            //         return await query.OrderBy(p => p.LastName).ToListAsync(cancellationToken);
            //     case "Za":
            //         return await query.OrderByDescending(p => p.LastName).ToListAsync(cancellationToken);
            //     case "la":
            //     default:
            //         return await query.OrderByDescending(p => p.Created).ToListAsync(cancellationToken);
            // }
        }

        public Task<List<T>> FindAllAsync<T>(CancellationToken cancellationToken = default) where T : Entity
        {
            throw new NotImplementedException();
        }

        public Task<T?> SingleOrDefaultAsync<T>(Expression<Func<T, bool>> expression, string? includeProperties = null, CancellationToken cancellationToken = default) where T : Entity
        {
            throw new NotImplementedException();
        }
    }
}