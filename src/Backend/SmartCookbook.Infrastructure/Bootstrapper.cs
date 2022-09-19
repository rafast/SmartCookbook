using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCookbook.Domain.Extension;
using System.Reflection;

namespace SmartCookbook.Infrastructure;

public static class Bootstrapper
{
    public static void AddRepository(this IServiceCollection services, IConfiguration configurationManager)
    {
        AddFluentMigrator(services, configurationManager);
    }

    private static void AddFluentMigrator(this IServiceCollection services, IConfiguration configurationManager)
    {
        services.AddFluentMigratorCore().ConfigureRunner(c =>
        {
            c.AddMySql5()
            .WithGlobalConnectionString(configurationManager.GetFullConnection()).ScanIn(Assembly.Load("SmartCookbook.Infrastructure")).For.All();
        });
    }
}
