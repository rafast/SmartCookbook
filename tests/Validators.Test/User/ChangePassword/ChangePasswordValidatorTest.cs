using FluentAssertions;
using SmartCookbook.Application.UseCases.User.ChangePassword;
using SmartCookbook.Exceptions;
using TestsUtility.Requests;
using Xunit;

namespace Validators.Test.User.ChangePassword;
public class ChangePasswordValidatorTest
{
    [Fact]
    public void Validate_Success()
    {
        var validator = new ChangePasswordValidator();

        var request = ChangePasswordRequestBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Validate_Error_Invalid_Password(int passwordLength)
    {
        var validator = new ChangePasswordValidator();

        var request = ChangePasswordRequestBuilder.Build(passwordLength);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.INVALID_USER_PASSWORD));
    }

    [Fact]
    public void Validate_Error_Empty_Password()
    {
        var validator = new ChangePasswordValidator();

        var request = ChangePasswordRequestBuilder.Build();
        request.NewPassword = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_USER_PASSWORD));
    }
}
