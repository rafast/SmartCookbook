using FluentAssertions;
using SmartCookbook.Comunicacao.Request;
using SmartCookbook.Exceptions;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TestsUtility.Requests;
using Xunit;

namespace WebApi.Test.User.V1.ChangePassword;
public class ChangePasswordTest : ControllerBase
{
    private const string METHOD = "user/change-password";

    private SmartCookbook.Domain.Entities.User _user;
    private string _password;

    public ChangePasswordTest(SmartCookbookWebApplicationFactory<Program> factory) : base(factory)
    {
        _user = factory.GetUser();
        _password = factory.GetPassword();
    }

    [Fact]
    public async Task Validate_Success()
    {
        var request = ChangePasswordRequestBuilder.Build();
        request.OldPassword = _password;

        var token = await Login(_user.Email, _password);

        var response = await PutRequest(METHOD, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Validate_Error_EmptyNewPassword()
    {
        var request = new ChangePasswordRequestJson
        {
            OldPassword = _password,
            NewPassword = string.Empty
        };

        var token = await Login(_user.Email, _password);

        var response = await PutRequest(METHOD, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var respondeBody = await response.Content.ReadAsStreamAsync();

        var respondeData = await JsonDocument.ParseAsync(respondeBody);

        var errors = respondeData.RootElement.GetProperty("messages").EnumerateArray();
        errors.Should().ContainSingle().And.Contain(x => x.GetString().Equals(ResourceErrorMessages.EMPTY_USER_PASSWORD));
    }
}
