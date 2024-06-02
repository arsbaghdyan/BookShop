using BookShop.Data.Options;
using BookShop.Services.Options;

namespace BookShop.Api.Extensions;

public static class ConfigurationExtensions
{
    public static DbOptions GetDbOptions(this IConfiguration configuration)
    {
        var connString = configuration.GetSection($"{DbOptions.SectionName}:{nameof(DbOptions.ConnectionString)}").Value;

        return new DbOptions { ConnectionString = connString };
    }

    public static ClientJwtOptions GetClientJwtOptions(this IConfiguration configuration)
    {
        var key = configuration.GetSection($"{ClientJwtOptions.SectionName}:{nameof(ClientJwtOptions.Key)}").Value;
        var issuer = configuration.GetSection($"{ClientJwtOptions.SectionName}:{nameof(ClientJwtOptions.Issuer)}").Value;
        var audience = configuration.GetSection($"{ClientJwtOptions.SectionName}:{nameof(ClientJwtOptions.Audience)}").Value;

        return new ClientJwtOptions { Key = key!, Issuer = issuer!, Audience = audience! };
    }

    public static AdminJwtOptions GetAdminJwtOptions(this IConfiguration configuration)
    {
        var key = configuration.GetSection($"{AdminJwtOptions.SectionName}:{nameof(AdminJwtOptions.Key)}").Value;
        var issuer = configuration.GetSection($"{AdminJwtOptions.SectionName}:{nameof(AdminJwtOptions.Issuer)}").Value;
        var audience = configuration.GetSection($"{AdminJwtOptions.SectionName}:{nameof(AdminJwtOptions.Audience)}").Value;

        return new AdminJwtOptions { Key = key!, Issuer = issuer!, Audience = audience! };
    }
}
