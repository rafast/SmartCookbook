using FluentValidation;
using SmartCookbook.Comunicacao.Request;

namespace SmartCookbook.Application.UseCases.User.ChangePassword;
public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequestJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(c => c.NewPassword).SetValidator(new PasswordValidator()); 
    }
}
