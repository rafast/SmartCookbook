using FluentValidation;
using SmartCookbook.Exceptions;

namespace SmartCookbook.Application.UseCases.User;
public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(password => password).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_USER_PASSWORD);
        When(c => !string.IsNullOrWhiteSpace(c), () =>
        {
            RuleFor(c => c.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceErrorMessages.INVALID_USER_PASSWORD);
        });
    }
}
