using FluentValidation;
using SmartCookbook.Comunicacao.Request;
using SmartCookbook.Domain.Enum;
using SmartCookbook.Domain.Extension;
using SmartCookbook.Exceptions;

namespace SmartCookbook.Application.UseCases.Recipe;

public class RecipeValidator : AbstractValidator<RecipeRequestJson>
{
    public RecipeValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_USER_EMAIL);
        RuleFor(x => x.Category).IsInEnum().WithMessage("");
        RuleFor(x => x.Instructions).NotEmpty().WithMessage("");
        RuleFor(x => x.Ingredients).NotEmpty().WithMessage("");
        RuleForEach(x => x.Ingredients).ChildRules(ingredient =>
        {
            ingredient.RuleFor(x => x.Name).NotEmpty().WithMessage("");
            ingredient.RuleFor(x => x.Quantity).NotEmpty().WithMessage("");
        });

        RuleFor(x => x.Ingredients).Custom((ingredients, context) =>
        {
            var distinctIngredients = ingredients.Select(i => i.Name.IgnoreNonSpace().ToLower().Distinct());
            if(ingredients.Count != distinctIngredients.Count())
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure("Ingredients", "Error message"));
            }
        });
    }
}