using FluentAssertions;
using SmartCookbook.Application.UseCases.User.Register;
using SmartCookbook.Exceptions;
using TestsUtility.Requests;
using Xunit;

namespace Validators.Test.User.Register;

public class UserRegisterValidatorTest
{
    [Fact]
    public void Validate_Success()
    {
        var validator = new RegisterUserValidator();

        var request = RegisterUserRequestBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();      
    }

    [Fact]
    public void Validate_Error_Empty_Name()
    {
        var validator = new RegisterUserValidator();

        var request = RegisterUserRequestBuilder.Build();
        request.Name = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_USER_NAME));
    }

    [Fact]
    public void Validate_Error_Empty_Email()
    {
        var validator = new RegisterUserValidator();

        var request = RegisterUserRequestBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_USER_EMAIL));
    }

    [Fact]
    public void Validate_Error_Empty_Password()
    {
        var validator = new RegisterUserValidator();

        var request = RegisterUserRequestBuilder.Build();
        request.Password = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_USER_PASSWORD));
    }

    [Fact]
    public void Validate_Error_Empty_Phone()
    {
        var validator = new RegisterUserValidator();

        var request = RegisterUserRequestBuilder.Build();
        request.Phone = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_USER_PHONE));
    }

    [Fact]
    public void Validate_Error_Invalid_Email()
    {
        var validator = new RegisterUserValidator();

        var request = RegisterUserRequestBuilder.Build();
        request.Email = "email";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.INVALID_USER_EMAIL));
    }

    [Fact]
    public void Validate_Error_Invalid_Phone()
    {
        var validator = new RegisterUserValidator();

        var request = RegisterUserRequestBuilder.Build();
        request.Phone = "32 2231";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.INVALID_USER_PHONE));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Validate_Error_Invalid_Password(int passwordLength)
    {
        var validator = new RegisterUserValidator();

        var request = RegisterUserRequestBuilder.Build(passwordLength);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.INVALID_USER_PASSWORD));
    }


}
