using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCookbook.Domain.Extension;
using SmartCookbook.Domain.Repositories;
using SmartCookbook.Domain.Repositories.User;
using SmartCookbook.Infrastructure.RepositoryAccess;
using System.Reflection;

namespace SmartCookbook.Infrastructure;

public static class Bootstrapper
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configurationManager)
    {
        AddFluentMigrator(services, configurationManager);

        AddContext(services, configurationManager);
        AddUnityOfWork(services);
        AddRepositories(services);
    }

    private static void AddFluentMigrator(this IServiceCollection services, IConfiguration configurationManager)
    {
        bool.TryParse(configurationManager.GetSection("Configurations:DatabaseInMemory").Value, out bool databaseInMemory);

        if (!databaseInMemory)
        {
            services.AddFluentMigratorCore().ConfigureRunner(c =>
            {
                c.AddMySql5()
                 .WithGlobalConnectionString(configurationManager.GetFullConnection()).ScanIn(Assembly.Load("SmartCookbook.Infrastructure")).For.All();
            });
        }
    }

    private static void AddContext(IServiceCollection services, IConfiguration configurationManager)
    {
        bool.TryParse(configurationManager.GetSection("Configurations:DatabaseInMemory").Value, out bool databaseInMemory);

        if (!databaseInMemory)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));
            var connectionString = configurationManager.GetFullConnection();

            services.AddDbContext<SmartCookbookContext>(dbContextOpts =>
            {
                dbContextOpts.UseMySql(connectionString, serverVersion);
            });
        }

    }

    private static void AddUnityOfWork(IServiceCollection services)
    {
        services.AddScoped<IUnityOfWork, UnityOfWork>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>()
                .AddScoped<IUserReadOnlyRepository, UserRepository>()
                .AddScoped<IUserUpdateOnlyRepository, UserRepository>();
    }
}
