using BookShop.Data;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Api.Services;

public class DatabaseMigrationService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseMigrationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BookShopDbContext>();

            await dbContext.Database.MigrateAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
