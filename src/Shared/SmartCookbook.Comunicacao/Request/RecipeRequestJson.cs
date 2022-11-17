using SmartCookbook.Comunicacao.Enum;

namespace SmartCookbook.Comunicacao.Request;

public class RecipeRequestJson
{
    public RecipeRequestJson()
    {
        Ingredients = new();
    }

    public string Title { get; set; }
    public Category Category { get; set; }
    public string Instructions { get; set; }
    public List<IngredientRequestJson> Ingredients { get; set; }
}