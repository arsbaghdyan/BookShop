using BookShop.Data.Options;
using BookShop.Services.Options;

namespace BookShop.Api.Extensions;

public static class ConfigurationExtensions
{
    public static DbOptions ConfigureDbOptions(this IConfiguration configuration)
    {
        var connString = configuration.GetSection($"{DbOptions.SectionName}:{nameof(DbOptions.ConnectionString)}").Value;

        return new DbOptions { ConnectionString = connString };
    }

    public static JwtOptions ConfigureJwtOptions(this IConfiguration configuration)
    {
        var key = configuration.GetSection($"{JwtOptions.SectionName}:{nameof(JwtOptions.Key)}").Value;
        var issuer = configuration.GetSection($"{JwtOptions.SectionName}:{nameof(JwtOptions.Issuer)}").Value;
        var audience = configuration.GetSection($"{JwtOptions.SectionName}:{nameof(JwtOptions.Audience)}").Value;

        return new JwtOptions { Key = key!, Issuer = issuer!, Audience = audience! };
    }
}
