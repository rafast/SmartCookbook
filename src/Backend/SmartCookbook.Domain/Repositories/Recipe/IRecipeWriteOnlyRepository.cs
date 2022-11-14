namespace SmartCookbook.Domain.Repositories.Recipe;

public interface IRecipeWriteOnlyRepository
{
    Task Save(Entities.Recipe recipe);
}