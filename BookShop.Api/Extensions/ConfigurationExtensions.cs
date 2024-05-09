using BookShop.Data.Options;

namespace BookShop.Api.Extensions;

public static class ConfigurationExtensions
{
    public static DbOptions ConfigureDbOptions(this IConfiguration configuration)
    {
        var connString = configuration.GetSection($"{DbOptions.SectionName}:{nameof(DbOptions.ConnectionString)}").Value;

        return new DbOptions { ConnectionString = connString };
    }
}
