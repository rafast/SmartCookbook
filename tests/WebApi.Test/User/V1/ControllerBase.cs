using Newtonsoft.Json;
using SmartCookbook.Exceptions;
using System.Globalization;
using System.Net.Http;
using System.Text;
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
}
