using SmartCookbook.Domain.Enum;

namespace SmartCookbook.Domain.Entities;

public class Recipe : EntityBase
{
    public string Title { get; set; }
    public Category Category { get; set; }
    public string Instructions { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; }
    public long UserId { get; set; }
}