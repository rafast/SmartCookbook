using SmartCookbook.Comunicacao.Enum;

namespace SmartCookbook.Comunicacao.Response;

public class RecipeResponseJson
{
    public string Id { get; set; }
    public string Title { get; set; }
    public Category Category { get; set; }
    public string Instructions { get; set; }
    public List<IngredientResponseJson> Ingredients { get; set; }
}