using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Interfaces;

namespace RespectCounter.Infrastructure.Repositories
{
    public class Repository : IRepository
    {
        private readonly RespectDbContext dbContext;

        public Repository(RespectDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<T?> GetById<T>(Guid id) where T : Entity
        {
            return await dbContext.Set<T>().FindAsync(id);
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

        public Task<List<T>> FindAllAsync<T>(CancellationToken cancellationToken) where T : Entity
        {
            return dbContext.Set<T>().ToListAsync(cancellationToken);
        }

        public Task<T?> SingleOrDefaultAsync<T>(Expression<Func<T, bool>> expression, string includeProperties) where T : Entity
        {
            var query = dbContext.Set<T>().AsQueryable();

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.SingleOrDefaultAsync(expression);
        }

        public T Add<T>(T entity) where T : Entity
        {
            return dbContext.Set<T>().Add(entity).Entity;
        }

        public void Update<T>(T entity) where T : Entity
        {
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete<T>(T entity) where T : Entity
        {
            dbContext.Set<T>().Remove(entity);
        }
    }
}