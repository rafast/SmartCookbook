using SmartCookbook.Application.Cryptograph;
using SmartCookbook.Application.Services.Token;
using SmartCookbook.Comunicacao.Request;
using SmartCookbook.Comunicacao.Response;
using SmartCookbook.Domain.Repositories;

namespace SmartCookbook.Application.UseCases.Login.DoLogin;

public class LoginUseCase : ILoginUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly PasswordCryptograph _passwordCryptograph;
    private readonly TokenController _tokenController;

    public LoginUseCase(IUserReadOnlyRepository userReadOnlyRepository, PasswordCryptograph passwordCryptograph, TokenController tokenController)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _passwordCryptograph = passwordCryptograph;
        _tokenController = tokenController;
    }

    public async Task<LoginResponseJson> Execute(LoginRequestJson request)
    {
        var cryptographPassword = _passwordCryptograph.Cryptograph(request.Password);

        var user = await _userReadOnlyRepository.RecuperaPorEmailSenha(request.Email, cryptographPassword);

        if (user == null)
            throw new Exception();

        return new LoginResponseJson
        {
            Name = user.Name,
            Token = _tokenController.TokenGen(user.Email)
        };
    }
}
