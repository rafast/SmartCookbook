﻿using Microsoft.Extensions.Configuration;

namespace SmartCookbook.Domain.Extension;

public static class RepositoryExtension
{
    public static string GetDatabaseName(this IConfiguration configurationManager)
    {
        var databaseName = configurationManager.GetConnectionString("DatabaseName");
        return databaseName;
    }

    public static string GetConnection(this IConfiguration configurationManager)
    {
        var connectionString = configurationManager.GetConnectionString("Connection");
        return connectionString;
    }

    public static string GetFullConnection(this IConfiguration configurationManager)
    {
        var databaseName = configurationManager.GetDatabaseName();
        var connection = configurationManager.GetConnection();

        return $"{connection}Database={databaseName}";
    }
}
