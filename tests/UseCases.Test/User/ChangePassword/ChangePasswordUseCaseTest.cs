using FluentAssertions;
using SmartCookbook.Application.UseCases.User.ChangePassword;
using SmartCookbook.Comunicacao.Request;
using SmartCookbook.Exceptions;
using SmartCookbook.Exceptions.ExceptionsBase;
using System;
using System.Threading.Tasks;
using TestsUtility.Cryptograph;
using TestsUtility.CurrentUser;
using TestsUtility.Entities;
using TestsUtility.Repositories;
using TestsUtility.Requests;
using Xunit;

namespace UseCases.Test.User.ChangePassword;
public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Validate_Success()
    {
        (var user,  var password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var request = ChangePasswordRequestBuilder.Build();
        request.OldPassword = password;

        Func<Task> action = async () =>
        {
            await useCase.Execute(request);
        };

        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Validate_Error_Invalid_OldPassword()
    {
        (var user, var password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var request = ChangePasswordRequestBuilder.Build();
        request.OldPassword = "invalidPassword";

        Func<Task> action = async () =>
        {
            await useCase.Execute(request);
        };

        await action.Should().ThrowAsync<ValidationErrorException>()
            .Where(ex => ex.ErrorMessages.Count == 1 && ex.ErrorMessages.Contains(ResourceErrorMessages.INVALID_OLDPASSWORD));
    }

    [Fact]
    public async Task Validate_Error_Empty_NewPassword()
    {
        (var user, var password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> action = async () =>
        {
            await useCase.Execute(new ChangePasswordRequestJson()
            {
                OldPassword = password,
                NewPassword = String.Empty
            });
        };

        await action.Should().ThrowAsync<ValidationErrorException>()
            .Where(ex => ex.ErrorMessages.Count == 1 && ex.ErrorMessages.Contains(ResourceErrorMessages.EMPTY_USER_PASSWORD));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Validate_Error_Invalid_NewPassword_Lenght(int passwordLenght)
    {
        (var user, var password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var request = ChangePasswordRequestBuilder.Build(passwordLenght);
        request.OldPassword = password;

        Func<Task> action = async () =>
        {
            await useCase.Execute(request);
        };

        await action.Should().ThrowAsync<ValidationErrorException>()
            .Where(ex => ex.ErrorMessages.Count == 1 && ex.ErrorMessages.Contains(ResourceErrorMessages.INVALID_USER_PASSWORD));
    }

    private ChangePasswordUseCase CreateUseCase(SmartCookbook.Domain.Entities.User user)
    {
        var currentUser = CurrentUserBuilder.Instance().GetCurrentUser(user).Build();
        var repository = UserUpdateOnlyRepositoryBuilder.Instance().GetById(user).Build();
        var unityOfWork = UnityOfWorkBuilder.Instance().Build();
        var cryptograph = PasswordCryptographBuilder.Instance();

        return new ChangePasswordUseCase(currentUser, repository, cryptograph, unityOfWork);
    }
}
