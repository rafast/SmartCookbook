using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCookbook.Application.UseCases.Login.DoLogin;
using SmartCookbook.Comunicacao.Request;
using SmartCookbook.Comunicacao.Response;
using System.Threading.Tasks;

namespace SmartCookbook.API.Controllers;

public class LoginController : SmartCookbookControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(LoginResponseJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(
        [FromServices] ILoginUseCase useCase,
        [FromBody] LoginRequestJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}
