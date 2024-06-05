using BookShop.Api.Extensions;
using BookShop.Api.Middlewares;
using BookShop.Data.Extensions;
using BookShop.Services.Extensions;
using BookShop.Services.Mapping;
using BookShop.Common;

var builder = WebApplication.CreateBuilder(args);

var dbOptions = builder.Configuration.GetDbOptions();
var clientJwtOptions = builder.Configuration.GetClientJwtOptions();
var adminJwtOptions = builder.Configuration.GetAdminJwtOptions();
builder.Services.AddSingleton(dbOptions);
builder.Services.AddSingleton(clientJwtOptions);
builder.Services.AddSingleton(adminJwtOptions);

builder.Services.AddDatabaseMigrationService();
builder.Services.AddBookShopDbContext(dbOptions);
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ClientContextMiddleware>();

app.UseMiddleware<EmployeeContextMiddleware>();

app.MapControllers();

app.Run();
