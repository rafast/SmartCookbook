using Newtonsoft.Json;
using SmartCookbook.Comunicacao.Request;
using SmartCookbook.Exceptions;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace WebApi.Test.User.V1;

public class ControllerBase : IClassFixture<SmartCookbookWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ControllerBase(SmartCookbookWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        ResourceErrorMessages.Culture = CultureInfo.CurrentCulture;
    }

    protected async Task<HttpResponseMessage> PostRequest(string method, object body)
    {
        var jsonString = JsonConvert.SerializeObject(body);
        return await _client.PostAsync(method, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    }

    protected async Task<HttpResponseMessage> PutRequest(string method, object body, string token = "")
    {
        RequestAuthorize(token);
        var jsonString = JsonConvert.SerializeObject(body);
        return await _client.PutAsync(method, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    }

    protected async Task<string> Login(string email, string password)
    {
        var request = new LoginRequestJson
        {
            Email = email,
            Password = password
        };

        var response = await PostRequest("login", request);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        return responseData.RootElement.GetProperty("token").ToString();
    }

    private void RequestAuthorize(string token)
    {
        if (!string.IsNullOrWhiteSpace(token))
        {
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
    }
}
