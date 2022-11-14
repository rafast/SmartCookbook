namespace SmartCookbook.Domain.Entities;

public class Ingredient : EntityBase
{
    public string Name { get; set; }
    public string Quantity { get; set; }
    public long RecipeId { get; set; }
}