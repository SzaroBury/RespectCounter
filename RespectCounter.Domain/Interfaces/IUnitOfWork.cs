using Microsoft.AspNetCore.Identity;

namespace RespectCounter.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    UserManager<IdentityUser> UserManager { get; }
    IJwtService JwtService { get; }
    IRepository Repository();
    Task<int> CommitAsync(CancellationToken cancellationToken);
}