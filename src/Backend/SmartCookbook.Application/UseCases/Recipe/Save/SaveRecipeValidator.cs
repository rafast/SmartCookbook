using FluentValidation;
using SmartCookbook.Comunicacao.Request;

namespace SmartCookbook.Application.UseCases.Recipe.Save;

public class SaveRecipeValidator : AbstractValidator<RecipeRequestJson>
{
    public SaveRecipeValidator()
    {
        RuleFor(x => x).SetValidator(new RecipeValidator());
    }
}