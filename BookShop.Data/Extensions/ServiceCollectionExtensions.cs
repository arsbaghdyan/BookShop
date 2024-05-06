using BookShop.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUniversityDbContext(this IServiceCollection services, DbOptions dbOptions)
    {
        services.AddDbContext<BookShopDbContext>(b =>
        {
            b.UseNpgsql(dbOptions.ConnectionString);
        });
        return services;
    }
}
