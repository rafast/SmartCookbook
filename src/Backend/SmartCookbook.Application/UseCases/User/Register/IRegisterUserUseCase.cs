using SmartCookbook.Comunicacao.Request;
using SmartCookbook.Comunicacao.Response;

namespace SmartCookbook.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    Task<ResponseUserRegisteredJson> Execute(RequestRegisterUserJson request);
}
