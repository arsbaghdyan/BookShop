using BookShop.Api.Middlewares;
using BookShop.Services.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace BookShop.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection JwtConfiguration(this IServiceCollection services, JwtOptions jwtOption)
    {
        var key = Encoding.ASCII.GetBytes(jwtOption.Key);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOption.Issuer,
                        ValidAudience = jwtOption.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

        return services;
    }

    public static IServiceCollection SwaggerConfiguration(this IServiceCollection services)
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
    {
        services.AddTransient<GlobalExceptionHandler>();
        return services;
    }

    public static IServiceCollection AddClientContextMiddleware(this IServiceCollection services)
    {
        services.AddTransient<ClientContextMiddleware>();
        return services;
    }
}
