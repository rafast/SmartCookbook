using FluentAssertions;
using SmartCookbook.Application.UseCases.User.Register;
using SmartCookbook.Exceptions;
using SmartCookbook.Exceptions.ExceptionsBase;
using System;
using System.Threading.Tasks;
using TestsUtility.Cryptograph;
using TestsUtility.Mapper;
using TestsUtility.Repositories;
using TestsUtility.Requests;
using TestsUtility.Token;
using Xunit;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{

    [Fact]
    public async void Validate_Success()
    {
        var request = RegisterUserRequestBuilder.Build();

        var useCase = CreateUseCase();

        var response = await useCase.Execute(request);

        response.Should().NotBeNull();
        response.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async void Validate_Error_Email_In_Use()
    {
        var request = RegisterUserRequestBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        Func<Task> action = async () => { await useCase.Execute(request);  };

        await action.Should().ThrowAsync<ValidationErrorException>()
            .Where(exception => exception.ErrorMessages.Count == 1 &&
                                exception.ErrorMessages.Contains(ResourceErrorMessages.EMAIL_ALREADY_IN_USE));
    }

    [Fact]
    public async void Validate_Error_Empty_Email()
    {
        var request = RegisterUserRequestBuilder.Build();
        request.Email = string.Empty;

        var useCase = CreateUseCase();

        Func<Task> action = async () => { await useCase.Execute(request); };

        await action.Should().ThrowAsync<ValidationErrorException>()
            .Where(exception => exception.ErrorMessages.Count == 1 &&
                                exception.ErrorMessages.Contains(ResourceErrorMessages.EMPTY_USER_EMAIL));
    }

    private RegisterUserUseCase CreateUseCase(string email = "")
    {
        var mapper = MapperBuilder.Instance();
        var repository = UserWriteOnlyRepositoryBuilder.Instance().Build();
        var unityOfWork = UnityOfWorkBuilder.Instance().Build();
        var cryptograph = PasswordCryptographBuilder.Instance();
        var token = TokenControllerBuilder.Instance();
        var repositoryReadOnly = UserReadOnlyRepositoryBuilder.Instance().IsEmailInUse(email).Build();

        return new RegisterUserUseCase(repository, mapper, unityOfWork, cryptograph, token, repositoryReadOnly);
    }
}
