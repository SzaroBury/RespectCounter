using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Infrastructure.Services;

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly RespectDbContext _context;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(RespectDbContext context, ILogger<DatabaseInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        _logger.LogInformation("Checking for pending EF Core migrations...");

        var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            _logger.LogInformation("Pending migrations found. Applying migrations...");
            await _context.Database.MigrateAsync();
            _logger.LogInformation("Migrations applied successfully.");
        }
        else
        {
            _logger.LogInformation("No pending migrations found. Database is up-to-date.");
        }
    }
}