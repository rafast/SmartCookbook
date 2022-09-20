using AutoMapper;
using SmartCookbook.Application.Cryptograph;
using SmartCookbook.Application.Services.Token;
using SmartCookbook.Comunicacao.Request;
using SmartCookbook.Comunicacao.Response;
using SmartCookbook.Domain.Repositories;
using SmartCookbook.Exceptions;
using SmartCookbook.Exceptions.ExceptionsBase;

namespace SmartCookbook.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserWriteOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnityOfWork _unityOfWork;
    private readonly PasswordCryptograph _passwordCryptograph;
    private readonly TokenController _tokenController;

    public RegisterUserUseCase(IUserWriteOnlyRepository repository, IMapper mapper, IUnityOfWork unityOfWork, PasswordCryptograph passwordCryptograph, TokenController tokenController, IUserReadOnlyRepository userReadOnlyRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _unityOfWork = unityOfWork;
        _passwordCryptograph = passwordCryptograph;
        _tokenController = tokenController;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task<ResponseUserRegisteredJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);

        var user = _mapper.Map<Domain.Entities.User>(request);

        //criptografar a senha
        user.Password = _passwordCryptograph.Cryptograph(request.Password);

        await _repository.Add(user);
        await _unityOfWork.Commit();

        var token = _tokenController.TokenGen(user.Email);

        return new ResponseUserRegisteredJson
        {
            Token = token
        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();
        var result = validator.Validate(request);

        var existingUserWithEmail = await _userReadOnlyRepository.IsEmailInUse(request.Email);

        if (existingUserWithEmail)
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceErrorMessages.EMAIL_ALREADY_IN_USE));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(err => err.ErrorMessage).ToList();
            throw new ValidationErrorException(errorMessages);
        }
    }
}
