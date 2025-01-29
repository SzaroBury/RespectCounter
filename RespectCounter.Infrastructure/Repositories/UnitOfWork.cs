using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RespectCounter.Domain.Interfaces;

namespace RespectCounter.Infrastructure.Repositories;

/// <summary>
/// Unit of work: Maintains a list of objects affected by a business transaction and coordinates the writing out of changes and the resolution of concurrency problems. https://www.martinfowler.com/eaaCatalog/unitOfWork.html
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly RespectDbContext context;
    private bool disposed;
    public UserManager<IdentityUser> UserManager { get; init; }
    public IJwtService JwtService { get; init; }
    public UnitOfWork(RespectDbContext context, UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        this.context = context;
        UserManager = userManager;
        JwtService = new JwtService(configuration);
        context.Database.EnsureCreated();
    }

    public IRepository Repository()
    {
        return new Repository(context);
    }

    public Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                context.Dispose();
                UserManager.Dispose();
            }
        }
        disposed = true;
    }
}