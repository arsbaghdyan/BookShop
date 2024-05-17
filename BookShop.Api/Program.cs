using BookShop.Api.Extensions;
using BookShop.Api.Middlewares;
using BookShop.Data.Extensions;
using BookShop.Services.Extensions;
using BookShop.Services.Mapping;
using BookShop.Common.ClientService;

var builder = WebApplication.CreateBuilder(args);

var dbOption = builder.Configuration.ConfigureDbOptions();
var jwtOption = builder.Configuration.ConfigureJwtOptions();
builder.Services.AddSingleton(dbOption);
builder.Services.AddSingleton(jwtOption);

builder.Services.AddDatabaseMigrationService();
builder.Services.AddBookShopDbContext(dbOption);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAllServices();
builder.Services.AddJwtConfiguration(jwtOption);
builder.Services.AddSwaggerConfiguration();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddGlobalExceptionHandler();
builder.Services.AddClientContextMiddleware();
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

app.MapControllers();

app.Run();
