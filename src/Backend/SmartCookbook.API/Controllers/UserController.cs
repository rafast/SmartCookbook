using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCookbook.Application.UseCases.User.Register;
using SmartCookbook.Comunicacao.Request;
using SmartCookbook.Comunicacao.Response;
using System.Threading.Tasks;

namespace SmartCookbook.API.Controllers;

public class UserController : SmartCookbookControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseUserRegisteredJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterUser(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }
}
