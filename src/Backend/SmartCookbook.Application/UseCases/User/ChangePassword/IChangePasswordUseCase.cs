using SmartCookbook.Comunicacao.Request;

namespace SmartCookbook.Application.UseCases.User.ChangePassword;
public interface IChangePasswordUseCase
{
    Task Execute(ChangePasswordRequestJson request);
}
