using FluentValidation;
using SmartCookbook.Comunicacao.Request;
using SmartCookbook.Exceptions;
using System.Text.RegularExpressions;

namespace SmartCookbook.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_USER_NAME);
        RuleFor(c => c.Email).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_USER_EMAIL);
        RuleFor(c => c.Phone).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_USER_PHONE);
        RuleFor(c => c.Password).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_USER_PASSWORD);
        When(c => !string.IsNullOrWhiteSpace(c.Email), () =>
        {
            RuleFor(c => c.Email).EmailAddress().WithMessage(ResourceErrorMessages.INVALID_USER_EMAIL);
        });
        When(c => !string.IsNullOrWhiteSpace(c.Password), () =>
        {
            RuleFor(c => c.Password.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceErrorMessages.INVALID_USER_PASSWORD);
        });
        When(c => !string.IsNullOrWhiteSpace(c.Phone), () =>
        {
            RuleFor(c => c.Phone).Custom((phone, context) =>
            {
                string phonePattern = "[0-9]{2} [1-9]{1} [0-9]{4}-[0-9]{4}";
                var isMatch = Regex.IsMatch(phone, phonePattern);
                if (!isMatch)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(phone), ResourceErrorMessages.INVALID_USER_PHONE));
                }
            });
        });

    }
}
