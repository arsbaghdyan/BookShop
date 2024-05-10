using BookShop.Api.Extensions;
using BookShop.Api.Mapping;
using BookShop.Api.Middlewares;
using BookShop.Api.Services;
using BookShop.Data.Extensions;
using BookShop.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

var dbOption = builder.Configuration.ConfigureDbOptions();
var jwtOption = builder.Configuration.ConfigureJwtOptions();

builder.Services.AddHostedService<DatabaseMigrationService>();
builder.Services.AddSingleton(dbOption);
builder.Services.AddSingleton(jwtOption);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAllServices();
builder.Services.AddBookShopDbContext(dbOption);
builder.Services.AddTransient<GlobalExceptionHandler>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.SwaggerConfiguration();
builder.Services.JwtConfiguration(jwtOption);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
