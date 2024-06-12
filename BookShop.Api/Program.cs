using BookShop.Api.Extensions;
using BookShop.Api.Middlewares;
using BookShop.Data.Extensions;
using BookShop.Services.Extensions;
using BookShop.Services.Mapping;
using BookShop.Common;
using BookShop.Api.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

var dbOptions = builder.Configuration.GetDbOptions();
var clientJwtOptions = builder.Configuration.GetClientJwtOptions();
var adminJwtOptions = builder.Configuration.GetAdminJwtOptions();
var redisOptions = builder.Configuration.GetRedisOptions();
builder.Services.AddSingleton(dbOptions);
builder.Services.AddSingleton(clientJwtOptions);
builder.Services.AddSingleton(adminJwtOptions);
builder.Services.AddSingleton(redisOptions);

builder.Services.AddDatabaseMigrationService();
builder.Services.AddBookShopDbContext(dbOptions);
builder.Services.AddRedisCache(redisOptions);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAllServices();
builder.Services.AddShopAuthentication(clientJwtOptions, adminJwtOptions);
builder.Services.AddSwaggerConfiguration();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddGlobalExceptionHandler();
builder.Services.AddClientContextMiddleware();
builder.Services.AddEmployeeContextMiddleware();
builder.Services.AddClientContext();
builder.Services.AddHealthChecks().AddCheck<RedisHealthCheck>("Redis");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandler>();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ClientContextMiddleware>();

app.UseMiddleware<EmployeeContextMiddleware>();

app.MapControllers();

app.Run();
