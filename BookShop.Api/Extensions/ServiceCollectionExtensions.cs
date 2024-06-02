using BookShop.Api.Constants;
using BookShop.Api.Middlewares;
using BookShop.Api.Services;
using BookShop.Services.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace BookShop.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddShopAuthentication(this IServiceCollection services,
        ClientJwtOptions clientJwtOption,
        AdminJwtOptions adminJwtOption)
    {
        var clientKey = Encoding.ASCII.GetBytes(clientJwtOption.Key);
        var adminKey = Encoding.ASCII.GetBytes(adminJwtOption.Key);

        services.AddAuthentication()
                .AddJwtBearer(AuthSchemas.ClientFlow, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = clientJwtOption.Issuer,
                        ValidAudience = clientJwtOption.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(clientKey)
                    };
                })
                .AddJwtBearer(AuthSchemas.AdminFlow, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = adminJwtOption.Issuer,
                        ValidAudience = adminJwtOption.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(adminKey)
                    };
                });

        return services;
    }

    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
        });

        return services;
    }

    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
        => services.AddTransient<GlobalExceptionHandler>();

    public static IServiceCollection AddClientContextMiddleware(this IServiceCollection services)
        => services.AddTransient<ClientContextMiddleware>();

    public static IServiceCollection AddDatabaseMigrationService(this IServiceCollection services)
        => services.AddHostedService<DatabaseMigrationService>();
}
