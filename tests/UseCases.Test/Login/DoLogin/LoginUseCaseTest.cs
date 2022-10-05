using FluentAssertions;
using SmartCookbook.Application.UseCases.Login.DoLogin;
using SmartCookbook.Exceptions;
using SmartCookbook.Exceptions.ExceptionsBase;
using System;
using System.Threading.Tasks;
using TestsUtility.Cryptograph;
using TestsUtility.Entities;
using TestsUtility.Repositories;
using TestsUtility.Token;
using Xunit;

namespace UseCases.Test.Login.DoLogin;
public class LoginUseCaseTest
{
    [Fact]
    public async Task Validate_Sucess()
    {
        (var user, var password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var response = await useCase.Execute(new SmartCookbook.Comunicacao.Request.LoginRequestJson
        {
            Email = user.Email,
            Password = password
        });

        response.Should().NotBeNull();
        response.Name.Should().Be(user.Name);
        response.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Validate_Error_Invalid_Password()
    {
        (var user, var password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> action = async () =>
        {
            await useCase.Execute(new SmartCookbook.Comunicacao.Request.LoginRequestJson
            {
                Email = user.Email,
                Password = "invalidPassword"
            });
        };

        await action.Should().ThrowAsync<InvalidLoginException>()
            .Where(exception => exception.Message.Equals(ResourceErrorMessages.INVALID_LOGIN));
    }

    [Fact]
    public async Task Validate_Error_Invalid_Email()
    {
        (var user, var password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> action = async () =>
        {
            await useCase.Execute(new SmartCookbook.Comunicacao.Request.LoginRequestJson
            {
                Email = "email@invalid.com",
                Password = password
            });
        };

        await action.Should().ThrowAsync<InvalidLoginException>()
            .Where(exception => exception.Message.Equals(ResourceErrorMessages.INVALID_LOGIN));
    }

    [Fact]
    public async Task Validate_Error_Invalid_Email_Password()
    {
        (var user, var password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> action = async () =>
        {
            await useCase.Execute(new SmartCookbook.Comunicacao.Request.LoginRequestJson
            {
                Email = "email@invalid.com",
                Password = "invalidPassword"
            });
        };

        await action.Should().ThrowAsync<InvalidLoginException>()
            .Where(exception => exception.Message.Equals(ResourceErrorMessages.INVALID_LOGIN));
    }

    private LoginUseCase CreateUseCase(SmartCookbook.Domain.Entities.User user)
    {
        var cryptograph = PasswordCryptographBuilder.Instance();
        var token = TokenControllerBuilder.Instance();
        var repositoryReadOnly = UserReadOnlyRepositoryBuilder.Instance().GetByEmailAndPassword(user).Build();

        return new LoginUseCase(repositoryReadOnly, cryptograph, token);
    }
}
