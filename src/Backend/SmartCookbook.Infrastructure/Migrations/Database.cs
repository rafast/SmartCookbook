using Dapper;
using MySqlConnector;

namespace SmartCookbook.Infrastructure.Migrations;

public static class Database
{
    public static void CreateDatabase(string connectionString, string databaseName)
    {
        using var conn = new MySqlConnection(connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("name", databaseName);

        var entities = conn.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @name", parameters);

        if (!entities.Any())
        {
            conn.Execute($"CREATE DATABASE {databaseName}");
        }
    }
}
