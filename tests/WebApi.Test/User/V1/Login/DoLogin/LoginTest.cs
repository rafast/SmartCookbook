using FluentAssertions;
using SmartCookbook.Comunicacao.Request;
using SmartCookbook.Exceptions;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace WebApi.Test.User.V1.Login.DoLogin;
public class LoginTest : ControllerBase
{
    private const string METHOD = "login";

    private SmartCookbook.Domain.Entities.User _user;
    private string _password;
    public LoginTest(SmartCookbookWebApplicationFactory<Program> factory) : base(factory)
    {
        _user = factory.GetUser();
        _password = factory.GetPassword();
    }

    [Fact]
    public async Task Validate_Success()
    {
        var request = new LoginRequestJson
        {
            Email = _user.Email,
            Password = _password
        };

        var response = await PostRequest(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_user.Name);
    }

    [Fact]
    public async Task Validar_Error_Invalid_Password()
    {
        var request = new LoginRequestJson
        {
            Email = _user.Email,
            Password = "invalidPassword"
        };

        var response = await PostRequest(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("messages").EnumerateArray();
        
        errors.Should().ContainSingle().And.Contain(x => x.GetString().Equals(ResourceErrorMessages.INVALID_LOGIN));
    }

    [Fact]
    public async Task Validar_Error_Invalid_Email()
    {
        var request = new LoginRequestJson
        {
            Email = "invalid@email.com",
            Password = _password
        };

        var response = await PostRequest(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("messages").EnumerateArray();

        errors.Should().ContainSingle().And.Contain(x => x.GetString().Equals(ResourceErrorMessages.INVALID_LOGIN));
    }

    [Fact]
    public async Task Validar_Error_Invalid_Email_And_Password()
    {
        var request = new LoginRequestJson
        {
            Email = "invalid@email.com",
            Password = "invalidPassword"
        };

        var response = await PostRequest(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("messages").EnumerateArray();

        errors.Should().ContainSingle().And.Contain(x => x.GetString().Equals(ResourceErrorMessages.INVALID_LOGIN));
    }
}
