using RespectCounter.Domain.Interfaces;

namespace RespectCounter.Infrastructure.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository Repository();
    Task<int> CommitAsync(CancellationToken cancellationToken);
}