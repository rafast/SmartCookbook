using SmartCookbook.Comunicacao.Request;
using SmartCookbook.Comunicacao.Response;

namespace SmartCookbook.Application.UseCases.Recipe.Save;

public interface ISaveRecipeUseCase
{
    Task<RecipeResponseJson> Execute(RecipeRequestJson request);
}