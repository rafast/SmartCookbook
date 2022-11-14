using SmartCookbook.Domain.Entities;
using SmartCookbook.Domain.Repositories.Recipe;

namespace SmartCookbook.Infrastructure.RepositoryAccess.Repository;

public class RecipeRepository : IRecipeWriteOnlyRepository
{
    private readonly SmartCookbookContext _context;

    public RecipeRepository(SmartCookbookContext context)
    {
        _context = context;
    }

    public async Task Save(Recipe recipe)
    {
        await _context.Recipes.AddAsync(recipe);
    }
}