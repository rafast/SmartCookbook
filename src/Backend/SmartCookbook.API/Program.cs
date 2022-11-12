using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartCookbook.API.Filters;
using SmartCookbook.API.Middleware;
using SmartCookbook.Application;
using SmartCookbook.Application.Services.Automapper;
using SmartCookbook.Domain.Extension;
using SmartCookbook.Infrastructure;
using SmartCookbook.Infrastructure.Migrations;
using SmartCookbook.Infrastructure.RepositoryAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting(opt => opt.LowercaseUrls = true);
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddMvc(opt =>
{
    opt.Filters.Add(typeof(FilterExceptions));
});

builder.Services.AddScoped(provider => new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutomapperConfig());
}).CreateMapper());

builder.Services.AddScoped<AuthenticatedUser>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

UpdateDatabase();

app.UseMiddleware<CultureMiddleware>();

app.Run();

void UpdateDatabase()
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    using var context = serviceScope.ServiceProvider.GetService<SmartCookbookContext>();

    bool? databaseInMemory = context?.Database?.ProviderName?.Equals("Microsoft.EntityFrameworkCore.InMemory");

    if (!databaseInMemory.HasValue || !databaseInMemory.Value)
    {
        var connection = builder.Configuration.GetConnection();
        var databaseName = builder.Configuration.GetDatabaseName();

        Database.CreateDatabase(connection, databaseName);

        app.MigrateDatabase();
    }

}

public partial class Program { }

