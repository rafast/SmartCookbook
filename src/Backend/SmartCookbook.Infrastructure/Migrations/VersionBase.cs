using FluentMigrator.Builders.Create.Table;

namespace SmartCookbook.Infrastructure.Migrations;

public static class VersionBase
{
    public static ICreateTableColumnOptionOrWithColumnSyntax InsertDefaultColumns(ICreateTableWithColumnOrSchemaOrDescriptionSyntax table)
    {
        return table
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("CreatedAt").AsDateTime().NotNullable();
    }
}
