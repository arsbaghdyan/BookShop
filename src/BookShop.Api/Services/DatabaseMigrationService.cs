using BookShop.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BookShop.Api.Services;

public class DatabaseMigrationService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<DatabaseMigrationService> _logger;

    public DatabaseMigrationService(IServiceScopeFactory serviceScopeFactory,
        ILogger<DatabaseMigrationService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();

        var bookshopDbContext = serviceScope.ServiceProvider.GetRequiredService<BookShopDbContext>();

        var migrationTime = Stopwatch.StartNew();
        _logger.LogInformation($"{nameof(BookShopDbContext)} migration started");

        try
        {
            await bookshopDbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(BookShopDbContext)} migration failed. Message: {ex.Message ?? ex.InnerException?.Message}. " +
                $"ExceptionType: {ex.GetType().FullName}. Duration = {migrationTime.ElapsedMilliseconds}.");
            throw;
        }

        migrationTime.Stop();
        _logger.LogInformation($"{nameof(BookShopDbContext)} migration finished successfully. Duration = {migrationTime.ElapsedMilliseconds}.");

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
