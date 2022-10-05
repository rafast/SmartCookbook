using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartCookbook.Infrastructure.RepositoryAccess;
using System.Linq;

namespace WebApi.Test;

public class SmartCookbookWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private SmartCookbook.Domain.Entities.User user;
    private string password;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(SmartCookbookContext));

                if (descriptor != null)
                    services.Remove(descriptor);

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<SmartCookbookContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var scopeService = scope.ServiceProvider;

                var database = scopeService.GetRequiredService<SmartCookbookContext>();

                database.Database.EnsureDeleted();

                (user, password) = ContextSeedInMemory.Seed(database);
            });
    }

    public SmartCookbook.Domain.Entities.User GetUser()
    {
        return user;
    }

    public string GetPassword()
    {
        return password;
    }
}
