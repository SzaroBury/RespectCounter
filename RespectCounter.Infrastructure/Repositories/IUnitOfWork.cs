using Microsoft.AspNetCore.Identity;
using RespectCounter.Domain.Interfaces;

namespace RespectCounter.Infrastructure.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository Repository();
    UserManager<IdentityUser> UserManager { get; init; }
    Task<int> CommitAsync(CancellationToken cancellationToken);
}