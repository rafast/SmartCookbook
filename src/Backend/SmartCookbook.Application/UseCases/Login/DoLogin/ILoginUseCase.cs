using SmartCookbook.Comunicacao.Request;
using SmartCookbook.Comunicacao.Response;

namespace SmartCookbook.Application.UseCases.Login.DoLogin;

public interface ILoginUseCase
{
    Task<LoginResponseJson> Execute(LoginRequestJson request);
}
