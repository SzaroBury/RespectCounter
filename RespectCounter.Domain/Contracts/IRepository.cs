using System.Linq.Expressions;
using RespectCounter.Domain.Model;

namespace RespectCounter.Domain.Contracts;

public interface IRepository
{
    Task<T?> FindByIdAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : Entity;
    IQueryable<T> FindQueryable<T>(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null) where T : Entity;
    Task<List<T>> FindListAsync<T>(Expression<Func<T, bool>>? expression, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, CancellationToken cancellationToken = default) where T : class;
    Task<List<T>> FindAllAsync<T>(CancellationToken cancellationToken = default) where T : Entity;
    Task<T?> SingleOrDefaultAsync<T>(Expression<Func<T, bool>> expression, string? includeProperties = null, CancellationToken cancellationToken = default) where T : Entity;
    T Add<T>(T entity) where T : Entity;
    void Update<T>(T entity) where T : Entity;
}