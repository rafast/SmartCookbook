using FluentMigrator;

namespace SmartCookbook.Infrastructure.Migrations.Versions;

[Migration((long)EnumVersions.CriarTabelaReceita, "Cria tabela receitas")]
public class Version0000002 : Migration
{
    public override void Down()
    {        
    }

    public override void Up()
    {
        CreateRecipesTable();
        CreateIngredientsTable();
    }

    private void CreateRecipesTable()
    {
        var table = VersionBase.InsertDefaultColumns(Create.Table("Recipes"));

        table
            .WithColumn("Title").AsString(100).NotNullable()
            .WithColumn("Category").AsInt16().NotNullable()
            .WithColumn("Instructions").AsString(5000).NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Recipe_User_Id", "Users", "Id");
    }
    private void CreateIngredientsTable()
    {
        var table = VersionBase.InsertDefaultColumns(Create.Table("Ingredients"));

        table
            .WithColumn("Name").AsString(100).NotNullable()
            .WithColumn("Quantity").AsString().NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Ingredient_Recipe_Id", "Recipes", "Id")
                                                           .OnDeleteOrUpdate(System.Data.Rule.Cascade);
    }
}
